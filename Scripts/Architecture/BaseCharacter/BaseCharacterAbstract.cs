using MVCS.Architecture.BaseCharacter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCharacterAbstract : RyoMonoBehaviour
{
    [SerializeField] protected BaseCharacter character;
    public BaseCharacter Character => character;

    protected override void LoadComponents()
    {
        base.LoadComponents();

        this.LoadCharacter();
    }

    protected virtual void LoadCharacter()
    {
        if (this.character != null) return;

        this.character = GetComponentInParent<BaseCharacter>();
    }
}
