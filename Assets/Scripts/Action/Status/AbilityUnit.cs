using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityUnit : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI abilityName;  // 表示用のTextMeshProUGUIフィールド
    [SerializeField] Image dialogImage;
    [SerializeField] AbilityDialog dialog;
    private float scale = 1.1f; // ターゲットスケール

    public virtual void Setup(Ability ability)
    {
        abilityName.text = ability.Name;
        dialogImage.sprite = ability.Sprite;
        dialog.gameObject.SetActive(true);
        dialog.Setup(ability);
    }

    public void OnPointerEnter()
    {
        dialog.ShowDialog(true);
        StartCoroutine(ChangeScale(true));
    }

    public void OnPointerExit()
    {
        dialog.ShowDialog(false);
        StartCoroutine(ChangeScale(false));
    }

    public IEnumerator ChangeScale(bool focusFlg)
    {
        float time = 0.05f;
        float currentTime = 0f;
        if (focusFlg)
        {
            Vector3 originalScale = transform.localScale;
            Vector3 targetScale = new Vector3(scale, scale, scale);
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
