using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandSlot : MonoBehaviour
{
    public Command command { get; set; }
    [SerializeField] Image image;
    [SerializeField] Image maskImage;
    [SerializeField] CommandDialog commandDialog;

    public void Start()
    {
    }

    public void Setup(Command command)
    {
        this.command = command;
        image.sprite = command.Base.Sprite;
        commandDialog.Setup(command);
    }

    public void OnPointerEnter()
    {
        if (command != null)
        {
            commandDialog.ShowDialog(true);
            StartCoroutine(Targetfoucs(true));
        }
    }

    public void OnPointerExit()
    {
        if (command != null)
        {
            commandDialog.ShowDialog(false);
            StartCoroutine(Targetfoucs(false));
        }
    }

    public void RemoveCommand()
    {
        this.command = null;
        maskImage.color = new Color(maskImage.color.r, maskImage.color.g, maskImage.color.b, 0f);
        image.sprite = null;
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
