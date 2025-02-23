using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DescriptionPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI commandName;
    [SerializeField] private TextMeshProUGUI description;

    void Start()
    {
        transform.gameObject.SetActive(false);
    }

    public virtual void Setup(Command command)
    {
        commandName.text = command.Base.Name;
        description.text = command.Base.Description;
    }

    public void ShowDescriptionPanel(bool showFlg)
    {
        transform.gameObject.SetActive(showFlg);
    }
}
