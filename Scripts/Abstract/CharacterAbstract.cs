using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterAbstract : RyoMonoBehaviour
{
    [Header("Character Abstract")]
    [SerializeField] protected BaseCharacter_Old baseCharacter;
    public BaseCharacter_Old BaseCharacter => baseCharacter;

    protected override void LoadComponents()
    {
        base.LoadComponents();

        if (this.baseCharacter == null)
        this.baseCharacter = GetComponentInParent<BaseCharacter_Old>();
    }
}
