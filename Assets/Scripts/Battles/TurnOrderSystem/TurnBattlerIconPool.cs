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
        foreach (var icon in pool)
        {
            if (!icon.gameObject.activeInHierarchy)
            {
                icon.gameObject.SetActive(true);
                return icon;
            }
        }

        // プールに空きがない場合、新しく生成
        TurnBattlerIcon newIcon = Instantiate(iconPrefab);
        pool.Add(newIcon);
        return newIcon;
    }

    public void ReleaseIcon(TurnBattlerIcon icon)
    {
        icon.gameObject.SetActive(false);
    }
}
