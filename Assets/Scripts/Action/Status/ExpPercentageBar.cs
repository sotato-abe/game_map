using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExpPercentageBar : PercentageBar
{
    public void SetExpBar(int levelDifference, int exp)
    {
        StartCoroutine(PlayLevelUpAnimation(levelDifference, exp));
    }

    private IEnumerator PlayLevelUpAnimation(int levelDifference, int exp)
    {
        for (int i = 0; i < levelDifference; i++)
        {
            yield return StartCoroutine(FullBar()); // フルまで行って…
            ResetBar();                     // 0に戻す（レベルアップ演出）
            yield return null;
        }

        float percentage = (float)exp / 100f; // 最後のレベルのEXPバーだけ描画
        SetBar(percentage);
    }
}
