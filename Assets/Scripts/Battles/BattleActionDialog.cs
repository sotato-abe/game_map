using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleActionDialog : MonoBehaviour
{
    bool selectable;
    SelectableText[] selectableTexts;
    [SerializeField] SelectableText command;
    [SerializeField] GameObject actionList; // ActionListのGameObjectをアサインします
    [SerializeField] Image dialogBackground;
    [SerializeField] BattleSystem battleSystem;

    private List<SelectableText> actionTexts; // ActionList内のテキストリスト
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

        // ActionList内のSelectableTextコンポーネントのみを取得
        actionTexts = new List<SelectableText>(actionList.GetComponentsInChildren<SelectableText>());

        if (actionTexts == null || actionTexts.Count == 0)
        {
            Debug.LogError("actionTexts is null or empty. Ensure that there are SelectableText components assigned.");
        }
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
    public void SetActionValidity(float alpha)
    {
        // SelectableText の透明度を変更
        if (actionTexts != null && actionTexts.Count > 0)
        {
            foreach (var selectableText in actionTexts)
            {
                selectableText.SetTextValidity(alpha);
            }
        }
        else
        {
            Debug.LogError("actionTexts is still null or empty. Check the ActionList assignment.");
        }
    }
}