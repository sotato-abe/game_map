using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class FieldInfoSystem : MonoBehaviour
{
    [SerializeField] WorldMapDialog worldMapDialog;
    [SerializeField] FieldInfoPanel fieldInfoPanel;
    [SerializeField] List<Battler> enemies;
    [SerializeField] MapBase mapBase;

    void Start()
    {
        Debug.Log("FieldInfoSystem start");
        transform.gameObject.SetActive(true);
        SetupFieldInfo();
    }

    public void SetupFieldInfo()
    {
        FieldDialogOpen();
        fieldInfoPanel.SetInfo(mapBase);
    }

    public void FieldDialogOpen()
    {
        // worldMapDialog.gameObject.SetActive(true);
        fieldInfoPanel.gameObject.SetActive(true);
    }

    public void FieldDialogClose()
    {
        // worldMapDialog.gameObject.SetActive(false);
        fieldInfoPanel.gameObject.SetActive(false);
    }

    public Battler GetRandomEnemy()
    {
        int r = Random.Range(0, enemies.Count);
        return enemies[r];
    }
}
