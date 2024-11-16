using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionPanel : MonoBehaviour
{
    bool selectable;
    [SerializeField] SelectableText selectableTextPrefab;
    [SerializeField] Image dialogBackground;
    [SerializeField] ActionBoard actionBoard;
    private List<SelectableText> actionTexts; // ActionList内のテキストリスト
    ActionType selectedAction;

    private int actionCount = System.Enum.GetValues(typeof(ActionType)).Length;
    public int selectedIndex;

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        selectedIndex = 0;

        // 既存の子オブジェクトを削除
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // アクションの項目を作成
        int actionNum = 0;
        foreach (ActionType value in System.Enum.GetValues(typeof(ActionType)))
        {
            string action = System.Enum.GetName(typeof(ActionType), value);

            SelectableText selectableText = Instantiate(selectableTextPrefab, transform);
            selectableText.gameObject.SetActive(true);
            selectableText.SetText($"{action}");
            actionNum++;
        }

        // ActionList内のSelectableTextコンポーネントのみを取得
        actionTexts = new List<SelectableText>(transform.GetComponentsInChildren<SelectableText>());

        if (actionTexts == null || actionTexts.Count == 0)
        {
            Debug.LogError("actionTexts is null or empty. Ensure that there are SelectableText components assigned.");
        }

        // 初期表示時に中央に選択されたアクションを配置
        ReloadActionList();
    }

    public void SetAction(bool prev)
    {
        if (prev)
        {
            selectedIndex++;
            if (selectedIndex >= actionCount)
            {
                selectedIndex = 0;
            }
        }
        else
        {
            selectedIndex--;
            if (selectedIndex < 0)
            {
                selectedIndex = actionCount - 1;
            }
        }
        ReloadActionList();
        actionBoard.changeDialogType((ActionType)selectedIndex);
    }

    private void ReloadActionList()
    {
        // actionTexts にある SelectableText を更新
        actionTexts = new List<SelectableText>(transform.GetComponentsInChildren<SelectableText>());

        int centerPosition = 2; // リストの中央位置（3番目）
        for (int i = 0; i < actionTexts.Count; i++)
        {
            int targetIndex = GetWrappedIndex(selectedIndex - centerPosition + i, actionCount);

            // 各 SelectableText に対応するアクション名を設定
            actionTexts[i].SetText(System.Enum.GetName(typeof(ActionType), (ActionType)targetIndex));

            // 中央の項目だけを選択状態にする
            actionTexts[i].SetSelectedColor(i == centerPosition);
        }
    }

    // リストのインデックスを折り返して取得するヘルパーメソッド
    private int GetWrappedIndex(int index, int max)
    {
        if (index < 0) return max + (index % max);
        return index % max;
    }

    // フォントの透明度制御
    public void SetActionValidity(float alpha)
    {
        Color bgColor = dialogBackground.color;
        bgColor.a = Mathf.Clamp(alpha, 0f, 1f);
        dialogBackground.color = bgColor;
    }
}
