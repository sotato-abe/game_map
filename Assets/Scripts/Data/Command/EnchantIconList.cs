using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enchant/EnchantIconList")]
public class EnchantIconList : ScriptableObject
{
    [System.Serializable]
    public class EnchantIconPair
    {
        public EnchantType type;
        public Sprite icon;
    }

    [SerializeField] private List<EnchantIconPair> enchantIcons;

    private Dictionary<EnchantType, Sprite> enchantIconDictionary;

    private void OnEnable()
    {
        InitDictionary();
    }

    private void InitDictionary()
    {
        enchantIconDictionary = new Dictionary<EnchantType, Sprite>();

        foreach (var pair in enchantIcons)
        {
            if (!enchantIconDictionary.ContainsKey(pair.type))
            {
                enchantIconDictionary.Add(pair.type, pair.icon);
            }
        }
    }

    public Sprite GetIcon(EnchantType type)
    {
        if (enchantIconDictionary == null || enchantIconDictionary.Count == 0)
        {
            InitDictionary();
        }

        return enchantIconDictionary.TryGetValue(type, out var icon) ? icon : null;
    }
}
