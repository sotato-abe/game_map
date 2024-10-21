using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class FieldInfoSystem : MonoBehaviour
{
    [SerializeField] WorldMapDialog worldMapDialog;
    [SerializeField] FieldInfoDialog fieldInfoDialog;

    void Start()
    {
        transform.gameObject.SetActive(true);
    }

    public void SetupFieldInfo()
    {
        Debug.Log("SetupFieldInfo!!");
        FieldDialogOpen();
    }

    public void FieldDialogOpen()
    {
        worldMapDialog.gameObject.SetActive(true);
        fieldInfoDialog.gameObject.SetActive(true);
    }

    public void FieldDialogClose()
    {
        worldMapDialog.gameObject.SetActive(false);
        fieldInfoDialog.gameObject.SetActive(false);
    }
}
