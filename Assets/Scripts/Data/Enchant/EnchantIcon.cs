using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnchantIcon : Unit
{
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI val;
    // [SerializeField] EnchantDialog dialog;

    Color32 buffColor = new Color32(3, 137, 229, 255);
    Color32 debuffColor = new Color32(245, 52, 124, 255);
    Color32 bothColor = new Color32(0, 0, 0, 200);

    private float magnificationPower = 1.1f; // ターゲットスケール

    public void SetEnchant(Enchant enchant)
    {
        val.text = enchant.Val.ToString();
        SetEnchantIcon(enchant.Type);
        // dialog.Setup(enchant);
    }

    public void OnPointerEnter()
    {
        // dialog.ShowDialog(true);
        StartCoroutine(ChangeScale(true));
    }

    public void OnPointerExit()
    {
        // dialog.ShowDialog(false);
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

        BuffType buffType = BuffType.Both;
        if (EnchantDatabase.Instance != null)
        {
            buffType = EnchantDatabase.Instance.IsBuff(type);
        }
        SetColor(buffType);
    }

    public void SetColor(BuffType buffType)
    {
        switch (buffType)
        {
            case BuffType.Buff:
                image.color = buffColor;
                val.color = buffColor;
                break;
            case BuffType.Debuff:
                image.color = debuffColor;
                val.color = debuffColor;
                break;
            default:
                image.color = bothColor;
                val.color = bothColor;
                break;
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

