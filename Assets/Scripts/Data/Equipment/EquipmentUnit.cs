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
        SetSkill();
        SetCost();
    }

    private void SetSkill()
    {
        foreach (var skill in Equipment.Base.SkillList)
        {
            SkillIcon skillObject = Instantiate(skillPrefab, skillList.transform);
            skillObject.gameObject.SetActive(true);
            SkillIcon skillUnit = skillObject.GetComponent<SkillIcon>();

            skillUnit.SetSkillIcon(skill);
        }
    }

    private void SetCost()
    {
        foreach (var cost in Equipment.Base.CostList)
        {
            if (0 < cost.val)
            {
                CostIcon costObject = Instantiate(costPrefab, costList.transform);
                costObject.gameObject.SetActive(true);
                CostIcon costUnit = costObject.GetComponent<CostIcon>();
                costUnit.SetCostIcon(cost);
            }
        }
    }
}
