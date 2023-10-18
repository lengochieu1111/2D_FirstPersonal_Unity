using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterWeaponComponent : CharacterAbstract
{
    [Header("Character Weapon")]
    [SerializeField] private CapsuleCollider2D _capsuleCollider;
    public CapsuleCollider2D CapsuleCollider => _capsuleCollider;

    protected override void LoadComponents()
    {
        base.LoadComponents();

        if (this._capsuleCollider == null)
            this._capsuleCollider = GetComponent<CapsuleCollider2D>();

    }


}
