using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status/StatusIconList")]
public class StatusIconList : ScriptableObject
{
    [System.Serializable]
    public class StatusIconPair
    {
        public StatusType type;
        public Sprite icon;
    }

    [SerializeField] private List<StatusIconPair> enchantIcons;

    private Dictionary<StatusType, Sprite> statusIconDictionary;

    private void OnEnable()
    {
        InitDictionary();
    }

    private void InitDictionary()
    {
        statusIconDictionary = new Dictionary<StatusType, Sprite>();

        foreach (var pair in enchantIcons)
        {
            if (!statusIconDictionary.ContainsKey(pair.type))
            {
                statusIconDictionary.Add(pair.type, pair.icon);
            }
        }
    }

    public Sprite GetIcon(StatusType type)
    {
        if (statusIconDictionary == null || statusIconDictionary.Count == 0)
        {
            InitDictionary();
        }

        return statusIconDictionary.TryGetValue(type, out var icon) ? icon : null;
    }
}
