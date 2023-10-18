using UnityEngine;

public class CharacterMeshComponent : CharacterAbstract
{
    [SerializeField] private Animator _animator;
    [SerializeField] private bool _bIsJumping;
    [SerializeField] private bool _bIsAttacking;
    [SerializeField] private bool _bIsPaining;
    [SerializeField] private bool _bIsDead;
    private bool _bFlipLeft;
    private int _iAnimState;
    public bool BIsJumping { set => _bIsJumping = value;  get => _bIsJumping; }
    public bool BIsAttacking { set => _bIsAttacking = value;  get => _bIsAttacking; }
    public bool BIsPaining { set => _bIsPaining = value;  get => _bIsPaining; }
    public bool BIsDead { set => _bIsDead = value;  get => _bIsDead; }

    public Animator Animator => _animator;

    protected override void LoadComponents()
    {
        base.LoadComponents();

        if (this._animator == null)
            this._animator = GetComponent<Animator>();
    }

    protected override void SetupValues()
    {
        base.SetupValues();
        this._bIsJumping = false;
        this._bIsAttacking = false;
        this._bIsDead = false;
        this._bIsPaining = false;
        this._bFlipLeft = false;
        this._iAnimState = this.baseCharacter.CharacterSO.Anim_Idle;
    }

    public void RequestUpdateState(int iState)
    {
        if (this._iAnimState == iState) return;
        this._iAnimState = iState;
        this.UpdateState(); 
    }

    private void UpdateState()
    {
        this._animator.CrossFade(this._iAnimState, 0, 0);
    }

    public void RequestFlip(float fAngle)
    {
        bool bCanFlip = (this._bFlipLeft == true && fAngle == 0)
            || (this._bFlipLeft == false && fAngle == -180);

        if (bCanFlip == false) return;

        this.FlipMesh(fAngle);
        this._bFlipLeft = !this._bFlipLeft;
    }

    private void FlipMesh(float fAngle)
    {
        Vector3 rotator = new Vector3(this.transform.rotation.x, fAngle, this.transform.rotation.z);
        this.transform.rotation = Quaternion.Euler(rotator);
    }


        #region Call the function in animation event attack

    private void AttackCombo_AnimEvent()
    {
        this.baseCharacter?.I_AE_AttackCombo();
    }

    private void AttackEnd_AnimEvent()
    {
        this.baseCharacter?.I_AE_AttackEnd();
    }
    
    private void TraceStart_AnimEvent()
    {
        this.baseCharacter?.I_AE_TraceStart();
    }

    private void TraceEnd_AnimEvent()
    {
        this.baseCharacter?.I_AE_TraceEnd();
    }

        #endregion
}
