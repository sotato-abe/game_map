using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatePanelController : MonoBehaviour
{
    [SerializeField] private Image statePanel;
    [SerializeField] private TextMeshProUGUI stateText;

    // カラーを変更するメソッド
    public void ChangeState(TimeState state)
    {
        if (state == TimeState.Stop)
        {
            statePanel.color = new Color(214f / 255f, 51f / 255f, 36f / 255f, 1.0f); // 修正箇所
            stateText.text = "STOP";
        }
        else if (state == TimeState.Live)
        {
            statePanel.color = new Color(3f / 255f, 169f / 255f, 244f / 255f, 1.0f); // 修正箇所
            stateText.text = "LIVE";
        }
        else
        {
            statePanel.color = new Color(129f / 255f, 67f / 255f, 214f / 255f, 1.0f); // 修正箇所
            stateText.text = "FAST";
        }
    }
}
