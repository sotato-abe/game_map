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
    [SerializeField] Image dialogBackground;
    [SerializeField] float letterPerSecond;


    BattleDialogType battleDialogType;

    public void changeDialogType(BattleDialogType type)
    {
        switch (type)
        {
            case BattleDialogType.Message:
                SetMessageDialog();
                break;
            case BattleDialogType.Command:
                SetCommandDialog();
                break;
            case BattleDialogType.Item:
                SetItemDialog();
                break;
        }
    }

    private void SetMessageDialog()
    {
        Debug.Log("SetMessageDialog");
        itemList.SetActive(false);
        text.gameObject.SetActive(true);
    }

    public IEnumerator TypeDialog(string line)
    {
        text.SetText("");
        foreach (char letter in line)
        {
            text.text += letter;
            yield return new WaitForSeconds(letterPerSecond);
        }
        yield return new WaitForSeconds(2f);
    }

    private void SetCommandDialog()
    {
        Debug.Log("SetCommandDialog");

    }

    private void SetItemDialog()
    {
        Debug.Log("SetItemDialog");
        text.gameObject.SetActive(false);
        itemList.SetActive(true);
    }

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
