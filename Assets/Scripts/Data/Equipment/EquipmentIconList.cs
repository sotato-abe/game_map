using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enegy/EquipmentIconList")]
public class EquipmentIconList : ScriptableObject
{
    [System.Serializable]
    public class EquipmentIconPair
    {
        public EnegyType type;
        public Sprite icon;
    }

    [SerializeField] private List<EquipmentIconPair> enchantIcons;

    private Dictionary<EnegyType, Sprite> enchantIconDictionary;

    private void OnEnable()
    {
        InitDictionary();
    }

    private void InitDictionary()
    {
        enchantIconDictionary = new Dictionary<EnegyType, Sprite>();

        foreach (var pair in enchantIcons)
        {
            if (!enchantIconDictionary.ContainsKey(pair.type))
            {
                enchantIconDictionary.Add(pair.type, pair.icon);
            }
        }
    }

    public Sprite GetIcon(EnegyType type)
    {
        if (enchantIconDictionary == null || enchantIconDictionary.Count == 0)
        {
            InitDictionary();
        }

        return enchantIconDictionary.TryGetValue(type, out var icon) ? icon : null;
    }
}
