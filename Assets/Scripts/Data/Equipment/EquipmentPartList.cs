using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Equipment/EquipmentPartList")]
public class EquipmentPartList : ScriptableObject
{
    [System.Serializable]
    public class EquipmentPartPair
    {
        public EquipmentPart type;
        public Sprite icon;
    }

    [SerializeField] private List<EquipmentPartPair> equipmentParts;

    private Dictionary<EquipmentPart, Sprite> equipmentPartDictionary;

    private void OnEnable()
    {
        InitDictionary();
    }

    private void InitDictionary()
    {
        equipmentPartDictionary = new Dictionary<EquipmentPart, Sprite>();

        foreach (var pair in equipmentParts)
        {
            if (!equipmentPartDictionary.ContainsKey(pair.type))
            {
                equipmentPartDictionary.Add(pair.type, pair.icon);
            }
        }
    }

    public Sprite GetIcon(EquipmentPart type)
    {
        if (equipmentPartDictionary == null || equipmentPartDictionary.Count == 0)
        {
            InitDictionary();
        }

        return equipmentPartDictionary.TryGetValue(type, out var icon) ? icon : null;
    }
}
