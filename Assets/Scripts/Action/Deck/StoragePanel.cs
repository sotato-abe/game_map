using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class StoragePanel : Panel, IDropHandler
{
    [SerializeField] CommandSlot commandPrefab;  // CommandSlotのプレハブ
    [SerializeField] GameObject blockPrefab;  // blockのプレハブ
    [SerializeField] GameObject storageArea;
    [SerializeField] TextMeshProUGUI storageRatio;
    [SerializeField] DeckWindow deckWindow;
    [SerializeField] BattleUnit playerUnit;

    private int headHeight = 40;
    private int commandWidth = 70;
    int row = 10;
    int padding = 10;
    List<CommandSlot> storageList = new List<CommandSlot>();
    private List<GameObject> blockList = new List<GameObject>();

    private void OnEnable()
    {
        if (playerUnit != null && playerUnit.Battler != null)
        {
            deckWindow.SetUp(playerUnit.Battler);
            SetPanelSize();
            SetStorage();
        }
        else
        {
            Debug.LogWarning("playerUnit or its properties are not initialized.");
        }
    }

    // CommandSlotがドロップされたときの処理
    public void OnDrop(PointerEventData eventData)
    {
        CommandSlot droppedCommandSlot = eventData.pointerDrag.GetComponent<CommandSlot>();

        if (droppedCommandSlot != null)
        {
            if (playerUnit.Battler.StorageList.Contains(droppedCommandSlot.command))
            {
                Debug.Log("コマンドはすでにデッキに存在しています。");
                return; // 追加しない
            }
            if (playerUnit.Battler.StorageList.Count >= playerUnit.Battler.Storage.val)
            {
                Debug.Log("ストレージがいっぱいです。");
                return; // 追加しない
            }

            AddCommandSlot(droppedCommandSlot.command); // バッグに追加
            deckWindow.RemoveCommand(droppedCommandSlot);
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            isActive = true;
        }

        if (isActive)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnActionExit?.Invoke();
            }
        }
    }

    public void SetPanelSize()
    {
        int width = commandWidth * row + 30;
        int column = (playerUnit.Battler.Storage.val - 1) / row + 1;
        int height = commandWidth * column + headHeight;
        GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
    }

    public void SetStorage()
    {
        storageList.Clear();
        foreach (Transform child in storageArea.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Command command in playerUnit.Battler.StorageList)
        {
            CommandSlot commandSlot = Instantiate(commandPrefab, storageArea.transform);
            commandSlot.gameObject.SetActive(true);
            commandSlot.OnEndDragAction += ArrengeStorage; // 正しく登録
            commandSlot.Setup(command);
            storageList.Add(commandSlot);
        }
        SetBlock();
        ArrengeStorage();
        storageRatio.text = $"{playerUnit.Battler.StorageList.Count}/{playerUnit.Battler.Storage.val}";
    }

    private void SetBlock()
    {
        int blockNum = row - playerUnit.Battler.Storage.val % row;
        blockList.Clear();
        for (int i = 0; i < blockNum; i++)
        {
            GameObject blockObject = Instantiate(blockPrefab, storageArea.transform);
            blockObject.gameObject.SetActive(true);
            blockList.Add(blockObject);
        }
    }

    public void AddCommandSlot(Command command)
    {
        playerUnit.Battler.StorageList.Add(command);
        SetStorage();
    }

    public void RemoveCommandSlot(CommandSlot commandSlot)
    {
        playerUnit.Battler.StorageList.Remove(commandSlot.command);
        SetStorage();
    }

    private void ArrengeStorage()
    {
        Debug.Log("ArrengeStorage");
        storageList.RemoveAll(command => command == null); // 破棄されたオブジェクトを削除
        for (int i = 0; i < storageList.Count; i++)
        {
            int cardHalfWidth = commandWidth / 2;
            int xPosition = (i % row) * commandWidth + cardHalfWidth + padding;
            int yPosition = -cardHalfWidth - padding;
            storageList[i].transform.localPosition = new Vector3(xPosition, yPosition, 0);
        }

        for (int i = 0; i < blockList.Count; i++)
        {
            int cardHalfWidth = commandWidth / 2;
            int xPosition = (playerUnit.Battler.Storage.val % row + i) * commandWidth + cardHalfWidth + padding;
            int yPosition = -((playerUnit.Battler.Storage.val / row) * commandWidth + cardHalfWidth) - padding;
            blockList[i].transform.localPosition = new Vector3(xPosition, yPosition, 0);
        }
    }
}
