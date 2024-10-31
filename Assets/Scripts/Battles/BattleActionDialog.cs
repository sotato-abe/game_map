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
    [SerializeField] MessageDialog messageDialog;
    private List<SelectableText> actionTexts; // ActionList内のテキストリスト
    BattleAction selectedAction;
    private int previousAction;
    public int selectedIndex;

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        selectedIndex = 0;
        previousAction = 0;
        selectableTexts = GetComponentsInChildren<SelectableText>();
        selectableTexts[0].SetSelectedColor(true);

        // ActionList内のSelectableTextコンポーネントのみを取得
        actionTexts = new List<SelectableText>(actionList.GetComponentsInChildren<SelectableText>());

        if (actionTexts == null || actionTexts.Count == 0)
        {
            Debug.LogError("actionTexts is null or empty. Ensure that there are SelectableText components assigned.");
        }
    }

    public void SetAction(bool prev)
    {
        if (prev)
        {
            selectedIndex++;
        }
        else
        {
            selectedIndex--;
        }
        selectedIndex = Mathf.Clamp(selectedIndex, 0, selectableTexts.Length - 1);
        if (previousAction != selectedIndex)
        {
            for (int i = 0; i < selectableTexts.Length; i++)
            {
                selectableTexts[i].SetSelectedColor(selectedIndex == i);
            }
            messageDialog.changeDialogType((BattleAction)selectedIndex);
            previousAction = selectedIndex;
        }
    }

    // フォントの透明度制御
    public void SetActionValidity(float alpha)
    {
        // SelectableText の透明度を変更
        if (actionTexts != null && actionTexts.Count > 0)
        {
            for (int i = 0; i < selectableTexts.Length; i++)
            {
                selectableTexts[i].SetTextValidity(alpha);
                selectableTexts[i].SetSelectedColor(selectedIndex == i);
            }
        }
        else
        {
            Debug.LogError("actionTexts is still null or empty. Check the ActionList assignment.");
        }
    }
}