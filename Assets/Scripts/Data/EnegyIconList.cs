using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enegy/EnegyIconList")]
public class EnegyIconList : ScriptableObject
{
    [System.Serializable]
    public class EnegyIconPair
    {
        public EnegyType type;
        public Sprite icon;
    }

    [SerializeField] private List<EnegyIconPair> enchantIcons;

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
