using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterAbstract : RyoMonoBehaviour
{
    [Header("Character Abstract")]
    [SerializeField] protected BaseCharacter baseCharacter;
    public BaseCharacter BaseCharacter => baseCharacter;

    protected override void LoadComponents()
    {
        base.LoadComponents();

        if (this.baseCharacter == null)
        this.baseCharacter = GetComponentInParent<BaseCharacter>();
    }
}
