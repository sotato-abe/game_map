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

        foreach (var item in playerUnit.Battler.Inventory)
        {
            // ItemUnitのインスタンスを生成
            Debug.Log($"item:{item.Base.Name}");
            GameObject itemUnitObject = Instantiate(itemUnitPrefab, itemList.transform);
            itemUnitObject.gameObject.SetActive(true);
            ItemUnit itemUnit = itemUnitObject.GetComponent<ItemUnit>();

            // ItemUnitのSetupメソッドでアイテムデータを設定
            itemUnit.Setup(item);
        }
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
