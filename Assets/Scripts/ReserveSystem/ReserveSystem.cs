using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ReserveSystem : MonoBehaviour
{
    public UnityAction OnReserveEnd;

    [SerializeField] ReserveActionBoard actionBoard;
    [SerializeField] MessagePanel messagePanel;
    [SerializeField] SlidePanel reservePanel;
    [SerializeField] SlidePanel leftUnitGroup;
    [SerializeField] FieldInfoPanel fieldInfoPanel;
    [SerializeField] TitlePanel titlePanel;

    void Start()
    {
        transform.gameObject.SetActive(false);
        actionBoard.OnReserveEnd += ResorveEnd;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.E))
        {
            ResorveEnd();
        }
    }

    public void ReserveStart()
    {
        transform.gameObject.SetActive(true);
        messagePanel.SetActive(false);
        fieldInfoPanel.SetActive(false);
        leftUnitGroup.SetActive(false);
        reservePanel.SetActive(true);
        titlePanel.SetTitle(TitleType.Reserve);

        actionBoard.gameObject.SetActive(true);
    }

    public void ResorveEnd()
    {
        actionBoard.gameObject.SetActive(false);
        int completed = 0;
        void CheckAllComplete()
        {
            completed++;
            if (completed >= 4)
            {
                OnReserveEnd?.Invoke();
                transform.gameObject.SetActive(false);
            }
        }

        titlePanel.SetActive(false, CheckAllComplete);
        reservePanel.SetActive(false, CheckAllComplete);
        messagePanel.SetActive(true, CheckAllComplete);
        fieldInfoPanel.SetActive(true, CheckAllComplete);
        leftUnitGroup.SetActive(true, CheckAllComplete);
    }
}