using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FieldEnemy : FieldCharacter
{
    protected virtual void OnEnable()
    {
        StartCoroutine(JumpMotion());
    }
}
