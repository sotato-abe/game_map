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

    public void SetEquipmentMotion(EquipmentUnitMotionType motion)
    {
        if (this == null || !gameObject.activeInHierarchy) return; // 破棄されている場合は処理しない

        switch (motion)
        {
            case EquipmentUnitMotionType.Jump:
                StartCoroutine(JumpMotion());
                break;
        }
    }

    private IEnumerator JumpMotion()
    {
        float bounceHeight = 40f;
        float damping = 0.5f;
        float gravity = 5000f;
        float groundY = transform.position.y;

        while (bounceHeight >= 0.1f)
        {
            float verticalVelocity = Mathf.Sqrt(2 * gravity * bounceHeight);
            bool isFalling = false;

            while (transform.position.y >= groundY || !isFalling)
            {
                if (this == null) yield break; // 途中でオブジェクトが破棄されたら終了

                verticalVelocity -= gravity * Time.deltaTime;
                transform.position += Vector3.up * verticalVelocity * Time.deltaTime;

                if (transform.position.y <= groundY)
                {
                    isFalling = true;
                    break;
                }

                yield return null;
            }

            bounceHeight *= damping;
        }

        if (this != null)
        {
            transform.position = new Vector3(transform.position.x, groundY, transform.position.z);
        }
    }
}
