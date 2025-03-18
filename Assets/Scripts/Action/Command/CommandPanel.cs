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
    }

    private void OnEnable()
    {
        playerBattler = playerUnit.Battler;
        SetCommandUnit();
        CountEnegyCost();
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
            commandNum++;
        }
    }

    public void CountEnegyCost()
    {
        lifeCost = 0;
        batteryCost = 0;
        soulCost = 0;

        foreach (Command command in playerBattler.RunTable)
        {
            var lifeCheck = playerBattler.Life > command.Base.LifeCost.val + lifeCost;
            var batteryCheck = playerBattler.Battery >= command.Base.BatteryCost.val + batteryCost;
            var soulCheck = playerBattler.Soul >= command.Base.SoulCost.val + soulCost;

            if (lifeCheck && batteryCheck && soulCheck)
            {
                lifeCost += command.Base.LifeCost.val;
                batteryCost += command.Base.BatteryCost.val;
                soulCost += command.Base.SoulCost.val;
            }
        }

        SetEnegyCost();
        SetEnegyCost();
    }

    public void SetEnegyCost()
    {
        lifeCostText.SetText($"{lifeCost}");
        batteryCostText.SetText($"{batteryCost}");
        soulCostText.SetText($"{soulCost}");
    }

    private void ExecuteCommand()
    {
        List<EnchantCount> enchants = ActivateCommands();
        attackSystem.ExecutePlayerCommand(enchants);
        CountEnegyCost();
    }

    public List<EnchantCount> ActivateCommands()
    {
        List<EnchantCount> enchants = new List<EnchantCount>();

        foreach (CommandUnit commandUnit in commandUnitList)
        {
            if (CheckEnegy(commandUnit.Command) == false)
            {
                continue;
            }

            UseEnegy(commandUnit.Command);

            TargetType target = commandUnit.Command.Base.TargetType;

            foreach (var enchant in commandUnit.Command.Base.EnchantList)
            {
                EnchantCount enchantCount = new EnchantCount(enchant.Type, target, enchant.Val);
                enchants.Add(enchantCount);
            }
        }

        return enchants;
    }

    public bool CheckEnegy(Command command)
    {
        if (
            command.Base.LifeCost.val <= playerBattler.Life &&
            command.Base.BatteryCost.val <= playerBattler.Battery &&
            command.Base.SoulCost.val <= playerBattler.Soul
        )
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void UseEnegy(Command command)
    {
        playerBattler.Life -= command.Base.LifeCost.val;
        playerBattler.Battery -= command.Base.BatteryCost.val;
        playerBattler.Soul -= command.Base.SoulCost.val;
        playerUnit.UpdateEnegyUI();
    }
}
