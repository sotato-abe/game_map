using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Blowing : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI messageText;
    [SerializeField] RectTransform backImageRectTransform;

    private float padding = 52f;

    private List<string> messageList = new List<string>();
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

    public void AddMesageList(string message)
    {
        messageList.Add(message);
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
            string message = messageList[0]; // 先頭のメッセージを取得
            yield return TypeDialog(message);
            yield return new WaitForSeconds(2f);
            messageList.RemoveAt(0); // タイプし終わったメッセージを削除
        }
        messageCoroutine = null; // すべてのメッセージが終了したら、コルーチンの参照をクリア
        transform.gameObject.SetActive(false); // すべてのメッセージが終了したら、オブジェクトを非アクティブにする
    }

    public IEnumerator TypeDialog(string line)
    {
        messageText.SetText("");
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

        float newHeight = messageText.preferredHeight + padding;
        backImageRectTransform.sizeDelta = new Vector2(backImageRectTransform.sizeDelta.x, newHeight);
    }
}
