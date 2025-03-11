using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class DeckPanel : Panel
{
    [SerializeField] CommandSlot commandPrefab;  // CommandSlotのプレハブ
    [SerializeField] GameObject storage;
    [SerializeField] TextMeshProUGUI storageRatio;
    [SerializeField] DeckWindow deckWindow;
    [SerializeField] BattleUnit playerUnit;

    private int headHeight = 105;
    private int commandWidth = 70;
    int row = 5;
    int padding = 0;
    List<CommandSlot> storageList = new List<CommandSlot>();

    private void OnEnable()
    {
        if (playerUnit != null && playerUnit.Battler != null)
        {
            deckWindow.SetUp(playerUnit.Battler);
            SetStorage();
        }
        else
        {
            Debug.LogWarning("playerUnit or its properties are not initialized.");
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

    public void SetStorage()
    {
        storageRatio.text = $"{playerUnit.Battler.StorageList.Count}/{playerUnit.Battler.Storage.val}";

        storageList.Clear();
        ClearTransformChildren(storage.transform);

        foreach (Command command in playerUnit.Battler.StorageList)
        {
            CommandSlot commandSlot = Instantiate(commandPrefab, storage.transform);
            commandSlot.gameObject.SetActive(true);
            commandSlot.OnEndDragAction += ArrengeStorage; // 正しく登録
            commandSlot.Setup(command);
            storageList.Add(commandSlot);
        }
    }

    public void RemoveCommand(CommandSlot commandSlot)
    {
        playerUnit.Battler.StorageList.Remove(commandSlot.command);
        SetStorage();
    }

    private void ArrengeStorage()
    {
        Debug.Log("ArrengeStorage");
    }

    private void ClearTransformChildren(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }
}
