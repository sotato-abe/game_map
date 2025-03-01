using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour
{
    public Equipment Equipment { get; set; }
    [SerializeField] Image image;
    [SerializeField] EquipmentPart equipmentPart;
    [SerializeField] EquipmentPartList equipmentPartList;

    public void Start()
    {
        SetEquipmentPartImage();
    }

    public virtual void Setup(Equipment equipment)
    {
        // if(parts == equipment.Base.Parts)
        // {
        //     Equipment = equipment;
        //     image.sprite = Equipment.Base.Sprite;
        // }
        Equipment = equipment;
        image.sprite = Equipment.Base.Sprite;
    }

    public void SetEquipmentPartImage()
    {
        image.sprite = equipmentPartList.GetIcon(equipmentPart);
    }
}
