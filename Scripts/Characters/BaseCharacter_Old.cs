using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacter_Old : RyoMonoBehaviour ,IAttackInterface
{
    public event Action<BaseCharacter_Old, float> TakePointDamage_Action;

        #region Property

    [Header("Components")]
    [SerializeField] protected Rigidbody2D _rigidbody;
    [SerializeField] protected CharacterSO_Old _characterSO;
    [SerializeField] protected CharacterController _characterController;
    [SerializeField] protected CharacterCapsuleComponent _characterCapsule;
    [SerializeField] protected CharacterMeshComponent _characterMesh;
    [SerializeField] protected CharacterWeaponComponent _characterWeapon;
    [SerializeField] protected CharacterMovementComponent _characterMovement;
    [SerializeField] protected CharacterAttackComponent _characterAttack;
    [SerializeField] protected CharacterHealthComponent _characterHealth;

    [SerializeField] protected IAttackInterface attackInterface_Target;

    public Rigidbody2D Rigidbody => _rigidbody;
    public CharacterSO_Old CharacterSO => _characterSO;
    public CharacterController CharacterController => _characterController;
    public CharacterCapsuleComponent CharacterCapsule => _characterCapsule;
    public CharacterMeshComponent CharacterMesh => _characterMesh;
    public CharacterWeaponComponent CharacterWeapon => _characterWeapon;
    public CharacterMovementComponent CharacterMovement => _characterMovement;
    public CharacterAttackComponent CharacterAttack => _characterAttack;
    public CharacterHealthComponent CharacterHealth => _characterHealth;

        #endregion

        #region Load_Component

    protected override void OnEnable()
    {
        base.OnEnable();

        if (this._characterAttack != null)
            this._characterAttack.HitSomeThing_Action += HandleHitSomeThing;

        TakePointDamage_Action += HandleTakePointDamage;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        if (this._characterAttack != null)
            this._characterAttack.HitSomeThing_Action -= HandleHitSomeThing;

        TakePointDamage_Action -= HandleTakePointDamage;

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
            this._characterSO = Resources.Load<CharacterSO_Old>(resPath);
        }
    }

        #endregion

        #region Handle Movement
    public void HandlePressMove(float moveValue, bool bAcceleration = true)
    {
        if (this._characterAttack.bIsAttacking) return;
        if (this._characterHealth.bIsPaining) return;
        if (this._characterHealth.bIsDead) return;

        this._characterMesh?.RequestFlipMesh(moveValue);

        if (this._characterMovement.bIsJumping) return;
        if (this._characterCapsule.bIsFalling) return;

        this._characterMovement?.RequestMove(moveValue, bAcceleration);
    }

    public void HandleReleaseMove()
    {
        if (this._characterAttack.bIsAttacking) return;
        if (this._characterMovement.bIsJumping) return;
        if (this._characterCapsule.bIsFalling) return;
        if (this._characterHealth.bIsPaining) return;
        if (this._characterHealth.bIsDead) return;

        this._characterMovement?.RequestIdle();
        this._characterMesh?.RequestIdleUpdate();

    }
    
    public void HandlePressSprint()
    {
        if (this._characterAttack.bIsAttacking) return;
        if (this._characterMovement.bIsJumping) return;
        if (this._characterCapsule.bIsFalling) return;

        this._characterMovement?.ChangeMaxMovementSpeed(this._characterSO.SprintSpeed);
    }

    public void HandleReleaseSprint()
    {
        if (this._characterAttack.bIsAttacking) return;
        if (this._characterMovement.bIsJumping) return;
        if (this._characterCapsule.bIsFalling) return;

        this._characterMovement?.ChangeMaxMovementSpeed(this._characterSO.DefaultSpeed);

    }
        #endregion

        #region Handle Jump
    public void HandlePressedJump()
    {
        if (this._characterMovement.bIsJumping) return;
        if (this._characterCapsule.bIsFalling) return;
        if (!this._characterMovement.bCanJump) return;
        if (this._characterAttack.bIsAttacking) return;

        //this._characterMesh?.RequestIdleUpdate();
        this._characterMesh?.RequestJumpUpdate();
        this._characterMovement?.RequestJump();
    }

    public void AN_HandleRising()
    {
        this._characterMesh?.AN_Rising();
    }

    public void AN_HandleFalling()
    {
        this._characterMesh?.AN_Falling();
    }

    public void AN_HandleFromGroundToFalling()
    {
        this._characterMesh?.AN_FromGroundToFalling();
    }

    public void AN_HandleJumpEnd()
    {
        this._characterMovement?.AN_JumpEnd();
    }

    public void AN_HandleJumpEndToRun()
    {
        if (this._characterMovement.bIsAccelerating) return;

        if (this._characterController.bPressingMove)
        {
            bool bCanAcceleration = (this._characterMesh.bFlipLeft && this._characterController.InputValueMove == 1)
                || (!this._characterMesh.bFlipLeft && this._characterController.InputValueMove == -1);

            if (bCanAcceleration)
                this.HandlePressMove(this._characterController.InputValueMove, true);
            else
                this.HandlePressMove(this._characterController.InputValueMove, false);
        }
    }

    public void AN_HandleJumpEndToIdle()
    {
        if (!this._characterController.bPressingMove)
            this.HandleReleaseMove();
    }

    #endregion

        #region Handle Attack
    public bool CanAttack()
    {
        return !this._characterCapsule.bIsFalling 
            && !this._characterMovement.bIsJumping
            && !this._characterHealth.bIsPaining;
    }

    public void HandlePressNormalAttack()
    {
        if (this.CanAttack())
            this._characterAttack?.RequestAttack(EAttackType.Normal);
    }
    
    public void HandlePressStrongAttack()
    {
        if (this.CanAttack())
            this._characterAttack?.RequestAttack(EAttackType.Strong);
    }

    protected virtual void HandleHitSomeThing(BaseCharacter_Old characterReceives)
    {
        SoundSpawner.Instance.PlayAudio(this._characterSO.WeaponHitAudio);
        characterReceives?.TakePointDamage_Action?.Invoke(this, this._characterSO.Damage);
    }

    protected virtual void HandleTakePointDamage(BaseCharacter_Old characterCauses, float fDamage)
    {
        if (this._characterHealth.bIsDead) return;
        this._characterHealth?.UpdateHealthByDamage(fDamage);
    }

        #endregion

    #region Attack Interface

    public void I_PlayAttackAnim(int iStateAttack)
    {
        this._characterMesh?.RequestAttackUpdate(iStateAttack);
    }

    public virtual void I_AN_StartAttack()
    {
        this._characterMovement?.ChangeMaxMovementSpeed(this._characterSO.CombatSpeed);
        this._characterMesh.bCanAttackToRun = false;
        this._characterAttack?.AN_StartAttack();
    }

    public virtual void I_AN_AttackEnd()
    {
        this._characterAttack?.AN_AttackEnd();
        this._characterMovement?.ChangeMaxMovementSpeed(this._characterSO.DefaultSpeed);

        if (this._characterController.bPressingMove)
            this.HandlePressMove(this._characterController.InputValueMove, true);
        else
            this.HandleReleaseMove();
    }

    public virtual void I_AN_AtackCombo()
    {
        this._characterAttack?.AN_Combo();
    }

    public void I_AN_RunAttackToRun()
    {
        if (this._characterController.bPressingMove && this._characterMesh.bCanAttackToRun)
        {
            this.I_AN_AttackEnd();
            this.HandlePressMove(this._characterController.InputValueMove, false);
            this._characterMesh?.RequestAttackToRun();
        }
    }

    public void I_AN_AttackStopMoving()
    {
        this._characterMovement?.RequestIdle();
    }

    public void I_AN_TraceStart()
    {
        this._characterAttack?.AN_TraceStart();
    }

    public void I_AN_TraceEnd()
    {
        this._characterAttack?.AN_TraceEnd();
    }

    public void I_AN_TraceHit()
    {
        this._characterAttack?.AN_TraceHit();
    }

    public virtual void I_EnterCombat(BaseCharacter_Old character)
    {
        this.attackInterface_Target = character.GetComponent<IAttackInterface>();
        this._characterMovement.ChangeMaxMovementSpeed(this._characterSO.CombatSpeed);
    }

    public virtual void I_ExitCombat()
    {
        this._characterMovement.ChangeMaxMovementSpeed(this._characterSO.DefaultSpeed);
    }

    public virtual void I_HitTarget(float Health_Target, float MaxHealth_Target) { }

    public virtual void I_HandleTargetDestroyed()
    {
        this.I_ExitCombat();
    }

    #endregion

        #region Handle Pain - Dealth
    public virtual void HandleDead()
    {
        this._characterMovement?.RequestIdle();
        this._characterMesh?.RequestDeathUpdate();
        SoundSpawner.Instance.PlayAudio(this._characterSO.DeathAudio);
    }

    public virtual void AN_DeadEnd()
    {
        this.attackInterface_Target?.I_HandleTargetDestroyed();
        this.gameObject.SetActive(false);
    }

    public virtual void HandlePain()
    {
        this._characterMovement?.RequestIdle();
        this._characterMesh?.RequestPainUpdate();

        // Spawn effect
        Transform painEffect = EffectSpawner.Instance.SpawnObject(EffectSpawner.BloodEffect, this.transform.position, this.transform.rotation);
        painEffect.SetParent(this.transform);
        painEffect.gameObject.SetActive(true);

        // Play Sound
        SoundSpawner.Instance.PlayAudio(this._characterSO.PainAudio);
    }

    public virtual void AN_PainEnd()
    {
        this._characterHealth?.AN_PainEnd();
        this.I_AN_AttackEnd();

        if (this._characterController.bPressingMove)
            this._characterMesh?.RequestAttackToRun();

    }

        #endregion

}
