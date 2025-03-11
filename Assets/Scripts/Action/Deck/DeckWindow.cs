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

    private int headHeight = 105;
    private int commandWidth = 70;
    int row = 5;
    int padding = 0;

    public void Start()
    {
        SetDeckSize();
    }

    private void OnEnable()
    {
        SetRunTable();
        SetDeck();
    }

    public void SetUp(Battler battler)
    {
        playerBattler = battler;
    }

    public void OnDrop(PointerEventData eventData)
    {
        CommandSlot droppedCommandSlot = eventData.pointerDrag.GetComponent<CommandSlot>();

        if (droppedCommandSlot != null)
        {
            // すでにデッキに同じアイテムがあるか確認
            if (playerBattler.DeckList.Contains(droppedCommandSlot.command))
            {
                Debug.Log("アイテムはすでにデッキに存在しています。");
                return; // 追加しない
            }
            if (playerBattler.DeckList.Count >= playerBattler.Memory.val)
            {
                Debug.Log("デッキがいっぱいです。");
                return; // 追加しない
            }

            AddCommandSlot(droppedCommandSlot.command); // デッキに追加
            storagePanel.RemoveCommand(droppedCommandSlot); // ストレージから削除
        }
    }

    private void AddCommandSlot(Command command)
    {
        playerBattler.DeckList.Add(command);
        SetDeck();
    }

    private void RemoveCommand(CommandSlot commandSlot)
    {
        playerBattler.DeckList.Remove(commandSlot.command);
        SetDeck();
    }

    public void SetDeckSize()
    {
        Debug.Log("SetDeckSize");
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

    public void SetDeck()
    {
        int commandCount = playerBattler.RunTable.Count + playerBattler.DeckList.Count;
        deckRatio.text = $"{commandCount}/{playerBattler.Memory.val}";

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
    }

    private void ClearTransformChildren(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }
}
