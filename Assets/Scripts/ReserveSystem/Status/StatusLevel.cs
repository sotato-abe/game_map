using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatusLevel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI expText;
    [SerializeField] ExpPercentageBar bar;

    private int currentLevel = 1; // 現在のレベル
    private int currentExp = 0; // 現在の経験値
    public void SetLevel(int level, int exp)
    {
        if (currentLevel != level)
        {
            levelText.text = level.ToString();
        }
        if (currentExp != exp)
        {
            expText.text = exp.ToString() + "/100";
        }
        if (currentLevel == level && currentExp == exp) return; // 変更がない場合は処理しない
        UpdateExpBar(level, exp);
    }

    private void UpdateExpBar(int level, int exp)
    {
        // 以前からのレベル差と新しい経験値を受け渡す
        int levelDifference = level - currentLevel;
        bar.SetExpBar(levelDifference, exp);
        currentLevel = level; // 現在のレベルを更新
        currentExp = exp; // 現在の経験値を更新
    }
}
