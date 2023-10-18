using System.Collections;
using UnityEngine;

public class CharacterMovementComponent : CharacterAbstract
{
    [Header("Character Movement")]
    [SerializeField] private float _fDefaultSpeed;
    [SerializeField] private float _fCombatSpeed;
    [SerializeField] private float _fDefaultJumpHeight;
    [SerializeField] private float _fAttackJumpHeight;
    [SerializeField] private float _fAccelerationTime;
    private float _fMovementSpeed;
    private float _fJumpHeight;
    private Vector2 _moveVector;
    private bool _bIsAccelerating;  
    private float _fCounter;
    public bool BPressingMove => _moveVector.x != 0;

    protected override void SetupValues()
    {
        base.SetupValues();

        this._fAccelerationTime = 0.2f;
        this._bIsAccelerating = false;
        this._fDefaultSpeed = this.baseCharacter.CharacterSO.FDefaultSpeed;
        this._fCombatSpeed = this.baseCharacter.CharacterSO.FCombatSpeed;
        this._fDefaultJumpHeight = this.baseCharacter.CharacterSO.FDefaultJumpHeight;
        this._fAttackJumpHeight = this.baseCharacter.CharacterSO.FAttackJumpHeight;
    }

    private void FixedUpdate()
    {
        if (this._moveVector.x == 0) return;

        this.UpdateMovementSpeed();

        this.AccelerationProcess();

        this.Movement();
    }

    public void RequestMove(Vector2 moveVector, bool bIsAccelerating = true)
    {
        if (this.baseCharacter.CharacterMesh.BIsAttacking == true 
            && moveVector.x != this._moveVector.x 
            && moveVector.x != 0) return;

        this._moveVector = moveVector;

        if (this._moveVector.x != 0)
        {
            this._fCounter = this._fAccelerationTime;
            this._bIsAccelerating = bIsAccelerating;
        }
    }

    private void AccelerationProcess()
    {
        if (this._bIsAccelerating == false || this.baseCharacter.CharacterMesh.BIsJumping == true
            || this.baseCharacter.CharacterMesh.BIsAttacking == true)
            return;
        else
        {
            if (this._fCounter > 0)
            {
                this._fMovementSpeed = this._fDefaultSpeed * this._fCounter;
                this._fCounter -= Time.deltaTime;
            }
            else
            {
                if (this._moveVector.x != 0)
                {
                    this.baseCharacter.CharacterMesh.RequestUpdateState(this.baseCharacter.CharacterSO.Anim_Run);
                    this._fMovementSpeed = this._fDefaultSpeed;
                    this._bIsAccelerating = false;
                }

            }
        }
    }

    private void Movement()
    {
        this.baseCharacter.Rigidbody.velocity = new Vector2(this._moveVector.x * this._fMovementSpeed, this.baseCharacter.Rigidbody.velocity.y);
    }

    public void RequestJump()
    {
        this.UpdateJumpHeight();

        this.Jump();
    }

    private void UpdateJumpHeight()
    {
        if (this.baseCharacter.CharacterMesh.BIsAttacking == true)
            this._fJumpHeight = this._fAttackJumpHeight;
        else
            this._fJumpHeight = this._fDefaultJumpHeight;
    }

    private void UpdateMovementSpeed()
    {
        if (this.baseCharacter.CharacterMesh.BIsAttacking == true)
            this._fMovementSpeed = this._fCombatSpeed;
        else
            this._fMovementSpeed = this._fDefaultSpeed;
    }

    private void Jump()
    {
        this.baseCharacter.Rigidbody.velocity = new Vector2 (this.baseCharacter.Rigidbody.velocity.x, this._fJumpHeight);
    }

}
