using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUnit : MonoBehaviour
{

    [SerializeField] BattlerBase _base;
    public Battler Battler { get; set; }
    public BattlerBase Base { get => _base; }

    public virtual void Setup(Battler battler)
    {
        Battler = battler;
    }
}
