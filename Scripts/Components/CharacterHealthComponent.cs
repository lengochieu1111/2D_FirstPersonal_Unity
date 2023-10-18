using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealthComponent : CharacterAbstract
{
    [Header("Character Health")]
    [SerializeField] private float _fHealth;
    [SerializeField] private float _fMaxHealth;
    public float FHealth => _fHealth;
    public float FMaxHealth => _fMaxHealth;

    protected override void SetupValues()
    {
        base.SetupValues();

        this._fHealth = this.baseCharacter.CharacterSO.FHealth;
        this._fMaxHealth = this.baseCharacter.CharacterSO.FMaxHealth;
    }

    public void UpdateHealthByDamage(float fDamage)
    {
        this._fHealth = Mathf.Clamp(this._fHealth - fDamage, 0.0f, this._fMaxHealth);

        Debug.Log("TakePoinDamage");
    }

    public float GetHealthPercen()
    {
        return this._fHealth / this._fMaxHealth;
    }

}
