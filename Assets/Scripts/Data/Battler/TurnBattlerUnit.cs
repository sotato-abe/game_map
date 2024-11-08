using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnBattlerUnit : MonoBehaviour
{
    // キャラクターの画像
    [SerializeField] Image image;

    // コンストラクタ、または初期設定メソッドで必要な情報を設定
    public void Initialize(Battler battler)
    {
        if (battler.Base != null)
        {
            image.sprite = battler.Base.Sprite;
        }
        else
        {
            Debug.LogError("Battler.Base is null. Cannot assign sprite.");
        }
    }

    private void Start()
    {
        // 必要に応じて初期化や設定の確認
        Debug.Log($"Initialized TurnBattlerUnit");
    }
}
