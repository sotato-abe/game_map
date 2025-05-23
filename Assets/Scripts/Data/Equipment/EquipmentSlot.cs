using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour
{
    [SerializeField] Image plusIcon;
    [SerializeField] Image blockIcon;
    [SerializeField] EquipmentType equipmentType;
    [SerializeField] EquipmentTypeList equipmentTypeList;
    [SerializeField] bool isBlock = false;

    //Setupより前に実行したい
    protected void Awake()
    {
        plusIcon.gameObject.SetActive(true);
        blockIcon.gameObject.SetActive(false);
        if (isBlock)
        {
            plusIcon.gameObject.SetActive(false);
            blockIcon.gameObject.SetActive(true);
        }
        SetEquipmentType();
    }

    private void SetEquipmentType()
    {
        plusIcon.sprite = equipmentTypeList.GetIcon(equipmentType);
    }

    public void ReSetSlot()
    {
        //自身にアタッチされているEquipmentBlockを削除
        EquipmentBlock equipmentBlock = transform.GetComponentInChildren<EquipmentBlock>();
        if (equipmentBlock != null)
        {
            Destroy(equipmentBlock.gameObject);
        }
    }

    public void ArrangeEquipmentBlock()
    {
        EquipmentBlock equipmentBlock = transform.GetComponentInChildren<EquipmentBlock>();
        if (equipmentBlock != null)
        {
            RectTransform parentRect = GetComponent<RectTransform>();
            RectTransform childRect = equipmentBlock.GetComponent<RectTransform>();
            // アンカーを中央に設定（必要であれば）
            childRect.anchorMin = new Vector2(0.5f, 0.5f);
            childRect.anchorMax = new Vector2(0.5f, 0.5f);
            childRect.pivot = new Vector2(0.5f, 0.5f);

            // 中央揃え
            childRect.anchoredPosition = Vector2.zero;
        }
    }
}
