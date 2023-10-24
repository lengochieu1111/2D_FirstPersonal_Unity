using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacter : RyoMonoBehaviour, IAttackInterface
{
    public event Action<BaseCharacter, float> TakePointDamage_Action;

        #region Property

    [Header("Components")]
    [SerializeField] protected Rigidbody2D _rigidbody;
    [SerializeField] protected CharacterSO _characterSO;
    [SerializeField] protected CharacterController _characterController;
    [SerializeField] protected CharacterCapsuleComponent _characterCapsule;
    [SerializeField] protected CharacterMeshComponent _characterMesh;
    [SerializeField] protected CharacterWeaponComponent _characterWeapon;
    [SerializeField] protected CharacterMovementComponent _characterMovement;
    [SerializeField] protected CharacterAttackComponent _characterAttack;
    [SerializeField] protected CharacterHealthComponent _characterHealth;

    [SerializeField] protected IAttackInterface attackInterface_Target;

    [Header("State")]
    [SerializeField] public ECharacterState CharacterState;

    public Rigidbody2D Rigidbody => _rigidbody;
    public CharacterSO CharacterSO => _characterSO;
    public CharacterController CharacterController => _characterController;
    public CharacterCapsuleComponent CharacterCapsule => _characterCapsule;
    public CharacterMeshComponent CharacterMesh => _characterMesh;
    public CharacterWeaponComponent CharacterWeapon => _characterWeapon;
    public CharacterMovementComponent CharacterMovement => _characterMovement;
    public CharacterAttackComponent CharacterAttack => _characterAttack;
    public CharacterHealthComponent CharacterHealth => _characterHealth;

        #endregion

        #region Component

    protected override void OnEnable()
    {
        base.OnEnable();

        if (this._characterAttack != null)
            this._characterAttack.HitSomeThing_Action += HandleHitSomeThing;

        TakePointDamage_Action += HandleTakePoinDamage;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        if (this._characterAttack != null)
            this._characterAttack.HitSomeThing_Action -= HandleHitSomeThing;

        TakePointDamage_Action -= HandleTakePoinDamage;

    }

    protected override void LoadComponents()
    {
        base.LoadComponents();

        if (this._rigidbody == null)
            this._rigidbody = GetComponent<Rigidbody2D>();
        
        if (this._characterCapsule == null)
            this._characterCapsule = GetComponentInChildren<CharacterCapsuleComponent>();

        if (this._characterController == null )
            this._characterController = GetComponent<CharacterController>();

        if (this._characterMesh == null )
            this._characterMesh = GetComponentInChildren<CharacterMeshComponent>();
        
        if (this._characterWeapon == null )
            this._characterWeapon = GetComponentInChildren<CharacterWeaponComponent>();
        
        if (this._characterMovement == null )
            this._characterMovement = GetComponentInChildren<CharacterMovementComponent>();
        
        if (this._characterAttack == null )
            this._characterAttack = GetComponentInChildren<CharacterAttackComponent>();
        
        if (this._characterHealth == null )
            this._characterHealth = GetComponentInChildren<CharacterHealthComponent>();

        if (this._characterSO == null)
        {
            string resPath = this.transform.name;
            this._characterSO = Resources.Load<CharacterSO>(resPath);
        }
    }

        #endregion

        #region Handle Movement
    public void HandlePressedMove(Vector2 moveVector)
    {
        this._characterMovement.RequestMove(moveVector);

        if (moveVector.x != 0 && this._characterMesh.BIsAttacking == false)
            this.HandleRequestFlipMesh(moveVector);
        else
        {
            if (this._characterMesh.BIsJumping == true 
                || this._characterMesh.BIsAttacking == true
                || this._characterCapsule.BIsFalling == true) return;
            this._characterMesh.RequestUpdateState(this._characterSO.Anim_Idle);
        }

    }

    public void HandleRequestFlipMesh(Vector2 moveVector)
    {
        float fAngle = 0;
        if (moveVector.x > 0)
            fAngle = 0;
        else if (moveVector.x < 0)
            fAngle = -180;

        this.CharacterMesh.RequestFlip(fAngle);
    }

        #endregion

        #region Handle Jump

    public void HandlePressedJump()
    {
        if (this._characterMesh.BIsJumping == true 
            || this._characterCapsule.BIsFalling == true 
            || this._characterMesh.BIsAttacking == true) return;

        this._characterMesh.BIsJumping = true;
        this._characterMesh.RequestUpdateState(this._characterSO.Anim_Jump);
        this._characterMovement.RequestJump();
        this._characterMesh.Animator.SetBool("IsFalling", true);
    }

    public void HandleFalling()
    {
        if (this._characterCapsule.BIsFalling)
            this._characterMesh.Animator.SetBool("IsFalling", true);
        else
            this._characterMesh.Animator.SetBool("IsFalling", false);
    }

    public void HandleJumpEndToRun()
    {
        if (this._characterMovement.BPressingMove == true)
        {
            this._characterMesh.BIsJumping = false;
            this._characterMesh.RequestUpdateState(this._characterSO.Anim_Run);
        }
    }
    
    public void HandleJumpEndToIdle()
    {
        if (this._characterMovement.BPressingMove == false)
        {
            this._characterMesh.BIsJumping = false;
            this._characterMesh.RequestUpdateState(this._characterSO.Anim_Idle);
        }
    }

    #endregion

        #region Handle Attack

    public void HandlePressedAttack(EAttackType requestAttackType)
    {
        this._characterAttack.RequestAttackType = requestAttackType;

        if (this._characterMesh.BIsAttacking == true)
        {
            this._characterAttack.BSavedAttack = true;
            return;
        }

        if ( this._characterCapsule.BIsFalling == true
            || this._characterMesh.BIsJumping == true) return;

        this._characterMesh.BIsAttacking = true;
        this._characterAttack.RequestAttack();
        SoundSpawner.Instance.PlayAudio(this._characterSO.WeaponTrailAudio);
    }

        #endregion

        #region Attack Interface

    public void I_PlayAttackAnim(int iStateAttack)
    {
        this._characterMesh.RequestUpdateState(iStateAttack);
    }

    public virtual void I_AttackEnd()
    {
        this._characterMesh.BIsAttacking = false;
        this._characterAttack.IAttackIndex = 0;

        if (this._characterController.MoveInput.x != 0)
        {
            this._characterMesh.RequestUpdateState(this._characterSO.Anim_Run);
            this._characterMovement.RequestMove(this._characterController.MoveInput, false);
            this.HandleRequestFlipMesh(this._characterController.MoveInput);
        }
        else
            this._characterMesh.RequestUpdateState(this._characterSO.Anim_Idle);
    }

    public virtual void I_AtackCombo()
    {
        if (this._characterAttack.BSavedAttack == true)
        {
            this._characterAttack.NextAttack();
            this._characterAttack.RequestAttack();
            SoundSpawner.Instance.PlayAudio(this._characterSO.WeaponTrailAudio);
            this._characterAttack.BSavedAttack = false;
        }

    }

    public void I_TraceStart()
    {
        this._characterAttack?.TraceStart();
    }

    public void I_TraceEnd()
    {
        this._characterAttack?.TraceEnd();
    }

    public void I_AE_TraceHit()
    {
        this._characterAttack?.TraceHit();
    }

    #endregion

        #region Handle HitSomeThing - TakePointDamage

    public void HandleHitSomeThing(BaseCharacter characterReceives)
    {
        SoundSpawner.Instance.PlayAudio(this._characterSO.WeaponHitAudio);
        characterReceives?.TakePointDamage_Action?.Invoke(this, this._characterSO.FDamage);
    }

    protected virtual void HandleTakePoinDamage(BaseCharacter characterCauses, float fDamage)
    {
        if (this._characterMesh.BIsDead == true) return;

        this._characterHealth?.UpdateHealthByDamage(fDamage);

        if (this._characterHealth?.FHealth <= 0.0f)
            this.HandleDeadStart();
        else
            this.HandlePainStart();

    }

    #endregion

        #region Haandle Pain - Dealth

    public void HandleDeadStart()
    {
        this._characterMesh.BIsDead = true;
        this._characterMesh.RequestUpdateState(this._characterSO.Anim_Death);
        SoundSpawner.Instance.PlayAudio(this._characterSO.DeathAudio);
    }

    public virtual void HandleDeadEnd()
    {
        this.attackInterface_Target?.I_HandleTargetDestroyed();
        this._characterMesh.BIsDead = false;
        this.gameObject.SetActive(false);
    }

    public void HandlePainStart()
    {
        this._characterMesh.BIsPaining = true;
        SoundSpawner.Instance.PlayAudio(this._characterSO.PainAudio);
        Transform painEffect = EffectSpawner.Instance.SpawnObject(EffectSpawner.BloodEffect, this.transform.position, this.transform.rotation);
        painEffect.SetParent(this.transform);
        painEffect.gameObject.SetActive(true);
        this._characterMesh.RequestUpdateState(this._characterSO.Anim_Pain);

    }

    public void HandlePainEnd()
    {
        this._characterMesh.BIsPaining = false;
        this._characterMesh.BIsAttacking = false;

        if (this._characterMovement.BPressingMove == true)
            this._characterMesh.RequestUpdateState(this._characterSO.Anim_Run);
        else
            this._characterMesh.RequestUpdateState(this._characterSO.Anim_Idle);

    }

    public virtual void I_EnterCombat(BaseCharacter character)
    {
        this.attackInterface_Target = character.GetComponent<IAttackInterface>();

        this._characterMovement.ChangeMovementSpeed(this._characterSO.FCombatSpeed);
    }

    public virtual void I_ExitCombat()
    {
        this._characterMovement.ChangeMovementSpeed(this._characterSO.FDefaultSpeed);
    }

    public virtual void I_HitTarget(float Health_Target, float MaxHealth_Target) {}

    public virtual void I_HandleTargetDestroyed() 
    {
        this.I_ExitCombat();
    }


    #endregion


    /*    
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Bounds CapsuleBounds = this._capsuleCollider.bounds;
            Gizmos.DrawWireCube(CapsuleBounds.center, CapsuleBounds.size);
        }
    */



}
