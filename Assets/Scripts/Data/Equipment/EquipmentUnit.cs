using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentUnit : MonoBehaviour
{
    public Equipment Equipment { get; set; }
    [SerializeField] Image image;
    [SerializeField] GameObject skillList;
    [SerializeField] GameObject costList;
    [SerializeField] SkillIcon skillPrefab;
    [SerializeField] CostIcon costPrefab;

    public virtual void Setup(Equipment equipment)
    {
        Equipment = equipment;
        image.sprite = Equipment.Base.Sprite;
    }

    private void SetSkill()
    {}

    private void SetCost()
    {}
}
