using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttackComponent : CharacterAbstract
{
    public event Action<BaseCharacter_Old> HitSomeThing_Action;

    [Header("Character Attack")]
    [SerializeField] private IAttackInterface _attackInterface;
    [SerializeField] protected CharacterSO_Old _characterSO;
    [SerializeField] private bool _bIsAttacking;
    [SerializeField] private bool _bCanCombo;
    [SerializeField] private bool _bSavedAttack;
    [SerializeField] private int _iAttackIndex;
    [SerializeField] private EAttackType _requestAttackType;
    private List<BaseCharacter_Old> _processedResults = new List<BaseCharacter_Old>();
    public bool bIsAttacking => _bIsAttacking;

    [Header("Trace Hit")]
    [SerializeField] private bool _bIsTracing;
    [SerializeField] private LayerMask _tracerLayer;
    [SerializeField] private List<BaseCharacter_Old> _tracerResults = new List<BaseCharacter_Old>();
    public LayerMask TracerLayer => _tracerLayer;

    protected override void SetupValues()
    {
        base.SetupValues();
        this._characterSO = this.baseCharacter.CharacterSO;
        this._attackInterface = GetComponentInParent<IAttackInterface>();
        this._bIsTracing = false;
    }

    public void RequestAttack(EAttackType attackType)
    {
        this._requestAttackType = attackType;
        bool bCanAttack = this._bCanCombo || !this._bIsAttacking;

        if (bCanAttack)
            this.Attack();
        else
            this._bSavedAttack = true;

    }

    public void Attack()
    {
        SoundSpawner.Instance.PlayAudio(this.baseCharacter.CharacterSO.WeaponTrailAudio);

        this._attackInterface?.I_PlayAttackAnim(this.GetCorrectAnimAttack());

        this._bIsAttacking = true;
        this._bCanCombo = false;
        this._iAttackIndex = (this._iAttackIndex + 1) % this._characterSO.Anim_NormalAttacks_Idle.Count;

    }

    private int GetCorrectAnimAttack()
    {
        if (this._requestAttackType == EAttackType.Strong)
            return this._characterSO.Anim_StrongAttacks;
        else
        {
            if (this.baseCharacter.CharacterController.bPressingMove)
                return this._characterSO.Anim_NormalAttack_Run;
            else
                return this._characterSO.Anim_NormalAttacks_Idle[this._iAttackIndex];
        }
    }

    public void AN_StartAttack()
    {
        this._tracerResults.Clear();
        this._processedResults.Clear();
    }

    public void AN_AttackEnd()
    {
        this._bIsAttacking = false;
        this._bCanCombo = false;
        this._bSavedAttack = false;
        this._iAttackIndex = 0;
    }

    public void AN_Combo()
    {
        this._bCanCombo = true;

        if (this._bSavedAttack)
        {
            this.RequestAttack(this._requestAttackType);
            this._bSavedAttack = false;
        }
    }

    public void AN_TraceHit()
    {
        if (this._bIsTracing == false) return;

        Bounds capsuleBounds = this.baseCharacter.CharacterWeapon.Collider.bounds;
        RaycastHit2D[] hits = Physics2D.CapsuleCastAll(capsuleBounds.center, capsuleBounds.size, CapsuleDirection2D.Horizontal, 0, Vector2.left, 0, this._tracerLayer);

        if (hits.Length < 1) return;

        foreach (var hit in hits)
        {
            BaseCharacter_Old character = hit.collider.GetComponentInParent<BaseCharacter_Old>();
            if (character == this.baseCharacter) continue;
            if (this._tracerResults.Contains(character)) continue;
            this._tracerResults.Add(character);
        }

        this.HandleHitResults();

    }

    public void HandleHitResults()
    {
        if (this._tracerResults.Count < 1) return;

        foreach (BaseCharacter_Old character in this._tracerResults)
        {
            //if (character.CharacterHealth.bIsDead) continue;
            if (this._processedResults.Contains(character)) continue;
            this.HitSomeThing_Action?.Invoke(character);
            this._processedResults.Add(character);
        }
    }

    public void AN_TraceStart()
    {
        this._bIsTracing = true;
    }

    public void AN_TraceEnd()
    {
        this._bIsTracing = false;
    }

}
