using System.Collections;
using UnityEngine;

public class Dialog : MonoBehaviour
{
    float time = 0.05f;
    float currentTime = 0f;
    Vector3 originalScale = new Vector3(1f, 1f, 1f);
    Vector3 targetScale = new Vector3(1.1f, 1.1f, 1.1f);

    private Coroutine dialogCoroutine;

    private void Start()
    {
        transform.gameObject.SetActive(false);
    }

    public void ShowDialog(bool showFlg)
    {
        if (showFlg)
        {
            transform.gameObject.SetActive(true);
            dialogCoroutine = StartCoroutine(OpenDialog());
        }
        else
        {
            if (dialogCoroutine == null)
            {
                dialogCoroutine = StartCoroutine(CloseDialog());
            }
            transform.gameObject.SetActive(false);
        }
    }

    private IEnumerator OpenDialog()
    {
        currentTime = 0f; // 毎回リセット
        while (currentTime < time)
        {
            transform.localScale = Vector3.Lerp(originalScale, targetScale, currentTime / time);
            currentTime += Time.deltaTime;
            yield return null;
        }
        transform.localScale = targetScale;
        dialogCoroutine = null;
    }

    private IEnumerator CloseDialog()
    {
        currentTime = 0f; // 毎回リセット
        while (currentTime < time)
        {
            transform.localScale = Vector3.Lerp(targetScale, originalScale, currentTime / time);
            currentTime += Time.deltaTime;
            yield return null;
        }
        transform.localScale = originalScale;
        dialogCoroutine = null;
    }
}
