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

    private Battler playerBattler;
    private int lifeCost = 0;
    private int batteryCost = 0;
    private int soulCost = 0;
    List<CommandUnit> commandUnitList = new List<CommandUnit>();

    public void Start()
    {
        playerBattler = playerUnit.Battler;
        RefreshEnegyCost();
    }

    private void OnEnable()
    {
        RefreshEnegyCost();
        if (playerUnit != null && playerBattler != null)
        {
            playerBattler = playerUnit.Battler;
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

        foreach (var command in playerBattler.RunTable)
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
        List<EnchantCount> enchants = ActivateCommands();
        attackSystem.ExecutePlayerCommand(enchants);
    }

    public List<EnchantCount> ActivateCommands()
    {
        List<EnchantCount> enchants = new List<EnchantCount>();

        foreach (CommandUnit commandUnit in commandUnitList)
        {
            if (CheckEnegy(commandUnit) == false)
            {
                continue;
            }

            UseEnegy(commandUnit);

            TargetType target = commandUnit.Command.Base.TargetType;
            Debug.Log($"target:{target}");

            foreach (var enchant in commandUnit.Command.Base.EnchantList)
            {
                EnchantCount enchantCount = new EnchantCount(enchant.Type, target, enchant.Val);
                enchants.Add(enchantCount);
            }
        }

        return enchants;
    }

    public bool CheckEnegy(CommandUnit commandUnit)
    {
        if (
            commandUnit.Command.Base.LifeCost.val <= playerBattler.Life &&
            commandUnit.Command.Base.BatteryCost.val <= playerBattler.Battery &&
            commandUnit.Command.Base.SoulCost.val <= playerBattler.Soul
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
        playerBattler.Life -= commandUnit.Command.Base.LifeCost.val;
        playerBattler.Battery -= commandUnit.Command.Base.BatteryCost.val;
        playerBattler.Soul -= commandUnit.Command.Base.SoulCost.val;
        playerUnit.UpdateEnegyUI();
    }
}
