using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessageDialog : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] TextMeshProUGUI text;
    [SerializeField] GameObject itemList;
    [SerializeField] GameObject itemUnitPrefab;  // ItemUnitのプレハブ
    [SerializeField] GameObject commandUnitPrefab;  // ItemUnitのプレハブ
    [SerializeField] GameObject commandList;
    [SerializeField] Image dialogBackground;
    [SerializeField] BattleUnit playerUnit;
    // targetUnit.Battler.Base.Items;
    [SerializeField] float letterPerSecond;


    BattleDialogType battleDialogType;

    public void changeDialogType(BattleAction action)
    {
        switch (action)
        {
            case BattleAction.Talk:
                SetTalkDialog();
                break;
            case BattleAction.Attack:
                SetAttackDialog();
                break;
            case BattleAction.Command:
                SetCommandDialog();
                break;
            case BattleAction.Item:
                SetItemDialog();
                break;
            case BattleAction.Run:
                SetRunDialog();
                break;
        }
    }

    private void SetMessageDialog()
    {
        Debug.Log("SetMessageDialog");
        itemList.SetActive(false);
        commandList.SetActive(false);
        text.gameObject.SetActive(true);
    }

    public IEnumerator TypeDialog(string line)
    {
        text.SetText("");
        Debug.Log($"Line:{line}");
        foreach (char letter in line)
        {
            text.text += letter;
            yield return new WaitForSeconds(letterPerSecond);
        }
        yield return new WaitForSeconds(2f);
    }

    private void SetTalkDialog()
    {
        Debug.Log("SetTalkDialog");
        itemList.SetActive(false);
        commandList.SetActive(false);
        text.gameObject.SetActive(true);
    }

    private void SetAttackDialog()
    {
        Debug.Log("SetTalkDialog");
        itemList.SetActive(false);
        commandList.SetActive(false);
        text.gameObject.SetActive(true);
    }

    private void SetCommandDialog()
    {
        Debug.Log("SetCommandDialog");
        itemList.SetActive(false);
        text.gameObject.SetActive(false);
        commandList.SetActive(true);

        foreach (Transform child in commandList.transform)
        {
            Destroy(child.gameObject);
        }

        bool isFirstCommand = true;

        foreach (var command in playerUnit.Battler.Deck)
        {
            // CommandUnitのインスタンスを生成
            Debug.Log($"command:{command.Base.Name}");
            GameObject commandUnitObject = Instantiate(commandUnitPrefab, commandList.transform);
            commandUnitObject.gameObject.SetActive(true);
            CommandUnit commandUnit = commandUnitObject.GetComponent<CommandUnit>();

            // CommandUnitのSetupメソッドでアイテムデータを設定
            commandUnit.Setup(command);

            if (isFirstCommand)
            {
                targetCommand(commandUnit, true);
                isFirstCommand = false;  // 2回目以降は実行されないように設定
            }
            else
            {
                targetCommand(commandUnit, false);
            }
        }
    }

    public void targetCommand(CommandUnit target, bool focusFlg)
    {
        target.Targetfoucs(focusFlg);
    }

    private void SetItemDialog()
    {
        Debug.Log("SetItemDialog");
        text.gameObject.SetActive(false);
        commandList.SetActive(false);
        itemList.SetActive(true);

        foreach (Transform child in itemList.transform)
        {
            Destroy(child.gameObject);
        }

        bool isFirstItem = true;

        foreach (var item in playerUnit.Battler.Inventory)
        {
            // ItemUnitのインスタンスを生成
            Debug.Log($"item:{item.Base.Name}");
            GameObject itemUnitObject = Instantiate(itemUnitPrefab, itemList.transform);
            itemUnitObject.gameObject.SetActive(true);
            ItemUnit itemUnit = itemUnitObject.GetComponent<ItemUnit>();

            // ItemUnitのSetupメソッドでアイテムデータを設定
            itemUnit.Setup(item);

            if (isFirstItem)
            {
                targetItem(itemUnit, true);
                isFirstItem = false;  // 2回目以降は実行されないように設定
            }
            else
            {
                targetItem(itemUnit, false);
            }
        }
    }

    public void targetItem(ItemUnit target, bool focusFlg)
    {
        target.Targetfoucs(focusFlg);
    }

    private void SetRunDialog()
    {
        Debug.Log("SetRunDialog");
        commandList.SetActive(false);
        itemList.SetActive(false);
        text.gameObject.SetActive(true);
    }

    // 現在使用していない
    public IEnumerator SetTransparency(float alpha)
    {
        // TextMeshProUGUI の透明度を変更
        if (text != null)
        {
            Color textColor = text.color;
            textColor.a = Mathf.Clamp(alpha, 0f, 1f); // 透明度を 0～1 に制限
            text.color = textColor;
        }

        // 背景のImageコンポーネントの透明度を変更
        if (dialogBackground != null)
        {
            Color bgColor = dialogBackground.color;
            bgColor.a = Mathf.Clamp(alpha, 0f, 1f);
            dialogBackground.color = bgColor;
        }

        yield return null;
    }
}
