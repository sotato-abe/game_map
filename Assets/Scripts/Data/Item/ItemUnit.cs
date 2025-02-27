using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUnit : MonoBehaviour
{
    public Item Item { get; set; }
    [SerializeField] Image image;
    [SerializeField] Image dialogBackground;
    [SerializeField] ItemDescriptionPanel descriptionPanel;

    public virtual void Setup(Item item)
    {
        Item = item;
        image.sprite = Item.Base.Sprite;
        descriptionPanel.Setup(Item);
    }

    public virtual void Targetfoucs(bool focusFlg)
    {
        float alpha = focusFlg ? 1.0f : 0.0f;
        Color bgColor = dialogBackground.color;
        bgColor.a = Mathf.Clamp(alpha, 0f, 1f);
        dialogBackground.color = bgColor;
    }

    public void OnPointerEnter()
    {
        StartCoroutine(TargetfoucsMotion(true));
    }

    public void OnPointerExit()
    {
        StartCoroutine(TargetfoucsMotion(false));
    }

    public IEnumerator TargetfoucsMotion(bool focusFlg)
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
            descriptionPanel.ShowDescriptionPanel(true);
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
            descriptionPanel.ShowDescriptionPanel(false);
        }
    }
}
