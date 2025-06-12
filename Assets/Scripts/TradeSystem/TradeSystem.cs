using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

//　Talk,Attack,Command,Trade,Escape, を管理する。
public class TradeSystem : MonoBehaviour
{
    public UnityAction OnTradeEnd;

    [SerializeField] TradeActionBoard actionBoard;
    [SerializeField] MessagePanel messagePanel;
    [SerializeField] TitlePanel titlePanel;
    [SerializeField] BattleUnit rightUnitPrefab;
    [SerializeField] SlidePanel rightUnitGroup;
    [SerializeField] FieldInfoPanel fieldInfoPanel;
    [SerializeField] FieldSystem fieldSystem;

    private BuildingBase currentBuildingBase;

    void Start()
    {
        transform.gameObject.SetActive(false);
        actionBoard.OnTradeEnd += TradeEnd;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TradeEnd();
        }
    }

    public void TradeStart(BuildingType type)
    {
        transform.gameObject.SetActive(true);
        messagePanel.SetActive(false);
        titlePanel.SetTitle(TitleType.Trade);
        currentBuildingBase = fieldSystem.GetBuildingDataByType(type);
        foreach (Transform child in rightUnitGroup.transform)
        {
            Destroy(child.gameObject);
        }
        BattleUnit battlerUnit = Instantiate(rightUnitPrefab, rightUnitGroup.transform);
        battlerUnit.Setup(currentBuildingBase.Owner);
        battlerUnit.SetMotion(MotionType.Jump);
        battlerUnit.SetBattlerTalkMessage(MessageType.Encount);
        rightUnitGroup.SetActive(true);

        fieldInfoPanel.SetupBuilding(currentBuildingBase);
        fieldInfoPanel.SetActive(true);

        actionBoard.gameObject.SetActive(true);
    }

    public void TradeEnd()
    {
        Debug.Log("TradeSystem : TradeEnd");
        actionBoard.gameObject.SetActive(false);
        fieldSystem.SetFieldPanelData();

        int completed = 0;
        void CheckAllComplete()
        {
            completed++;
            if (completed >= 3)
            {
                OnTradeEnd?.Invoke();
                transform.gameObject.SetActive(false);
            }
        }

        titlePanel.SetActive(false, CheckAllComplete);
        rightUnitGroup.SetActive(false, CheckAllComplete);
        messagePanel.SetActive(true, CheckAllComplete);
    }
}