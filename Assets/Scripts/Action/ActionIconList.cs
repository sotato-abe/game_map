using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Action/ActionTypeList")]
public class ActionIconList : ScriptableObject
{
    [System.Serializable]
    public class ActionTypePair
    {
        public ActionType type;
        public Sprite icon;
    }

    [SerializeField] private List<ActionTypePair> ActionTypes;

    private Dictionary<ActionType, Sprite> ActionPartDictionary;

    private void OnEnable()
    {
        InitDictionary();
    }

    private void InitDictionary()
    {
        ActionPartDictionary = new Dictionary<ActionType, Sprite>();

        foreach (var pair in ActionTypes)
        {
            if (!ActionPartDictionary.ContainsKey(pair.type))
            {
                ActionPartDictionary.Add(pair.type, pair.icon);
            }
        }
    }

    public Sprite GetIcon(ActionType type)
    {
        if (ActionPartDictionary == null || ActionPartDictionary.Count == 0)
        {
            InitDictionary();
        }

        return ActionPartDictionary.TryGetValue(type, out var icon) ? icon : null;
    }
}
