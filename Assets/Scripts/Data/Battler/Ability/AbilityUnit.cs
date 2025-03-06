using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityUnit : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI name;  // 表示用のTextMeshProUGUIフィールド
    [SerializeField] private TextMeshProUGUI description;  // 表示用のTextMeshProUGUIフィールド
    private bool isActive = false;

    public virtual void Setup(Ability ability)
    {
        name.text = ability.Name;
        description.text = ability.Description;
    }

    public void OnPointerEnter()
    {
        StartCoroutine(OnPointer(true));

    }

    public void OnPointerExit()
    {
        StartCoroutine(OnPointer(false));
    }

    public IEnumerator OnPointer(bool focusFlg)
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

    public void SetTarget(bool activeFlg)
    {
        if (isActive == activeFlg) return;
        isActive = activeFlg;
    }
}
