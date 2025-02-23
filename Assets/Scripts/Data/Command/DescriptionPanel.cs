using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DescriptionPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    void Start()
    {
        transform.gameObject.SetActive(false);
    }

    public virtual void Setup(Command command)
    {
        string description = command.Base.Description;
        text.text = description;
    }

    public void ShowDescriptionPanel(bool showFlg)
    {
        transform.gameObject.SetActive(showFlg);
    }
}
