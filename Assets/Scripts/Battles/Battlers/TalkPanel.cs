using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TalkPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] float letterPerSecond;
    private RectTransform talkPanelRectTransform;

    private void Start()
    {
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
        StartCoroutine(CloseTypeDialog());
    }

    public IEnumerator CloseTypeDialog()
    {
        yield return new WaitForSeconds(3f);
        transform.gameObject.SetActive(false);
    }

    private void ResizePanel(string line)
    {
        int charPerLine = 30; // 1行あたりの文字数
        float baseWidth = 100f; // 基本のパネル高さ
        float baseHeight = 80f; // 基本のパネル高さ
        float widthIncrement = 15f; // 20文字ごとに増加する高さ
        float heightIncrement = 30f; // 20文字ごとに増加する高さ

        // テキスト全体の長さを取得
        int totalCharacters = line.Length;

        // 行数を計算（20文字ごとに1行としてカウント）
        int lineCount = Mathf.CeilToInt((float)totalCharacters / charPerLine);

        // 新しい高さを計算
        float newHeight = baseHeight + (lineCount - 1) * heightIncrement; // 基本高さ + 行数に応じた増加
        float newWidth = totalCharacters < charPerLine ? baseWidth + (totalCharacters * widthIncrement) : baseWidth + (charPerLine * widthIncrement); // 基本高さ + 行数に応じた増加

        // パネルのサイズを設定（幅はテキストに基づき、余白を加える）
        if (talkPanelRectTransform != null)
        {
            talkPanelRectTransform.sizeDelta = new Vector2(newWidth, newHeight);
        }
    }
}
