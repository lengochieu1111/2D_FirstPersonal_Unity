using System.Collections;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class CharacterMeshComponent : CharacterAbstract
{
    [SerializeField] private Animator _animator;
    [SerializeField] private IAttackInterface _attackInterface;
    [SerializeField] public bool bCanAttackToRun;
    private bool _bFlipLeft;
    private int _iAnimState;

    private Coroutine croutine;

    public Animator Animator => _animator;
    public bool bFlipLeft => _bFlipLeft;

    protected override void LoadComponents()
    {
        base.LoadComponents();

        if (this._animator == null)
            this._animator = GetComponent<Animator>();

        if (this._attackInterface == null)
            this._attackInterface = GetComponentInParent<IAttackInterface>();
    }

    protected override void SetupValues()
    {
        base.SetupValues();
        this._bFlipLeft = false;
        this.bCanAttackToRun = false;
        this._iAnimState = this.baseCharacter.CharacterSO.Anim_Idle;
    }

    public void RequestIdleUpdate()
    {
        this._animator.SetBool("bIsMoving", false);
    }

    public void RequestMigrationUpdate(float moveValue)
    {
        this._animator.SetBool("bIsMoving", true);

    }

    public void RequestJumpUpdate()
    {
        this._animator.SetTrigger("bIsJumping");
        this._animator.SetBool("bIsFalling", true);
    }

    public void RequestAttackUpdate(int iAttackState)
    {
        this.RequestUpdateState(iAttackState);
    }

    public void RequestAttackToRun()
    {
        this.RequestUpdateState(this.baseCharacter.CharacterSO.Anim_Run);
    }

    public void RequestDeathUpdate()
    {
        this.RequestUpdateState(this.baseCharacter.CharacterSO.Anim_Death);
    }

    public void RequestPainUpdate()
    {
        this.RequestUpdateState(this.baseCharacter.CharacterSO.Anim_Pain);
    }

    public void AN_Rising()
    {
        this._animator.SetFloat("yVelocity", this.baseCharacter.Rigidbody.velocity.y);
    }
    
    public void AN_Falling()
    {
        if (this.baseCharacter.CharacterCapsule.bIsFalling)
            this._animator.SetBool("bIsFalling", true);
        else
            this._animator.SetBool("bIsFalling", false);
    }

    public void AN_FromGroundToFalling()
    {
        bool bFromGroundToFalling = this.baseCharacter.CharacterCapsule.bIsFalling
            && !this.baseCharacter.CharacterMovement.bIsJumping;

        if (bFromGroundToFalling)
            this._animator.SetBool("bIsFalling", true);
    }

    public void AN_RunAttackEndToRun(float moveVector)
    {
        if (moveVector != 0)
        {
            this.RequestFlipMesh(moveVector);
            this.RequestUpdateState(this.baseCharacter.CharacterSO.Anim_Run);
        }
    }

    public void RequestFlipMesh(float moveValue)
    {
        float fAngle = 0;
        if (moveValue > 0)
            fAngle = 0;
        else if (moveValue < 0)
            fAngle = -180;

        bool bCanFlip = (this._bFlipLeft == true && fAngle == 0)
            || (this._bFlipLeft == false && fAngle == -180);

        if (bCanFlip)
        {
            this.FlipMesh(fAngle);
            this._bFlipLeft = !this._bFlipLeft;
        }
    }

    private void FlipMesh(float fAngle)
    {
        Vector3 rotator = new Vector3(this.transform.rotation.x, fAngle, this.transform.rotation.z);
        this.transform.rotation = Quaternion.Euler(rotator);
    }

    private void RequestUpdateState(int iState)
    {
        this._iAnimState = iState;
        this.UpdateState(); 
    }

    private void UpdateState()
    {
        //this._animator.Play(this._iAnimState, 0, 0f);
        this._animator.PlayInFixedTime(this._iAnimState, 0, 0f);
    }

    #region Call the function in animation event attack

    private void AE_AttackCombo()
    {
        this._attackInterface?.I_AN_AtackCombo();
    }

    private void AE_AttackEnd()
    {
        this._attackInterface?.I_AN_AttackEnd();
    }

    private void AE_CanAttackToRun()
    {
        this.bCanAttackToRun = true;
    }

    private void AE_AttackStopMoving()
    {
        this._attackInterface?.I_AN_AttackStopMoving();
    }

    private void AE_TraceStart()
    {
        this._attackInterface?.I_AN_TraceStart();
    }

    private void AE_TraceEnd()
    {
        this._attackInterface?.I_AN_TraceEnd();
    }

    private void AE_PainEnd()
    {
        this.baseCharacter?.AN_PainEnd();
    }

    #endregion


}
