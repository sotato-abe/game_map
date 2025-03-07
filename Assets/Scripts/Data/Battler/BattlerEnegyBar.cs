using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattlerEnegyBar : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] Image enegyBar;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] EnegyIconList enegyIconList;

    private int maxEnegy;
    private int currentEnergy;

    public void SetEnegy(EnegyType type, int maxEnegy, int currentEnergy)
    {
        gameObject.SetActive(true);
        icon.sprite = enegyIconList.GetIcon(type);
        this.maxEnegy = maxEnegy;
        this.currentEnergy = currentEnergy;
        text.text = currentEnergy.ToString();
        ChangeEnegyVal(this.currentEnergy);
    }

    public void ChangeEnegyVal(int enegy)
    {
        if (this == null || !gameObject.activeInHierarchy) return; // 破棄されている場合は処理しない

        currentEnergy = enegy;
        text.text = currentEnergy.ToString();

        // エネルギーの割合を計算
        float normalizedValue = (float)currentEnergy / maxEnegy;
        float targetFill = Mathf.Lerp(0.1f, 0.9f, normalizedValue);

        // Coroutineを開始してエネルギーバーを滑らかに変更
        StartCoroutine(SmoothFillChange(targetFill));
    }

    // 0.5秒かけてバーを滑らかに変更
    private IEnumerator SmoothFillChange(float targetFill)
    {
        float startFill = enegyBar.fillAmount;
        float elapsedTime = 0f;
        float duration = 0.5f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            enegyBar.fillAmount = Mathf.Lerp(startFill, targetFill, t);
            yield return null; // 次のフレームまで待機
        }

        // 最後に確実に目標値に設定
        enegyBar.fillAmount = targetFill;
    }

}

