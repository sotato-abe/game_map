using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessageDialog : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] TextMeshProUGUI text;
    [SerializeField] float letterPerSecond;

    public IEnumerator TypeDialog(string line)
    {
        Debug.Log("TypeDialog!!");
        text.SetText("");
        foreach (char letter in line)
        {
            text.text += letter;
            yield return new WaitForSeconds(letterPerSecond);
        }
    }
}
