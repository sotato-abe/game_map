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
    [SerializeField] Sprite tradeIcon;

    Color32 reserveColor = new Color32(187, 0, 255, 200);
    Color32 battleColor = new Color32(255, 0, 71, 200);
    Color32 settingColor = new Color32(0, 237, 255, 200);
    Color32 tradeColor = new Color32(18, 192, 65, 200);

    public void SetTitle(TitleType titleType)
    {
        SetActive(true);
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
            case TitleType.Trade:
                iconImage.sprite = tradeIcon;
                nameText.SetText("取引");
                backPanelImage.color = tradeColor;
                break;
            default:
                Debug.LogError("Unknown TitleType: " + titleType);
                break;
        }
    }
}

