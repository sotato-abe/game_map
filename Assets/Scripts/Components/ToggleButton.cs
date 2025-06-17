using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToggleButton : MonoBehaviour
{
    [SerializeField] protected GameObject button1;
    [SerializeField] protected GameObject button2;
    [SerializeField] protected Image activePanel;
    protected bool isActive = true;

    protected Color32 inactiveColor = new Color32(255, 255, 255, 255);
    protected Color32 activeColor = new Color32(0, 0, 0, 255);

    public virtual void SetActive()
    {
        if (!isActive)
        {
            isActive = true;
            ApplyVisualState(true);
            OnActivated();
        }
    }

    public virtual void SetInActive()
    {
        if (isActive)
        {
            isActive = false;
            ApplyVisualState(false);
            OnDeactivated();
        }
    }

    protected void ApplyVisualState(bool active)
    {
        button1.GetComponentInChildren<TextMeshProUGUI>().color = active ? activeColor : inactiveColor;
        button2.GetComponentInChildren<TextMeshProUGUI>().color = active ? inactiveColor : activeColor;
        Vector3 targetPos = active ? button1.transform.localPosition : button2.transform.localPosition;
        StartCoroutine(SlideActivePanel(targetPos));
    }

    protected virtual void OnActivated() { }
    protected virtual void OnDeactivated() { }

    private IEnumerator SlideActivePanel(Vector3 targetPosition)
    {
        Vector3 startPosition = activePanel.transform.localPosition;
        float elapsedTime = 0f;
        float duration = 0.2f;

        while (elapsedTime < duration)
        {
            activePanel.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        activePanel.transform.localPosition = targetPosition;
    }
}
