using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Equipment/EquipmentTypeList")]
public class EquipmentTypeList : ScriptableObject
{
    [System.Serializable]
    public class EquipmentTypePair
    {
        public EquipmentType type;
        public Sprite icon;
    }

    [SerializeField] private List<EquipmentTypePair> equipmentTypes;

    private Dictionary<EquipmentType, Sprite> equipmentPartDictionary;

    private void OnEnable()
    {
        InitDictionary();
    }

    private void InitDictionary()
    {
        equipmentPartDictionary = new Dictionary<EquipmentType, Sprite>();

        foreach (var pair in equipmentTypes)
        {
            if (!equipmentPartDictionary.ContainsKey(pair.type))
            {
                equipmentPartDictionary.Add(pair.type, pair.icon);
            }
        }
    }

    public Sprite GetIcon(EquipmentType type)
    {
        if (equipmentPartDictionary == null || equipmentPartDictionary.Count == 0)
        {
            InitDictionary();
        }

        return equipmentPartDictionary.TryGetValue(type, out var icon) ? icon : null;
    }
}
