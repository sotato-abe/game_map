using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemDescriptionPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI description;

    void Start()
    {
        transform.gameObject.SetActive(false);
    }

    public virtual void Setup(Item item)
    {
        itemName.text = item.Base.Name;
        description.text = item.Base.Description;
    }

    public void ShowDescriptionPanel(bool showFlg)
    {
        Debug.Log($"ItemDescriptionPanel ShowDescriptionPanel:{showFlg}");   
        transform.gameObject.SetActive(showFlg);
    }
}
