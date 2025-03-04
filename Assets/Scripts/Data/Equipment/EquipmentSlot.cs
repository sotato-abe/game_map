using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour
{
    public Equipment equipment { get; set; }
    [SerializeField] Image image;
    [SerializeField] Image maskImage;
    [SerializeField] Image backImage;
    [SerializeField] EquipmentType equipmentType;
    [SerializeField] EquipmentTypeList equipmentTypeList;

    public void Start()
    {
        SetEquipmentTypeImage();
    }

    public void Setup(Equipment equipment)
    {
        if (equipment.Base.Type == equipmentType)
        {
            this.equipment = equipment;
            Debug.Log($"EquipmentSlot:{equipmentType}:{equipment.Base.Name}");
            maskImage.color = new Color(maskImage.color.r, maskImage.color.g, maskImage.color.b, 1f);
            image.sprite = equipment.Base.Sprite;
        }
    }

    public void RemoveEquipment()
    {
        this.equipment = null;
        maskImage.color = new Color(maskImage.color.r, maskImage.color.g, maskImage.color.b, 0f);
        image.sprite = null;
    }

    public void SetEquipmentTypeImage()
    {
        backImage.sprite = equipmentTypeList.GetIcon(equipmentType);
    }
}
