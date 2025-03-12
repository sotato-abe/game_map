using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DeckWindow : MonoBehaviour, IDropHandler
{
    [SerializeField] CommandSlot commandPrefab;  // CommandSlotのプレハブ
    [SerializeField] GameObject blockPrefab;  // blockのプレハブ
    [SerializeField] GameObject runTableArea;
    [SerializeField] GameObject deckArea;

    [SerializeField] TextMeshProUGUI lifeCostTexe;
    [SerializeField] TextMeshProUGUI batteryCostTexe;
    [SerializeField] TextMeshProUGUI soulCostTexe;
    [SerializeField] TextMeshProUGUI deckRatio;
    [SerializeField] StoragePanel storagePanel;

    List<CommandSlot> runTableList = new List<CommandSlot>();
    List<CommandSlot> deckList = new List<CommandSlot>();

    private Battler playerBattler;

    int row = 5;
    private int headHeight = 260;
    private int commandWidth = 70;
    int padding = 10;

    public void Start()
    {
        SetWindowSize();
    }

    private void OnEnable()
    {
        SetWindowSize();
        SetRunTable();
        SetDeck();
    }

    public void OnDrop(PointerEventData eventData)
    {
        CommandSlot droppedCommandSlot = eventData.pointerDrag.GetComponent<CommandSlot>();

        if (droppedCommandSlot == null) return;

        if (eventData.pointerEnter != null)
        {
            Debug.Log("OnDrop");
            // RunTableにドロップされた場合
            if (eventData.pointerEnter.transform.IsChildOf(runTableArea.transform))
            {
                Debug.Log("RunTableにドロップされました。");
                if (playerBattler.RunTable.Contains(droppedCommandSlot.command))
                {
                    Debug.Log("コマンドはすでにランテーブルに存在しています。");
                    return;
                }
                if (playerBattler.DeckList.Count >= playerBattler.Memory.val)
                {
                    Debug.Log("メモリがいっぱいです。");
                    return;
                }
                AddRunTableCommand(droppedCommandSlot.command);
                playerBattler.DeckList.Remove(droppedCommandSlot.command);
                storagePanel.RemoveCommandSlot(droppedCommandSlot);
                SetRunTable();
                SetDeck();
                return;
            }
            // Deckにドロップされた場合
            else if (eventData.pointerEnter.transform.IsChildOf(deckArea.transform))
            {
                Debug.Log("Deckにドロップされました。");
                if (playerBattler.DeckList.Contains(droppedCommandSlot.command))
                {
                    Debug.Log("コマンドはすでにデッキに存在しています。");
                    return;
                }
                if (playerBattler.DeckList.Count >= playerBattler.Memory.val)
                {
                    Debug.Log("メモリがいっぱいです。");
                    return;
                }
                AddDeckCommand(droppedCommandSlot.command);
                playerBattler.RunTable.Remove(droppedCommandSlot.command);
                storagePanel.RemoveCommandSlot(droppedCommandSlot);
                SetRunTable();
                SetDeck();
                return;
            }
        }
    }

    public void SetUp(Battler battler)
    {
        playerBattler = battler;
    }

    public void SetWindowSize()
    {
        int width = commandWidth * row + 30;
        int column = (playerBattler.Memory.val - 1) / row + 1;
        int height = commandWidth * column + headHeight;
        GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
    }

    public void SetDeck()
    {
        deckList.Clear();
        ClearTransformChildren(deckArea.transform);

        foreach (Command command in playerBattler.DeckList)
        {
            CommandSlot commandSlot = Instantiate(commandPrefab, deckArea.transform);
            commandSlot.gameObject.SetActive(true);
            commandSlot.OnEndDragAction += ArrengeDeck; // 正しく登録
            commandSlot.Setup(command);
            deckList.Add(commandSlot);
        }

        int commandCount = playerBattler.RunTable.Count + playerBattler.DeckList.Count;
        deckRatio.text = $"{commandCount}/{playerBattler.Memory.val}";
        ArrengeDeck();
    }

    private void AddRunTableCommand(Command command)
    {
        playerBattler.RunTable.Add(command);
        SetRunTable();
    }

    private void AddDeckCommand(Command command)
    {
        playerBattler.DeckList.Add(command);
        SetDeck();
    }

    public void RemoveCommand(CommandSlot commandSlot)
    {
        playerBattler.DeckList.Remove(commandSlot.command);
        playerBattler.RunTable.Remove(commandSlot.command);
        SetRunTable();
        SetDeck();
    }

    public void SetRunTable()
    {
        int lifeCost = 0;
        int batteryCost = 0;
        int soulCost = 0;

        runTableList.Clear();

        ClearTransformChildren(runTableArea.transform);

        foreach (Command command in playerBattler.RunTable)
        {
            CommandSlot commandSlot = Instantiate(commandPrefab, runTableArea.transform);
            commandSlot.gameObject.SetActive(true);
            commandSlot.OnEndDragAction += ArrengeRunTable; // 正しく登録
            commandSlot.Setup(command);
            runTableList.Add(commandSlot);
        }
        ArrengeRunTable();
        CountEnegyCost();
    }

    private void CountEnegyCost()
    {
        int lifeCost = 0;
        int batteryCost = 0;
        int soulCost = 0;

        foreach (CommandSlot commandSlot in runTableList)
        {
            foreach (Enegy cost in commandSlot.command.Base.CostList)
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
        lifeCostTexe.SetText(lifeCost.ToString());
        batteryCostTexe.SetText(batteryCost.ToString());
        soulCostTexe.SetText(soulCost.ToString());
    }

    private void ArrengeRunTable()
    {
        Debug.Log("ArrengeRunTable");
        runTableList.RemoveAll(command => command == null); // 破棄されたオブジェクトを削除
        for (int i = 0; i < runTableList.Count; i++)
        {
            int cardHalfWidth = commandWidth / 2;
            int xPosition = (i % row) * commandWidth + cardHalfWidth + padding;
            int yPosition = -cardHalfWidth - padding;
            runTableList[i].transform.localPosition = new Vector3(xPosition, yPosition, 0);
        }
    }

    private void ArrengeDeck()
    {
        Debug.Log("ArrengeDeck");
        deckList.RemoveAll(command => command == null); // 破棄されたオブジェクトを削除
        for (int i = 0; i < deckList.Count; i++)
        {
            int cardHalfWidth = commandWidth / 2;
            int xPosition = (i % row) * commandWidth + cardHalfWidth + padding;
            int yPosition = -cardHalfWidth - padding;
            deckList[i].transform.localPosition = new Vector3(xPosition, yPosition, 0);
        }
    }

    private void ClearTransformChildren(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }
}
