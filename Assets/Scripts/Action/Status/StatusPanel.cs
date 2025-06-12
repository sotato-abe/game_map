using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class StatusPanel : Panel
{
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] SkillPoint skillPoint;
    [SerializeField] TextMeshProUGUI description;
    [SerializeField] GameObject enegyList;
    [SerializeField] GameObject statusList;
    [SerializeField] GameObject storageList;
    [SerializeField] GameObject enchantList;
    [SerializeField] GameObject abilityList;
    [SerializeField] EnegyCounter enegyCounterPrefab;
    [SerializeField] StatusCounter statusCounterPrefab;
    [SerializeField] AbilityUnit abilityPrefab;
    [SerializeField] EnchantIcon enchantIconPrefab;
    [SerializeField] StatusLevel statusLevel;
    [SerializeField] StatusDialog statusDialog;

    private void Start()
    {
        Setup();
    }

    private void OnEnable()
    {
        Setup();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Quit();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnActionExit?.Invoke();
        }
    }

    private void Quit()
    {
        isActive = false;
        OnActionExecute?.Invoke();
    }

    private void Setup()
    {
        SetLevel();
        SetSkillPoint();
        SetEnegy();
        SetStatus();
        SetEnchant();
        SetAbility();
        description.text = playerUnit.Battler.Base.Description;
        statusDialog.Setup(playerUnit.Battler);
    }

    private void SetEnegy()
    {
        Battler battler = playerUnit.Battler;
        ClearTransformChildren(enegyList.transform);

        foreach (EnegyCount enegyCount in battler.EnegyCountList)
        {
            EnegyCounter enegyCounterObject = Instantiate(enegyCounterPrefab, enegyList.transform);
            enegyCounterObject.gameObject.SetActive(true);
            enegyCounterObject.EnegyUp += EnegyUp;
            enegyCounterObject.SetEnegyCounter(enegyCount);
        }
    }

    private void SetStatus()
    {
        Battler battler = playerUnit.Battler;
        ClearTransformChildren(statusList.transform);
        ClearTransformChildren(storageList.transform);

        foreach (StatusCount status in battler.StatusCountList)
        {
            Transform parent = GetTargetParent(status.type);
            if (parent == null) continue;

            StatusCounter statusCounterObject = Instantiate(statusCounterPrefab, parent);
            statusCounterObject.gameObject.SetActive(true);
            statusCounterObject.StatusUp += StatusUp;
            statusCounterObject.SetStatusCounter(status);
        }
    }

    private void ClearTransformChildren(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }

    private Transform GetTargetParent(StatusType type)
    {
        return (type == StatusType.POW || type == StatusType.TEC ||
                type == StatusType.DEF || type == StatusType.SPD || type == StatusType.LUK)
                ? statusList.transform
                : (type == StatusType.MMR || type == StatusType.BAG || type == StatusType.STG || type == StatusType.POC)
                ? storageList.transform
                : null;
    }
    private void SetEnchant()
    {
        Battler battler = playerUnit.Battler;
        ClearTransformChildren(enchantList.transform);
        foreach (Enchant enchant in battler.Enchants)
        {
            EnchantIcon enchantCounter = Instantiate(enchantIconPrefab, enchantList.transform);
            enchantCounter.gameObject.SetActive(true);
            enchantCounter.SetEnchant(enchant);
        }
    }

    private void SetAbility()
    {
        Battler battler = playerUnit.Battler;
        foreach (Transform child in abilityList.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Ability ability in battler.AbilityList)
        {
            AbilityUnit abilityObject = Instantiate(abilityPrefab, abilityList.transform);
            abilityObject.gameObject.SetActive(true);
            abilityObject.Setup(ability);
        }
    }

    private void SetLevel()
    {
        statusLevel.SetLevel(playerUnit.Battler.Level, playerUnit.Battler.Exp);
    }

    private void SetSkillPoint()
    {
        PlayerBattler battler = playerUnit.Battler as PlayerBattler;
        if (battler.SkillPoint > 0)
        {
            skillPoint.gameObject.SetActive(true);
            skillPoint.SetPoint(battler.SkillPoint);
        }
        else
        {
            skillPoint.gameObject.SetActive(false);
        }
    }

    public void EnegyUp(EnegyType type)
    {
        PlayerBattler battler = playerUnit.Battler as PlayerBattler;
        battler.EnegyUp(type);
        playerUnit.SetEnegy();
        Debug.Log($"Enegy Up: {playerUnit.Battler.ColLife},{playerUnit.Battler.MaxLife}/{playerUnit.Battler.Life}");
        SetSkillPoint();
        SetEnegy();
    }

    public void StatusUp(StatusType type)
    {
        PlayerBattler battler = playerUnit.Battler as PlayerBattler;
        battler.StatusUp(type);
        SetSkillPoint();
        playerUnit.SetStatusDialog();
        SetStatus();
    }
}
