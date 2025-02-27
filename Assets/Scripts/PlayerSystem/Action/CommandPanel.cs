using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CommandPanel : Panel
{
    [SerializeField] GameObject commandUnitPrefab;  // ItemUnitのプレハブ
    [SerializeField] GameObject commandList;
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] TextMeshProUGUI lifeCostText;
    [SerializeField] TextMeshProUGUI batteryCostText;
    [SerializeField] TextMeshProUGUI soulCostText;

    private int lifeCost = 0;
    private int batteryCost = 0;
    private int soulCost = 0;

    int previousCommand;
    int selectedCommand;

    private void Init()
    {
        selectedCommand = 0;
        previousCommand = selectedCommand;
        RefreshEnegyCost();
    }

    private void OnEnable()
    {
        RefreshEnegyCost();
        if (playerUnit != null && playerUnit.Battler != null)
        {
            SetCommandDialog();
            SetEnegyCost();
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
            CountEnegyCost(command);

            if (commandNum == selectedCommand)
            {
                commandUnit.Targetfoucs(true);
            }

            commandNum++;
        }
    }

    public void CountEnegyCost(Command command)
    {
        foreach (Enegy cost in command.Base.CostList)
        {
            switch (cost.type)
            {
                case EnegyType.Life:
                    lifeCost += cost.val;
                    break;
                case EnegyType.Battery:
                    batteryCost += cost.val;
                    break;
                case EnegyType.Soul:
                    soulCost += cost.val;
                    break;
            }
        }
    }

    public void SetEnegyCost()
    {
        lifeCostText.SetText($"{lifeCost}");
        batteryCostText.SetText($"{batteryCost}");
        soulCostText.SetText($"{soulCost}");
    }

    public void RefreshEnegyCost()
    {
        lifeCost = 0;
        batteryCost = 0;
        soulCost = 0;
        lifeCostText.SetText($"{lifeCost}");
        batteryCostText.SetText($"{batteryCost}");
        soulCostText.SetText($"{soulCost}");
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
