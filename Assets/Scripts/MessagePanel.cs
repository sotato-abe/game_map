using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessagePanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] float letterPerSecond;

    private void SetMessageDialog()
    {
        Debug.Log("SetMessageDialog");
    }

    public IEnumerator TypeDialog(string line)
    {
        text.SetText("");
        Debug.Log($"Line:{line}");
        foreach (char letter in line)
        {
            text.text += letter;
            yield return new WaitForSeconds(letterPerSecond);
        }
        yield return new WaitForSeconds(2f);
    }
}
