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
        CountEnegyVal(enegy);
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

    private void CountEnegyVal(int enegy)
    {
        int diff = enegy - currentEnergy;

        if (diff == 0) return;

        // 差分テキストを生成
        GameObject diffTextObj = new GameObject("DiffText");
        diffTextObj.transform.SetParent(this.transform);
        diffTextObj.transform.localPosition = Vector3.zero;

        TextMeshProUGUI diffText = diffTextObj.AddComponent<TextMeshProUGUI>();
        diffText.fontSize = 24;
        diffText.alignment = TextAlignmentOptions.Center;
        diffText.text = (diff > 0 ? "+" : "") + diff.ToString();
        diffText.color = diff > 0 ? Color.green : Color.red;
        diffText.raycastTarget = false;

        RectTransform rect = diffText.GetComponent<RectTransform>();
        rect.anchoredPosition = Vector2.zero;
        rect.sizeDelta = new Vector2(100, 30);
        rect.localScale = Vector3.one;

        Debug.Log("DiffText: " + diffText.text);
        // 差分テキストをアニメーションで上に移動＆フェードアウト
        StartCoroutine(FadeAndMoveText(diffText));
    }

    private IEnumerator FadeAndMoveText(TextMeshProUGUI text)
    {
        float duration = 1f;
        float elapsed = 0f;
        Vector3 startPos = text.rectTransform.anchoredPosition;
        Vector3 endPos = startPos + new Vector3(0, 50f, 0);

        Color startColor = text.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            text.rectTransform.anchoredPosition = Vector3.Lerp(startPos, endPos, t);
            text.color = new Color(startColor.r, startColor.g, startColor.b, 1 - t);

            yield return null;
        }

        Destroy(text.gameObject);
    }
}

