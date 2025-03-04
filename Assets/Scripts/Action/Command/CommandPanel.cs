using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CommandPanel : Panel
{
    [SerializeField] GameObject commandUnitPrefab;  // CommandUnitのプレハブ
    [SerializeField] GameObject commandList;
    [SerializeField] TextMeshProUGUI lifeCostText;
    [SerializeField] TextMeshProUGUI batteryCostText;
    [SerializeField] TextMeshProUGUI soulCostText;
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] AttackSystem attackSystem;

    private int lifeCost = 0;
    private int batteryCost = 0;
    private int soulCost = 0;
    List<CommandUnit> commandUnitList = new List<CommandUnit>();

    private void Init()
    {
        RefreshEnegyCost();
    }

    private void OnEnable()
    {
        RefreshEnegyCost();
        if (playerUnit != null && playerUnit.Battler != null)
        {
            SetCommandUnit();
            SetEnegyCost();
        }
        else
        {
            Debug.LogWarning("playerUnit or its properties are not initialized.");
        }
    }
    public void Update()
    {
        if (executeFlg)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                ExecuteCommand();
            }
        }
    }

    private void SetCommandUnit()
    {
        commandUnitList.Clear();

        foreach (Transform child in commandList.transform)
        {
            Destroy(child.gameObject);
        }

        int commandNum = 0;

        foreach (var command in playerUnit.Battler.RunTable)
        {
            GameObject commandUnitObject = Instantiate(commandUnitPrefab, commandList.transform);
            commandUnitObject.gameObject.SetActive(true);
            CommandUnit commandUnit = commandUnitObject.GetComponent<CommandUnit>();
            commandUnitList.Add(commandUnit);
            commandUnit.Setup(command);
            CountEnegyCost(command);
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

    private void ExecuteCommand()
    {
        List<Enchant> enchants = ActivateCommands();
        attackSystem.ExecutePlayerCommand(enchants);
    }

    public List<Enchant> ActivateCommands()
    {
        List<Enchant> enchants = new List<Enchant>();

        foreach (CommandUnit commandUnit in commandUnitList)
        {
            if (CheckEnegy(commandUnit) == false)
            {
                continue;
            }

            UseEnegy(commandUnit);

            foreach (var enchant in commandUnit.command.Base.EnchantList)
            {
                enchants.Add(enchant);
            }
        }

        return enchants;
    }

    public bool CheckEnegy(CommandUnit commandUnit)
    {
        if (
            commandUnit.command.Base.LifeCost.val <= playerUnit.Battler.Life &&
            commandUnit.command.Base.BatteryCost.val <= playerUnit.Battler.Battery &&
            commandUnit.command.Base.SoulCost.val <= playerUnit.Battler.Soul
        )
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void UseEnegy(CommandUnit commandUnit)
    {
        playerUnit.Battler.Life -= commandUnit.command.Base.LifeCost.val;
        playerUnit.Battler.Battery -= commandUnit.command.Base.BatteryCost.val;
        playerUnit.Battler.Soul -= commandUnit.command.Base.SoulCost.val;
        playerUnit.UpdateEnegyUI();
    }
}
