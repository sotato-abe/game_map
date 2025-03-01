using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Probability
{
    [SerializeField] private int value;

    public int Value
    {
        get => value;
        set => this.value = Mathf.Clamp(value, 0, 100); // 0～100の範囲に制限
    }

    public Probability(int value)
    {
        this.Value = value; // プロパティ経由でセット（範囲チェックされる）
    }

    // int への暗黙的な変換
    public static implicit operator int(Probability p) => p.value;

    // int からの暗黙的な変換
    public static implicit operator Probability(int value) => new Probability(value);

    public override string ToString() => value.ToString();
}
