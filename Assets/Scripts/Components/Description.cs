using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Description : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI description;
    [SerializeField] Image image;

    private void Start()
    {
    }

    public IEnumerator TypeDescription(string line)
    {
        description.SetText("");
        foreach (char letter in line)
        {
            description.text += letter;
            yield return new WaitForSeconds(0f);
        }
    }
}
