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

    private List<Message> messageList = new List<Message>();

    private int messageCount = 10;

    private void Awake()
    {
        // foreach (Transform child in transform)
        // {
        //     Destroy(child.gameObject);
        // }
    }

    public void AddMessage(MessageIconType iconType, string message)
    {
        Sprite messagIcon = MessageDatabase.Instance?.GetIcon(iconType);
        Message newMessage = new Message(messagIcon, message);
        messageList.Add(newMessage);
        TypeMessageList();
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
