using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitStatusLayer : MonoBehaviour
{
    public Equipment Equipment { get; set; }
    [SerializeField] Image image;
    [SerializeField] Sprite ActiveSprite;
    [SerializeField] Sprite enegydeOutSprite;
    [SerializeField] Sprite stopSprite;
    [SerializeField] Sprite brokenSprite;
    [SerializeField] Sprite mysterySprite;
    [SerializeField] Color activeColor = new Color(81, 255, 0, 200);
    [SerializeField] Color stopColor = new Color(255, 0, 134, 200);

    private UnitStatus unitStatus = UnitStatus.Active;
    public void Setup(UnitStatus status)
    {
        unitStatus = status;
        SetStatusPanel();
    }

    private void SetStatusPanel()
    {
        switch (unitStatus)
        {
            case UnitStatus.Active:
                SetColor(activeColor);
                image.sprite = ActiveSprite;
                break;
            case UnitStatus.EnegyOut:
                SetColor(stopColor);
                image.sprite = enegydeOutSprite;
                break;
            case UnitStatus.Stop:
                SetColor(stopColor);
                image.sprite = stopSprite;
                break;
            case UnitStatus.Broken:
                SetColor(stopColor);
                image.sprite = brokenSprite;
                break;
            default:
                SetColor(stopColor);
                image.sprite = mysterySprite;
                break;
        }
    }

    private void SetColor(Color color)
    {
        image.color = color;
    }
}
