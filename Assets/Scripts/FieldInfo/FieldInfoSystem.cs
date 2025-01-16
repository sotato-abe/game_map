using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

// TODO : キャラクターが保有するクエストを受けとり、カードを表示する。
// TODO : Coordinate（座標）と座標を取得しアラートを該当するときカードを表示する。

public class FieldInfoSystem : MonoBehaviour
{
    [SerializeField] WorldMapDialog worldMapDialog;
    [SerializeField] FieldInfoPanel fieldInfoPanel;
    [SerializeField] List<Battler> enemies;
    [SerializeField] MapBase mapBase;

    void Start()
    {
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
