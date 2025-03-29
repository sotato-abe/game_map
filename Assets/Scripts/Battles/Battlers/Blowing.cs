using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Blowing : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] float letterPerSecond;
    private RectTransform talkPanelRectTransform;

    private float baseWidth = 100f; // 基本のパネル幅
    private float baseHeight = 40f; // 基本のパネル高さ
    private float maxWidth = 350f; // 最大のパネルの幅

    private void Awake()
    {
        gameObject.SetActive(true); // オブジェクトをアクティブにする
        if (text == null)
        {
            Debug.LogError("Text component is not assigned!");
        }

        // RectTransform のキャッシュを取っておく
        talkPanelRectTransform = GetComponent<RectTransform>();

        if (talkPanelRectTransform == null)
        {
            Debug.LogError("RectTransform component is missing on TalkPanel!");
        }
    }

    public IEnumerator TypeDialog(string line)
    {
        text.SetText("");
        ResizePanel(line); // テキストに合わせてパネルをリサイズ
        transform.gameObject.SetActive(true);
        foreach (char letter in line)
        {
            text.text += letter;
            yield return new WaitForSeconds(letterPerSecond);
        }
        if (gameObject.activeSelf)
        {
            StartCoroutine(CloseTypeDialog());
        }
    }

    public IEnumerator CloseTypeDialog()
    {
        yield return new WaitForSeconds(3f);
        transform.gameObject.SetActive(false);
    }

    private void ResizePanel(string line)
    {
        if (talkPanelRectTransform == null || text == null) return;

        // 現在のフォントサイズとテキストの幅を計算
        float textWidth = text.fontSize * line.Length * 0.5f; // 文字数に応じた幅
        float panelWidth = Mathf.Min(baseWidth + textWidth, maxWidth); // 最大幅を超えないように
        float panelHeight = baseHeight + Mathf.Ceil(textWidth / maxWidth) * text.fontSize * 1.8f; // 行数に応じて高さ増加

        // サイズ適用
        talkPanelRectTransform.sizeDelta = new Vector2(panelWidth, panelHeight);
    }

}
