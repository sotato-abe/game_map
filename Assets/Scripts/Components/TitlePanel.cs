using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TitlePanel : SlidePanel
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] Image iconImage;
    [SerializeField] Sprite reserveIcon;
    [SerializeField] Sprite battleIcon;
    [SerializeField] Sprite configIcon;

    public void SetTitle(TitleType titleType)
    {
        switch (titleType)
        {
            case TitleType.Reserve:
                iconImage.sprite = reserveIcon;
                nameText.SetText("リザーブ");
                break;
            case TitleType.Battle:
                iconImage.sprite = battleIcon;
                nameText.SetText("バトル");
                break;
            case TitleType.Config:
                iconImage.sprite = configIcon;
                nameText.SetText("設定");
                break;
            default:
                Debug.LogError("Unknown TitleType: " + titleType);
                break;
        }
    }
}

