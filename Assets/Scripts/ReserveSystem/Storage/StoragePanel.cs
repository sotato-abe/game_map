using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class StoragePanel : Panel, IDropHandler
{
    [SerializeField] CommandBlock commandPrefab;  // CommandBlockのプレハブ
    [SerializeField] GameObject blockPrefab;  // blockのプレハブ
    [SerializeField] GameObject storageArea;
    [SerializeField] TextMeshProUGUI storageRatio;
    [SerializeField] DeckWindow deckWindow;
    [SerializeField] RectTransform inventoryRectTransform;
    [SerializeField] BattleUnit playerUnit;

    private Battler playerBattler;

    private int headHeight = 20;
    private int defaultItemWidth = 70;
    int row = 10;
    int padding = 10;
    List<CommandBlock> storageList = new List<CommandBlock>();
    private List<GameObject> blockList = new List<GameObject>();

    public void Start()
    {
        playerBattler = playerUnit.Battler;
        SetPanelSize();
    }

    private void OnEnable()
    {
        if (playerUnit != null && playerUnit.Battler != null)
        {
            playerBattler = playerUnit.Battler;
            deckWindow.Setup(playerBattler);
            SetPanelSize();
            SetStorage();
        }
        else
        {
            Debug.LogWarning("playerUnit or its properties are not initialized.");
        }
    }

    // CommandBlockがドロップされたときの処理
    public void OnDrop(PointerEventData eventData)
    {
        CommandBlock droppedCommandBlock = eventData.pointerDrag.GetComponent<CommandBlock>();

        if (droppedCommandBlock != null)
        {
            if (playerBattler.StorageList.Contains(droppedCommandBlock.command))
            {
                Debug.Log("コマンドはすでにデッキに存在しています。");
                return; // 追加しない
            }
            if (playerBattler.StorageList.Count >= playerBattler.Storage.val)
            {
                Debug.Log("ストレージがいっぱいです。");
                return; // 追加しない
            }

            AddCommandBlock(droppedCommandBlock.command); // バッグに追加
            deckWindow.RemoveCommand(droppedCommandBlock.command);
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
        int width = defaultItemWidth * row + 30;
        int column = (playerBattler.Storage.val - 1) / row + 1;
        int height = defaultItemWidth * column + headHeight;
        inventoryRectTransform.sizeDelta = new Vector2(width, height);
    }

    public void SetStorage()
    {
        storageList.Clear();
        foreach (Transform child in storageArea.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Command command in playerBattler.StorageList)
        {
            CommandBlock commandSlot = Instantiate(commandPrefab, storageArea.transform);
            commandSlot.gameObject.SetActive(true);
            commandSlot.OnEndDragAction += ArrengeStorage; // 正しく登録
            commandSlot.OnDeleteCommand += RemoveCommandBlock; // 正しく登録
            commandSlot.Setup(command);
            storageList.Add(commandSlot);
        }
        SetBlock();
        ArrengeStorage();
        storageRatio.text = $"{playerBattler.StorageList.Count}/{playerBattler.Storage.val}";
    }

    private void SetBlock()
    {
        int blockNum = (row - (playerBattler.Storage.val % row)) % row;
        blockList.Clear();
        for (int i = 0; i < blockNum; i++)
        {
            GameObject blockObject = Instantiate(blockPrefab, storageArea.transform);
            blockObject.gameObject.SetActive(true);
            blockList.Add(blockObject);
        }
    }

    public void AddCommandBlock(Command command)
    {
        playerBattler.StorageList.Add(command);
        SetStorage();
    }

    public void RemoveCommandBlock(Command command)
    {
        playerBattler.StorageList.Remove(command);
        SetStorage();
    }

    private void ArrengeStorage()
    {
        storageList.RemoveAll(command => command == null); // 破棄されたオブジェクトを削除
        for (int i = 0; i < storageList.Count; i++)
        {
            int cardHalfWidth = defaultItemWidth / 2;
            int xPosition = (i % row) * defaultItemWidth + cardHalfWidth + padding;
            int yPosition = -((i / row) * defaultItemWidth + cardHalfWidth) - padding;
            storageList[i].transform.localPosition = new Vector3(xPosition, yPosition, 0);
        }

        for (int i = 0; i < blockList.Count; i++)
        {
            int cardHalfWidth = defaultItemWidth / 2;
            int xPosition = (playerBattler.Storage.val % row + i) * defaultItemWidth + cardHalfWidth + padding;
            int yPosition = -((playerBattler.Storage.val / row) * defaultItemWidth + cardHalfWidth) - padding;
            blockList[i].transform.localPosition = new Vector3(xPosition, yPosition, 0);
        }
    }
}
