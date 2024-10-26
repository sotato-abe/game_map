using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleActionDialog : MonoBehaviour
{
    bool selectable;
    SelectableText[] selectableTexts;
    [SerializeField] BattleSystem battleSystem;

    BattleAction selectedAction;
    public int selectedIndex;

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        selectedIndex = 0;
        selectableTexts = GetComponentsInChildren<SelectableText>();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectedIndex++;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectedIndex--;
        }

        selectedIndex = Mathf.Clamp(selectedIndex, 0, selectableTexts.Length - 1);

        for (int i = 0; i < selectableTexts.Length; i++)
        {
            selectableTexts[i].SetSelectedColor(selectedIndex == i);
        }
    }

    public void HandleUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(battleSystem.ActionExecution());
        }
    }

    // フォントの透明度制御
    public void SetActionValidity(bool validityFlg)
    {
        for (int i = 0; i < selectableTexts.Length; i++)
        {
            selectableTexts[i].SetTextValidity(validityFlg); // 各要素に対して個別に呼び出し
        }
    }
}