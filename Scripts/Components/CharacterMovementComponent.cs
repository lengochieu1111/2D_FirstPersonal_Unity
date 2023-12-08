using System;
using System.Collections;
using UnityEngine;

public class CharacterMovementComponent : CharacterAbstract
{
    [Header("Character Movement")]
    [SerializeField] private float _fAccelerationTime;
    [SerializeField] private bool _bIsAccelerating;  
    [SerializeField] private bool _bIsJumping;
    [SerializeField] private bool _bIsMoving => _moveValue != 0;
    [SerializeField] private float _fJumpRecoveryTime = 0.5f;
    private float _fJumpHeight;
    private float _fMovementSpeed;
    private float _moveValue;
    private float _fCounter;

    private float _fCounterJump;
    [SerializeField] private bool _bCanJump;
    public bool bIsMoving => _bIsMoving;
    public bool bIsJumping => _bIsJumping;
    public bool bCanJump => _bCanJump;
    public bool bIsAccelerating => _bIsAccelerating;

    protected override void SetupValues()
    {
        base.SetupValues();
        this._bCanJump = true;
        this._fAccelerationTime = 0.14f;
        this._bIsAccelerating = false;
        this._bIsJumping = false;
        this._fMovementSpeed = this.baseCharacter.CharacterSO.DefaultSpeed;
        this._fJumpHeight = this.baseCharacter.CharacterSO.JumpHeight;
    }

    private void FixedUpdate()
    {
        if (this._moveValue == 0) return;

        this.AccelerationProcess();

        this.Movement();
    }

    private void Update()
    {
        if (this._moveValue == 0) return;

        if (this.baseCharacter.CharacterCapsule.bIsOnWall)
            this.RequestIdle();

    }

    public void RequestIdle()
    {
        this._moveValue = 0;
        this.Idle();
    }

    private void Idle()
    {
        this.baseCharacter.Rigidbody.velocity = new Vector2(0, this.baseCharacter.Rigidbody.velocity.y);
    }

    public void RequestMove(float moveValue, bool bIsAccelerating = true)
    {
        this._moveValue = moveValue;
        this._bIsAccelerating = bIsAccelerating;

        if (this._bIsAccelerating)
            this._fCounter = this._fAccelerationTime;
        else
            this.baseCharacter.CharacterMesh.RequestMigrationUpdate(this._moveValue);

    }
     
    private void AccelerationProcess()
    {
        if (this._bIsAccelerating)
        {
            if (this._fCounter > 0)
            {
                this._fMovementSpeed = 0;
                this._fCounter -= Time.deltaTime;
            }
            else
            {
                if (this._moveValue != 0)
                {
                    this._bIsAccelerating = false;
                    this._fMovementSpeed = this.baseCharacter.CharacterSO.DefaultSpeed;
                    this.baseCharacter.CharacterMesh.RequestMigrationUpdate(this._moveValue);
                }
            }
        }
    }

    private void Movement()
    {
        this.baseCharacter.Rigidbody.velocity = new Vector2(this._moveValue * this._fMovementSpeed, this.baseCharacter.Rigidbody.velocity.y);
    }

    public void RequestJump()
    {
        this._bCanJump = false;
        this._bIsJumping = true;
        this.Jump();
    }
    
    private void Jump()
    {
        this.baseCharacter.Rigidbody.velocity = new Vector2 (this.baseCharacter.Rigidbody.velocity.x, this._fJumpHeight);
    }

    public void AN_JumpEnd()
    {
        this._bIsJumping = false;
        StartCoroutine(RegenJump());
    }

    private IEnumerator RegenJump()
    {
        yield return new WaitForSecondsRealtime(this._fJumpRecoveryTime);
        this._bCanJump = true;
    }

    public void ChangeMaxMovementSpeed(float movementSpeed)
    {
        this._fMovementSpeed = movementSpeed;
    }

}
