using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessagePanel2 : MonoBehaviour
{
    [SerializeField] MessagePrefab messagePrefab;
    [SerializeField] Sprite gameIconSprite;
    [SerializeField] Sprite battleIconSprite;
    [SerializeField] Sprite fieldIconSprite;

    private List<Message> messageList = new List<Message>();

    private void Awake()
    {
        // foreach (Transform child in transform)
        // {
        //     Destroy(child.gameObject);
        // }
    }

    public void AddGameMessage(string message)
    {
        Message newMessage = new Message(gameIconSprite, message);
        messageList.Add(newMessage);
        TypeMessageList();
    }

    public void AddBattleMesage(string message)
    {
        Message newMessage = new Message(battleIconSprite, message);
        messageList.Add(newMessage);
        TypeMessageList();
    }
    public void AddFieldMesage(string message)
    {
        Message newMessage = new Message(fieldIconSprite, message);
        messageList.Add(newMessage);
        TypeMessageList();
    }

    public void TypeMessageList()
    {
        Debug.Log("メッセージリストの数: " + messageList.Count);
        // messageListが100件以上なら古いものを削除
        if (messageList.Count > 100)
        {
            messageList.RemoveRange(0, messageList.Count - 100);
        }

        // 表示対象となるメッセージ（後ろから3件）
        int startIndex = Mathf.Max(0, messageList.Count - 5);
        int displayCount = messageList.Count - startIndex;

        // すでにあるMessagePrefabを取得
        int existingCount = transform.childCount;

        for (int i = 0; i < displayCount; i++)
        {
            Message message = messageList[startIndex + i];

            if (i < existingCount)
            {
                Debug.Log("test1 : " + existingCount);
                // 既存のMessagePrefabを使う
                Transform child = transform.GetChild(i);
                MessagePrefab prefab = child.GetComponent<MessagePrefab>();
                prefab.SetMessage(message);
            }
            else
            {
                Debug.Log("メッセージの内容: " + message.sprite.name + " " + message.messageText);
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
    }
}
