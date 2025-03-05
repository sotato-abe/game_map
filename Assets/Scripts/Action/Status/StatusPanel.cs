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
    [SerializeField] GameObject enegyList;
    [SerializeField] GameObject statusList;
    [SerializeField] GameObject storageList;
    [SerializeField] GameObject abilityList;
    [SerializeField] EnegyIcon enegyIconPrefab;
    // [SerializeField] StatusIcon statusIconPrefab;
    [SerializeField] EnegyIcon enchantIconPrefab;


    private Battler battler;

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
    }

    private void SetCharacterCard()
    {
        characterCard.SetCharacter(battler);
    }

    private void SetEnegy()
    {

    }

    private void SetStatus()
    {

    }

    private void SetEnchant()
    {

    }

    private void SetAbility()
    {

    }
}
