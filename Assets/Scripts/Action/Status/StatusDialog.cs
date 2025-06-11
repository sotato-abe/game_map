using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatusDialog : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI characterName;
    [SerializeField] public TextMeshProUGUI description;
    [SerializeField] RectTransform backRectTransform;
    [SerializeField] GameObject rarityPrefab;
    [SerializeField] GameObject rarityList;

    private float PaddingHeight = 130f;
    // private float PaddingWidth = 90f;
    private float dialogWidth = 600f;

    public void Setup(Battler battler)
    {
        characterName.SetText(battler.Base.Name);
        description.text = battler.Base.Description;
        SetRarity((int)battler.Base.Rarity);
        ResizeDialog();
    }
    private void SetRarity(int rarity = 1)
    {
        // rarityList内を初期化
        foreach (Transform child in rarityList.transform)
        {
            Destroy(child.gameObject);
        }
        // rarityList内にレアリティを追加
        for (int i = 0; i <= rarity; i++)
        {
            GameObject rarityObject = Instantiate(rarityPrefab, rarityList.transform);
            rarityObject.SetActive(true);
        }
    }

    public void ResizeDialog()
    {
        description.ForceMeshUpdate();
        float newHeight = description.preferredHeight + PaddingHeight;
        backRectTransform.sizeDelta = new Vector2(dialogWidth, newHeight);
    }
}
