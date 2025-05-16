using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnchantIcon : Unit
{
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI val;
    [SerializeField] EnchantDialog dialog;

    private float magnificationPower = 1.1f; // ターゲットスケール

    public void SetEnchant(Enchant enchant)
    {
        val.text = enchant.Val.ToString();
        SetEnchantIcon(enchant.Type);
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

    private void SetEnchantIcon(EnchantType type)
    {
        EnchantData data = EnchantDatabase.Instance?.GetData(type);
        if (data != null)
        {
            image.sprite = data.icon;
        }
        else
        {
            Debug.LogWarning($"EnchantIcon: No data found for type {type}");
        }
    }

    public IEnumerator ChangeScale(bool focusFlg)
    {
        float time = 0.05f;
        float currentTime = 0f;
        if (focusFlg)
        {
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
        }
    }
}

