using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentUnitStatus : MonoBehaviour
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

    private EquipmentStatus equipmentStatus = EquipmentStatus.Active;
    public void Setup(EquipmentStatus status)
    {
        equipmentStatus = status;
        SetStatusPanel();
    }

    private void SetStatusPanel()
    {
        switch (equipmentStatus)
        {
            case EquipmentStatus.Active:
                SetColor(activeColor);
                image.sprite = ActiveSprite;
                break;
            case EquipmentStatus.EnegyOut:
                SetColor(stopColor);
                image.sprite = enegydeOutSprite;
                break;
            case EquipmentStatus.Stop:
                SetColor(stopColor);
                image.sprite = stopSprite;
                break;
            case EquipmentStatus.Broken:
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
