using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TreasureDialog : VariableDialog
{
    [SerializeField] GameObject levelPrefab;
    [SerializeField] GameObject levelList;
    protected override float PaddingHeight => 60f;
    public void Setup(Item item)
    {
        if (item is Treasure treasure)
        {
            namePlate.SetName(item.Base.Name);
            description.text = item.Base.Description;
            SetRarity(item);
            ResizeDialog();
        }
    }

        private void SetRarity(Item item)
    {
        // levelList内のオブジェクトを削除
        foreach (Transform child in levelList.transform)
        {
            Destroy(child.gameObject);
        }

        // レベルに応じてレベルリストを設定
        for (int i = 0; i <= (int)item.Base.Rarity; i++)
        {
            GameObject levelObject = Instantiate(levelPrefab, levelList.transform);
            levelObject.SetActive(true);
        }
    }
}
