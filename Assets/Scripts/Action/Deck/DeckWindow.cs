using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class DeckWindow : MonoBehaviour, IDropHandler
{
    [SerializeField] CommandBlock commandPrefab;
    [SerializeField] GameObject blockPrefab;
    [SerializeField] GameObject runTableArea, deckArea;
    [SerializeField] TextMeshProUGUI lifeCostTexe, batteryCostTexe, soulCostTexe, deckRatio;
    [SerializeField] StoragePanel storagePanel;

    private List<CommandBlock> runTableList = new();
    private List<CommandBlock> deckList = new();
    private Battler playerBattler;
    private const int row = 5, headHeight = 155, commandWidth = 70, padding = 10;
    private const float windowWidth = 380f;

    private void OnEnable()
    {
        SetWindowSize();
        UpdateDeckUI();
    }

    public void Setup(Battler battler) => playerBattler = battler;

    private void SetWindowSize()
    {
        int windowRow = (playerBattler.Memory.val + 4) / 5;
        float windowHeight = commandWidth * windowRow + headHeight;
        GetComponent<RectTransform>().sizeDelta = new Vector2(windowWidth, windowHeight);
    }

    public void OnDrop(PointerEventData eventData)
    {
        CommandBlock droppedCommandBlock = eventData.pointerDrag?.GetComponent<CommandBlock>();
        if (droppedCommandBlock == null || playerBattler == null) return;

        if (eventData.pointerEnter?.transform.IsChildOf(runTableArea.transform) == true)
            HandleDrop(droppedCommandBlock, playerBattler.RunTable, playerBattler.DeckList);
        else if (eventData.pointerEnter?.transform.IsChildOf(deckArea.transform) == true)
            HandleDrop(droppedCommandBlock, playerBattler.DeckList, playerBattler.RunTable);
    }

    private void HandleDrop(CommandBlock commandSlot, List<Command> targetList, List<Command> sourceList)
    {
        if (targetList.Contains(commandSlot.command) || targetList.Count >= playerBattler.Memory.val) return;

        targetList.Add(commandSlot.command);
        sourceList.Remove(commandSlot.command);
        storagePanel.RemoveCommandBlock(commandSlot);
        UpdateDeckUI();
    }

    private void UpdateDeckUI()
    {
        PopulateCommandList(runTableArea.transform, runTableList, playerBattler.RunTable);
        PopulateCommandList(deckArea.transform, deckList, playerBattler.DeckList);
        deckRatio.text = $"{playerBattler.RunTable.Count + playerBattler.DeckList.Count}/{playerBattler.Memory.val}";
        CountEnergyCost();
    }

    private void PopulateCommandList(Transform parent, List<CommandBlock> slotList, List<Command> commands)
    {
        slotList.Clear();
        foreach (Transform child in parent) Destroy(child.gameObject);

        foreach (Command command in commands)
        {
            CommandBlock commandSlot = Instantiate(commandPrefab, parent);
            commandSlot.Setup(command);
            commandSlot.OnEndDragAction += () => ArrangeCommands(slotList);
            slotList.Add(commandSlot);
        }
        ArrangeCommands(slotList);
    }

    private void CountEnergyCost()
    {
        int life = 0, battery = 0, soul = 0;
        foreach (var slot in runTableList)
        {
            foreach (Enegy cost in slot.command.CostList)
            {
                switch (cost.type)
                {
                    case EnegyType.Life: life += cost.val; break;
                    case EnegyType.Battery: battery += cost.val; break;
                    case EnegyType.Soul: soul += cost.val; break;
                }
            }
        }
        lifeCostTexe.SetText(life.ToString());
        batteryCostTexe.SetText(battery.ToString());
        soulCostTexe.SetText(soul.ToString());
    }

    private void ArrangeCommands(List<CommandBlock> slotList)
    {
        slotList.RemoveAll(slot => slot == null);
        for (int i = 0; i < slotList.Count; i++)
        {
            int x = (i % row) * commandWidth + commandWidth / 2 + padding;
            int y = -commandWidth / 2 - padding;
            slotList[i].transform.localPosition = new Vector3(x, y, 0);
        }
    }

    public void RemoveCommand(CommandBlock commandSlot)
    {
        playerBattler.DeckList.Remove(commandSlot.command);
        playerBattler.RunTable.Remove(commandSlot.command);
        UpdateDeckUI();
    }
}
