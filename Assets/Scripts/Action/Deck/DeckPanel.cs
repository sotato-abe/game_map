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
    [SerializeField] TextMeshProUGUI lifeCostTexe;
    [SerializeField] TextMeshProUGUI batteryCostTexe;
    [SerializeField] TextMeshProUGUI soulCostTexe;
    [SerializeField] TextMeshProUGUI storageRatio;
    [SerializeField] TextMeshProUGUI deckRatio;
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
        int lifeCost = 0;
        int batteryCost = 0;
        int soulCost = 0;

        ClearTransformChildren(runTable.transform);

        foreach (Command command in playerUnit.Battler.RunTable)
        {
            CommandSlot commandSlot = Instantiate(commandPrefab, runTable.transform);
            commandSlot.gameObject.SetActive(true);
            commandSlot.Setup(command);

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
        lifeCostTexe.SetText(lifeCost.ToString());
        batteryCostTexe.SetText(batteryCost.ToString());
        soulCostTexe.SetText(soulCost.ToString());
    }

    public void SetDeck()
    {
        int commandCount = playerUnit.Battler.RunTable.Count + playerUnit.Battler.DeckList.Count;
        deckRatio.text = $"{commandCount}/{playerUnit.Battler.Memory.val}";

        ClearTransformChildren(deck.transform);

        foreach (Command command in playerUnit.Battler.DeckList)
        {
            CommandSlot commandSlot = Instantiate(commandPrefab, deck.transform);
            commandSlot.gameObject.SetActive(true);
            commandSlot.Setup(command);
        }
    }

    public void SetStorage()
    {
        storageRatio.text = $"{playerUnit.Battler.StorageList.Count}/{playerUnit.Battler.Storage.val}";

        ClearTransformChildren(storage.transform);

        foreach (Command command in playerUnit.Battler.StorageList)
        {
            CommandSlot commandSlot = Instantiate(commandPrefab, storage.transform);
            commandSlot.gameObject.SetActive(true);
            commandSlot.Setup(command);
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
