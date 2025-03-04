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
    [SerializeField] EquipmentDialog equipmentDialog;

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
            equipmentDialog.Setup(equipment);
        }
    }

    public void OnPointerEnter()
    {
        if (equipment != null)
        {
            equipmentDialog.ShowDialog(true);
            StartCoroutine(Targetfoucs(true));
        }
    }

    public void OnPointerExit()
    {
        if (equipment != null)
        {
            equipmentDialog.ShowDialog(false);
            StartCoroutine(Targetfoucs(false));
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

    public IEnumerator Targetfoucs(bool focusFlg)
    {
        float time = 0.05f;
        float currentTime = 0f;
        if (focusFlg)
        {
            Vector3 originalScale = transform.localScale;
            Vector3 targetScale = new Vector3(1.1f, 1.1f, 1.1f);
            while (currentTime < time)
            {
                transform.localScale = Vector3.Lerp(originalScale, targetScale, currentTime / time);
                currentTime += Time.deltaTime;
                yield return null;
            }
            transform.localScale = targetScale;
        }
        else
        {
            Vector3 originalScale = transform.localScale;
            Vector3 targetScale = new Vector3(1, 1, 1);
            while (currentTime < time)
            {
                transform.localScale = Vector3.Lerp(originalScale, targetScale, currentTime / time);
                currentTime += Time.deltaTime;
                yield return null;
            }
            transform.localScale = targetScale;
        }
    }
}
