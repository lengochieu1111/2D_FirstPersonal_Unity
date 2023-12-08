using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealthComponent : CharacterAbstract
{
    [Header("Character Health")]
    [SerializeField] private float _fHealth;
    [SerializeField] private float _fMaxHealth;
    [SerializeField] private bool _bIsPaining;
    [SerializeField] private bool _bIsDead;
    public float Health => _fHealth;
    public float MaxHealth => _fMaxHealth;
    public bool bIsPaining => _bIsPaining;
    public bool bIsDead => _bIsDead;

    protected override void SetupValues()
    {
        base.SetupValues();

        this._bIsPaining = false;
        this._bIsDead = false;
        this._fHealth = this.baseCharacter.CharacterSO.Health;
        this._fMaxHealth = this.baseCharacter.CharacterSO.MaxHealth;
    }

    public void UpdateHealthByDamage(float fDamage)
    {
        this._fHealth = Mathf.Clamp(this._fHealth - fDamage, 0.0f, this._fMaxHealth);

        if (this._fHealth <= 0.0f)
        {
            this._bIsDead = true;
            this.baseCharacter?.HandleDead();
        }
        else
        {
            this._bIsPaining = true;
            this.baseCharacter?.HandlePain();
        }
    }

    public void AN_PainEnd()
    {
        this._bIsPaining = false;
    }

}
