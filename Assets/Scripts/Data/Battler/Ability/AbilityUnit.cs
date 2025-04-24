using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityUnit : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI name;  // 表示用のTextMeshProUGUIフィールド
    [SerializeField] private TextMeshProUGUI description;  // 表示用のTextMeshProUGUIフィールド
    // private Canvas abilityCanvas;  // 表示用のTextMeshProUGUIフィールド
    private bool isActive = false;

    private float magnificationPower = 2.0f; // ターゲットスケール
    // void Awake()
    // {
    //     if (abilityCanvas == null)
    //         abilityCanvas = GetComponent<Canvas>(); // 自分にCanvasがある場合、自動取得
    // }
    public virtual void Setup(Ability ability)
    {
        name.text = ability.Name;
        description.text = ability.Description;
    }

    public void OnPointerEnter()
    {
        Debug.Log("OnPointerEnter");
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
            // abilityCanvas.overrideSorting = true;
            // abilityCanvas.sortingOrder = 10;

            Vector3 originalScale = transform.localScale;
            Vector3 targetScale = new Vector3(magnificationPower, magnificationPower, magnificationPower);
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
            // abilityCanvas.sortingOrder = 0;
        }
    }

    public void SetTarget(bool activeFlg)
    {
        if (isActive == activeFlg) return;
        isActive = activeFlg;
    }
}
