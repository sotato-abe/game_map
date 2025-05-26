using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Blowing : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI messageText;
    [SerializeField] RectTransform backImageRectTransform;
    [SerializeField] Image panelImage;
    [SerializeField] Sprite DefaultBackImage;
    [SerializeField] Sprite SurpriseBackImage;
    [SerializeField] Sprite ThinkingBackImage;
    [SerializeField] Sprite FearBackImage;

    private float paddingHeight = 50f;
    private float paddingWidth = 50f;
    private float maxWidth = 250f;
    private float blowingWidth = 250f;
    private List<TalkMessage> messageList = new List<TalkMessage>();
    private Coroutine fadeCoroutine;
    private Coroutine messageCoroutine;


    private void OnDisable()
    {
        if (messageCoroutine != null)
        {
            messageList.Clear(); // メッセージリストをクリア
            messageText.SetText(""); // テキストをクリア   
            ResizePlate();
            StopCoroutine(messageCoroutine);
            messageCoroutine = null;
        }
    }

    public void AddMessageList(TalkMessage talkMessage)
    {
        messageList.Add(talkMessage);
        if (messageCoroutine == null)
        {
            if (gameObject.activeSelf)
            {
                messageCoroutine = StartCoroutine(TypeMessageList());
            }
        }
    }

    private IEnumerator TypeMessageList()
    {
        while (messageList.Count > 0)
        {
            string message = messageList[0].message; // 先頭のメッセージを取得
            switch (messageList[0].panelType)
            {
                case PanelType.Default:
                    panelImage.sprite = DefaultBackImage; // デフォルトの背景画像を設定
                    break;
                case PanelType.Surprise:
                    panelImage.sprite = SurpriseBackImage; // サプライズの背景画像を設定
                    break;
                case PanelType.Thinking:
                    panelImage.sprite = ThinkingBackImage; // 考え中の背景画像を設定
                    break;
                case PanelType.Fear:
                    panelImage.sprite = FearBackImage; // 恐怖の背景画像を設定
                    break;
                default:
                    panelImage.sprite = DefaultBackImage; // デフォルトの背景画像を設定
                    break;
            }
            yield return TypeDialog(message);
            // messageの文字数によって待ち時間を変更する
            float waitTime = Mathf.Clamp(message.Length * 0.1f, 1f, 5f); // 最小1秒、最大5秒
            yield return new WaitForSeconds(waitTime);
            messageList.RemoveAt(0); // タイプし終わったメッセージを削除
        }
        messageCoroutine = null; // すべてのメッセージが終了したら、コルーチンの参照をクリア
        transform.gameObject.SetActive(false); // すべてのメッセージが終了したら、オブジェクトを非アクティブにする
    }

    public IEnumerator TypeDialog(string line)
    {
        messageText.SetText("");
        // lineの文字数によって横幅を変える。（20文字以上ならmaxWidthでそれ以内なら。その時のサイズ）
        if (line.Length > 15)
        {
            blowingWidth = maxWidth + paddingWidth;
        }
        else
        {
            blowingWidth = line.Length * 21f + paddingWidth;
        }
        foreach (char letter in line)
        {
            messageText.text += letter;
            ResizePlate();
            yield return new WaitForSeconds(0.03f);
        }
    }

    private void ResizePlate()
    {
        if (messageText == null || backImageRectTransform == null)
        {
            Debug.LogError("messageText または backImageRectTransform が null");
            return;
        }

        // TextMeshProのTextにレイアウトを再計算させる
        messageText.ForceMeshUpdate();

        // 横幅を最大値で制限
        // float newWidth = Mathf.Min(messageText.preferredWidth, maxWidth) + paddingWidth;
        float newHeight = messageText.preferredHeight + paddingHeight;
        backImageRectTransform.sizeDelta = new Vector2(blowingWidth, newHeight);
    }
}
