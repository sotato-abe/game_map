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
    List<CommandUnit> commandUnitList = new List<CommandUnit>();

    private int headWidth = 145;
    private int commandWidth = 90;
    private int panelHeight = 150;

    public void Start()
    {
        playerBattler = playerUnit.Battler;
    }

    private void OnEnable()
    {
        playerBattler = playerUnit.Battler;
        SetPanelSize();
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

    public void SetPanelSize()
    {
        int row = playerBattler.Memory.val;
        int width = commandWidth * row + headWidth;
        GetComponent<RectTransform>().sizeDelta = new Vector2(width, panelHeight);
    }

    private void SetCommandUnit()
    {
        commandUnitList.Clear();

        foreach (Transform child in commandList.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (var command in playerBattler.RunTable)
        {
            GameObject commandUnitObject = Instantiate(commandUnitPrefab, commandList.transform);
            commandUnitObject.gameObject.SetActive(true);
            CommandUnit commandUnit = commandUnitObject.GetComponent<CommandUnit>();
            commandUnitList.Add(commandUnit);
            commandUnit.Setup(command);
        }
    }

    public void CountEnegyCost()
    {
        int lifeCost = 0;
        int batteryCost = 0;
        int soulCost = 0;

        foreach (CommandUnit commandUnit in commandUnitList)
        {
            var lifeCheck = playerBattler.Life > commandUnit.Command.Base.LifeCost.val + lifeCost;
            var batteryCheck = playerBattler.Battery >= commandUnit.Command.Base.BatteryCost.val + batteryCost;
            var soulCheck = playerBattler.Soul >= commandUnit.Command.Base.SoulCost.val + soulCost;

            if (lifeCheck && batteryCheck && soulCheck)
            {
                lifeCost += commandUnit.Command.Base.LifeCost.val;
                batteryCost += commandUnit.Command.Base.BatteryCost.val;
                soulCost += commandUnit.Command.Base.SoulCost.val;
            }
            else
            {
                commandUnit.SetStatus(UnitStatus.EnegyOut);
            }
        }

        lifeCostText.SetText($"{lifeCost}");
        batteryCostText.SetText($"{batteryCost}");
        soulCostText.SetText($"{soulCost}");
    }

    public void ExecuteCommand()
    {
        if (executeFlg)
        {
            List<Attack> attacks = new List<Attack>();
            foreach (CommandUnit commandUnit in commandUnitList)
            {
                if (CheckEnegy(commandUnit.Command) == false)
                {
                    continue;
                }

                UseEnegy(commandUnit.Command);
                attacks.Add(commandUnit.Command.Attack);
            }
            attackSystem.ExecutePlayerAttack(attacks);
            CountEnegyCost();
        }
    }

    public bool CheckEnegy(Command command)
    {
        int life = Mathf.Max(0, playerBattler.Life);
        int battery = Mathf.Max(0, playerBattler.Battery);
        int soul = Mathf.Max(0, playerBattler.Soul);

        return
            command.Base.LifeCost.val <= life &&
            command.Base.BatteryCost.val <= battery &&
            command.Base.SoulCost.val <= soul;
    }

    public void UseEnegy(Command command)
    {
        playerBattler.Life -= command.Base.LifeCost.val;
        playerBattler.Battery -= command.Base.BatteryCost.val;
        playerBattler.Soul -= command.Base.SoulCost.val;
        playerUnit.UpdateEnegyUI();
    }
}
