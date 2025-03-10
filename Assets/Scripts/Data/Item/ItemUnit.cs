using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemUnit : MonoBehaviour, IDragHandler
{
    public Item Item { get; set; }
    [SerializeField] Image image;
    [SerializeField] Image dialogBackground;
    [SerializeField] ItemDialog itemDialog;
    private bool isActive = false;

    public virtual void Setup(Item item)
    {
        Item = item;
        image.sprite = Item.Base.Sprite;
        itemDialog.gameObject.SetActive(true);
        itemDialog.Setup(Item);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("test");
        transform.position = eventData.position;
    }

    public void OnPointerEnter()
    {
        itemDialog.ShowDialog(true);
        StartCoroutine(OnPointer(true));
    }

    public void OnPointerExit()
    {
        itemDialog.ShowDialog(false);
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

        // 背景の透明度を変更する。
        Color bgColor = dialogBackground.color;
        bgColor.a = isActive ? 1f : 0f;
        dialogBackground.color = bgColor;
    }
}
