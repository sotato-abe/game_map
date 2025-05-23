using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessagePanel : MonoBehaviour
{
    [SerializeField] MessagePrefab messagePrefab;
    [SerializeField] Sprite gameIconSprite;
    [SerializeField] Sprite battleIconSprite;
    [SerializeField] Sprite fieldIconSprite;
    public RectTransform rectTransform;

    private List<Message> messageList = new List<Message>();

    private int messageCount = 7;

    public bool isActive = false;

    public void AddMessage(MessageIconType iconType, string message)
    {
        Sprite messagIcon = MessageDatabase.Instance?.GetIcon(iconType);
        Message newMessage = new Message(messagIcon, message);
        messageList.Add(newMessage);
        TypeMessageList();
    }

    public void SetActive(bool activeFlg)
    {
        if (isActive == activeFlg) return;
        isActive = activeFlg;
        if (isActive)
        {
            StartCoroutine(SlidePanel(new Vector3(50, -170, 0)));
        }
        else
        {
            StartCoroutine(SlidePanel(new Vector3(-800, -170, 0)));
        }
    }

    private IEnumerator SlidePanel(Vector3 targetPosition)
    {
        Vector3 startPosition = rectTransform.anchoredPosition;
        float elapsedTime = 0f;
        float duration = 0.2f;

        while (elapsedTime < duration)
        {
            rectTransform.anchoredPosition = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition = targetPosition;
    }

    public void TypeMessageList()
    {
        // messageListが100件以上なら古いものを削除
        if (messageList.Count > 100)
        {
            messageList.RemoveRange(0, messageList.Count - 100);
        }

        // 表示対象となるメッセージ（後ろから3件）
        int startIndex = Mathf.Max(0, messageList.Count - messageCount);
        int displayCount = messageList.Count - startIndex;

        // すでにあるMessagePrefabを取得
        int existingCount = transform.childCount;

        for (int i = 0; i < displayCount; i++)
        {
            Message message = messageList[startIndex + i];

            if (i < existingCount)
            {
                // 既存のMessagePrefabを使う
                Transform child = transform.GetChild(i);
                MessagePrefab prefab = child.GetComponent<MessagePrefab>();
                prefab.SetMessage(message);
            }
            else
            {
                // 新しく生成する
                MessagePrefab newPrefab = Instantiate(messagePrefab, transform);
                newPrefab.SetMessage(message);
            }
        }

        // 余分なプレハブがあれば非表示または削除する（ここでは削除）
        for (int i = displayCount; i < existingCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }
}
