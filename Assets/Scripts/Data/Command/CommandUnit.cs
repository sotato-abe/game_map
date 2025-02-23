using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandUnit : MonoBehaviour
{
    public Command Command { get; set; }
    [SerializeField] Image image;
    [SerializeField] Image dialogBackground;

    public virtual void Setup(Command command)
    {
        Command = command;
        image.sprite = Command.Base.Sprite;
    }

    public void OnPointerEnter()
    {
        Debug.Log("OnPointerEnter");
        StartCoroutine(Targetfoucs(true));
    }

    public void OnPointerExit()
    {
        Debug.Log("OnPointerExit");
        StartCoroutine(Targetfoucs(false));
    }

    public IEnumerator Targetfoucs(bool focusFlg)
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
}
