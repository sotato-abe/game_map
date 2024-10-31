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

    private void OnEnable()
    {
        SetCommandDialog(); // commandPanelがアクティブ化されるたびに実行
    }

    private void SetCommandDialog()
    {
        Debug.Log($"command:{playerUnit.Battler.Base.Name}");

        foreach (Transform child in commandList.transform)
        {
            Destroy(child.gameObject);
        }

        bool isFirstCommand = true;

        foreach (var command in playerUnit.Battler.Deck)
        {
            // CommandUnitのインスタンスを生成
            Debug.Log($"command:{command.Base.Name}");
            GameObject commandUnitObject = Instantiate(commandUnitPrefab, commandList.transform);
            commandUnitObject.gameObject.SetActive(true);
            CommandUnit commandUnit = commandUnitObject.GetComponent<CommandUnit>();

            // CommandUnitのSetupメソッドでアイテムデータを設定
            commandUnit.Setup(command);

            if (isFirstCommand)
            {
                targetCommand(commandUnit, true);
                isFirstCommand = false;  // 2回目以降は実行されないように設定
            }
            else
            {
                targetCommand(commandUnit, false);
            }
        }
    }

    public void targetCommand(CommandUnit target, bool focusFlg)
    {
        target.Targetfoucs(focusFlg);
    }
}
