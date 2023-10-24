using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttackComponent : CharacterAbstract
{
    public event Action<BaseCharacter> HitSomeThing_Action;

    [Header("Character Attack")]
    [SerializeField] private List<int> Anim_Attacks;
    [SerializeField] private bool _bSavedAttack;
    [SerializeField] private int _iAttackIndex;
    [SerializeField] private EAttackType _requestAttackType;
    private List<BaseCharacter> _processedResults = new List<BaseCharacter>();

    [Header("Trace Hit")]
    [SerializeField] private bool _bIsTracing;
    [SerializeField] private LayerMask _tracerLayer;
    [SerializeField] private List<BaseCharacter> _tracerResults = new List<BaseCharacter>();

    public bool BIsTracing => _bIsTracing;
    public LayerMask TracerLayer => _tracerLayer;
    public EAttackType RequestAttackType { set => _requestAttackType = value; get => _requestAttackType; }
    public List<BaseCharacter> TracerResults => _tracerResults; 
    public int IAttackIndex { set => _iAttackIndex = value; get => _iAttackIndex; }
    public bool BSavedAttack { set => _bSavedAttack = value; get => _bSavedAttack; }

    protected override void SetupValues()
    {
        base.SetupValues();
        this._bIsTracing = false;
        this.Anim_Attacks = this.baseCharacter.CharacterSO.Anim_NormalAttacks;
    }

    public void RequestAttack()
    {
        this.Attack();
    }

    public void Attack()
    {
        this._processedResults.Clear();

        if (this._requestAttackType == EAttackType.Strong)
            this.baseCharacter.I_PlayAttackAnim(this.baseCharacter.CharacterSO.Anim_StrongAttacks);
        else
            this.baseCharacter.I_PlayAttackAnim(this.Anim_Attacks[this._iAttackIndex]);
    }

    public void NextAttack()
    {
        this._iAttackIndex = (this._iAttackIndex + 1) % this.Anim_Attacks.Count;
    }

    public void HandleHitResults()
    {
        if (this._tracerResults.Count < 1) return;

        foreach (BaseCharacter character in this._tracerResults)
        {
            if (this._processedResults.Contains(character) == true) continue;
            this.HitSomeThing_Action?.Invoke(character);
            this._processedResults.Add(character);
        }
    }

    public void TraceHit()
    {
        if (this._bIsTracing == false) return;
        Bounds capsuleBounds = this.baseCharacter.CharacterWeapon.CapsuleCollider.bounds;
        RaycastHit2D[] hits = Physics2D.CapsuleCastAll(capsuleBounds.center, capsuleBounds.size, CapsuleDirection2D.Horizontal, 0, Vector2.left, 0, this._tracerLayer);

        if (hits.Length < 1) return;

        foreach (var hit in hits)
        {
            BaseCharacter character = hit.collider.GetComponentInParent<BaseCharacter>();
            if (this._tracerResults.Contains(character) == true) continue;
            this._tracerResults.Add(character);
        }

        this.HandleHitResults();

    }

    public void TraceStart()
    {
        this._bIsTracing = true;
        this._tracerResults.Clear();
    }

    public void TraceEnd()
    {
        this._bIsTracing = false;
    }

}
