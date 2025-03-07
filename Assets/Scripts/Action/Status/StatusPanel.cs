using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class StatusPanel : Panel
{
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] CharacterCard characterCard;
    [SerializeField] BattlerEnegyBar life;
    [SerializeField] BattlerEnegyBar battery;
    [SerializeField] BattlerEnegyBar soul;
    [SerializeField] TextMeshProUGUI level;
    [SerializeField] GameObject enegyList;
    [SerializeField] GameObject statusList;
    [SerializeField] GameObject storageList;
    [SerializeField] GameObject enchantList;
    [SerializeField] GameObject abilityList;
    [SerializeField] EnegyIcon enegyCounterPrefab;
    [SerializeField] StatusIcon statusCounterPrefab;
    [SerializeField] EnchantIcon enchantIconPrefab;

    private Battler battler;
    private List<Enegy> enegyCountList = new List<Enegy>();

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
        this.battler = playerUnit.Battler;
        SetCharacterCard();
        SetEnegy();
        SetStatus();
        SetEnchant();
    }

    private void SetCharacterCard()
    {
        characterCard.SetCharacter(battler);
    }

    private void SetEnegy()
    {
        if (enegyCountList.Count > 0)
        {
            enegyCountList.Clear();
        }
        ClearTransformChildren(enegyList.transform);

        this.life.SetEnegy(EnegyType.Life, battler.MaxLife, battler.Life);
        this.battery.SetEnegy(EnegyType.Battery, battler.MaxBattery, battler.Battery);
        this.soul.SetEnegy(EnegyType.Soul, 100, battler.Soul);

        Enegy life = new Enegy(EnegyType.Life, battler.MaxLife);
        Enegy battery = new Enegy(EnegyType.Battery, battler.MaxBattery);
        enegyCountList.Add(life);
        enegyCountList.Add(battery);

        foreach (Enegy enegy in enegyCountList)
        {
            EnegyIcon enegyCounterObject = Instantiate(enegyCounterPrefab, enegyList.transform);
            enegyCounterObject.gameObject.SetActive(true);
            enegyCounterObject.SetCostIcon(enegy);
        }
    }
    private void SetStatus()
    {
        ClearTransformChildren(statusList.transform);
        ClearTransformChildren(storageList.transform);

        level.text = battler.Level.ToString();

        foreach (Status status in battler.StatusList)
        {
            Transform parent = GetTargetParent(status.type);
            if (parent == null) continue;

            StatusIcon statusCounterObject = Instantiate(statusCounterPrefab, parent);
            statusCounterObject.gameObject.SetActive(true);
            statusCounterObject.SetStatusIcon(status);
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
        return (type == StatusType.ATK || type == StatusType.TEC ||
                type == StatusType.DEF || type == StatusType.SPD || type == StatusType.LUK)
                ? statusList.transform
                : (type == StatusType.MMR || type == StatusType.BAG)
                ? storageList.transform
                : null;
    }
    private void SetEnchant()
    {
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

    }
}
