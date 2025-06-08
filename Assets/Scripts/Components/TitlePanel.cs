using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TitlePanel : SlidePanel
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] Image iconImage;
    [SerializeField] Image backPanelImage;
    [SerializeField] Sprite reserveIcon;
    [SerializeField] Sprite battleIcon;
    [SerializeField] Sprite configIcon;

    Color32 reserveColor = new Color32(187, 0, 255, 200);
    Color32 battleColor = new Color32(255, 0, 71, 200);
    Color32 settingColor = new Color32(0, 237, 255, 200);

    public void SetTitle(TitleType titleType)
    {
        switch (titleType)
        {
            case TitleType.Reserve:
                iconImage.sprite = reserveIcon;
                nameText.SetText("準備");
                backPanelImage.color = reserveColor;
                break;
            case TitleType.Battle:
                iconImage.sprite = battleIcon;
                nameText.SetText("戦闘");
                backPanelImage.color = battleColor;
                break;
            case TitleType.Config:
                iconImage.sprite = configIcon;
                nameText.SetText("設定");
                backPanelImage.color = settingColor;
                break;
            default:
                Debug.LogError("Unknown TitleType: " + titleType);
                break;
        }
    }
}

