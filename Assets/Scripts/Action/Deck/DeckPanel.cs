using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class DeckPanel : Panel
{
    [SerializeField] CommandSlot commandPrefab;  // CommandSlotのプレハブ
    [SerializeField] GameObject runTable;
    [SerializeField] GameObject deck;
    [SerializeField] GameObject storage;
    [SerializeField] BattleUnit playerUnit;


    private void OnEnable()
    {
        if (playerUnit != null && playerUnit.Battler != null)
        {
            SetRunTable();
            SetDeck();
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

    public void SetRunTable()
    {
        foreach (Transform child in runTable.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Command command in playerUnit.Battler.RunTable)
        {
            CommandSlot commandSlot = Instantiate(commandPrefab, runTable.transform);
            commandSlot.gameObject.SetActive(true);
            commandSlot.Setup(command);
        }
    }

    public void SetDeck()
    {
        foreach (Transform child in deck.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Command command in playerUnit.Battler.DeckList)
        {
            CommandSlot commandSlot = Instantiate(commandPrefab, deck.transform);
            commandSlot.gameObject.SetActive(true);
            commandSlot.Setup(command);
        }
    }

    public void SetStorage()
    {
        foreach (Transform child in storage.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Command command in playerUnit.Battler.StorageList)
        {
            CommandSlot commandSlot = Instantiate(commandPrefab, storage.transform);
            commandSlot.gameObject.SetActive(true);
            commandSlot.Setup(command);
        }
    }
}
