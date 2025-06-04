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

    void Start()
    {
        transform.gameObject.SetActive(false);
        actionBoard.OnReserveEnd += ResorveEnd;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.E))
        {
            actionBoard.gameObject.SetActive(false);
            OnReserveEnd?.Invoke();
        }
    }

    public void ReserveStart()
    {
        actionBoard.gameObject.SetActive(true);
    }

    public void ResorveEnd()
    {
        actionBoard.gameObject.SetActive(false);
        OnReserveEnd?.Invoke();
    }
}