using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CommandPanel : MonoBehaviour
{
    [SerializeField] GameObject commandUnitPrefab;  // ItemUnitのプレハブ
    [SerializeField] GameObject commandList;
    [SerializeField] BattleUnit playerUnit;

    int previousCommand;
    int selectedCommand;

    private void Init()
    {
        selectedCommand = 0;
        previousCommand = selectedCommand;
    }

    private void OnEnable()
    {
        if (playerUnit != null && playerUnit.Battler != null)
        {
            SetCommandDialog();
        }
        else
        {
            Debug.LogWarning("playerUnit or its properties are not initialized.");
        }
    }

    private void SetCommandDialog()
    {

        foreach (Transform child in commandList.transform)
        {
            Destroy(child.gameObject);
        }

        int commandNum = 0;

        foreach (var command in playerUnit.Battler.Deck)
        {
            // CommandUnitのインスタンスを生成
            GameObject commandUnitObject = Instantiate(commandUnitPrefab, commandList.transform);
            commandUnitObject.gameObject.SetActive(true);
            CommandUnit commandUnit = commandUnitObject.GetComponent<CommandUnit>();
            commandUnit.Setup(command);

            if (commandNum == selectedCommand)
            {
                commandUnit.Targetfoucs(true);
            }

            commandNum++;
        }
    }

    public void SelectCommand(bool selectDirection)
    {
        if (selectDirection)
        {
            selectedCommand++;
        }
        else
        {
            selectedCommand--;
        }
        selectedCommand = Mathf.Clamp(selectedCommand, 0, playerUnit.Battler.Deck.Count - 1);

        if (commandList.transform.childCount > 0 && previousCommand != selectedCommand)
        {
            var previousCommandUnit = commandList.transform.GetChild(previousCommand).GetComponent<CommandUnit>();
            previousCommandUnit.Targetfoucs(false);
            var currentCommandUnit = commandList.transform.GetChild(selectedCommand).GetComponent<CommandUnit>();
            currentCommandUnit.Targetfoucs(true);
            previousCommand = selectedCommand;
        }
    }
}
