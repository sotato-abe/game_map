using System.Collections.Generic;
using UnityEngine;

public class TurnBattlerIconPool : MonoBehaviour
{
    [SerializeField] private TurnBattlerIcon iconPrefab;
    private List<TurnBattlerIcon> pool = new List<TurnBattlerIcon>();

    public static TurnBattlerIconPool Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public TurnBattlerIcon GetIcon()
    {
        if (pool.Count > 0) // ここを修正
        {
            var icon = pool[0]; // 先頭のアイコンを取得
            pool.RemoveAt(0); // リストから削除
            icon.gameObject.SetActive(true);
            return icon;
        }

        // プールが空なら新しく生成する
        TurnBattlerIcon newIcon = Instantiate(iconPrefab);
        newIcon.gameObject.SetActive(true);
        return newIcon;
    }

    public void ReleaseIcon(TurnBattlerIcon icon)
    {
        icon.gameObject.SetActive(false);
        pool.Add(icon); // プールに戻す
    }
}
