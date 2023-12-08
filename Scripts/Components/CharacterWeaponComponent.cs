using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterWeaponComponent : CharacterAbstract
{
    [Header("Character Weapon")]
    [SerializeField] private Collider2D _collider;
    public Collider2D Collider => _collider;

    protected override void LoadComponents()
    {
        base.LoadComponents();

        if (this._collider == null)
            this._collider = GetComponent<Collider2D>();

    }


}
