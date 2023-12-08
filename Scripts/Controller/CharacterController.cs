using UnityEngine;
using UnityEngine.InputSystem;

public abstract class CharacterController : RyoMonoBehaviour
{
    [Header("Character Controller")]
    [SerializeField] protected BaseCharacter_Old baseCharacter;
    [SerializeField] protected float inputValueMove;
    public float InputValueMove => inputValueMove;
    public BaseCharacter_Old BaseCharacter => baseCharacter;
    [SerializeField] public bool bPressingMove => inputValueMove != 0;

    protected override void LoadComponents()
    {
        base.LoadComponents();

        if (this.baseCharacter == null)
            this.baseCharacter = GetComponent<BaseCharacter_Old>();
    }

        #region Handle Input

    protected virtual void PressMove()
    {
        this.baseCharacter?.HandlePressMove(this.inputValueMove);
    }
    
    protected virtual void ReleaseMove()
    {
        this.baseCharacter?.HandleReleaseMove();
    }

    protected virtual void PressJump()
    {
        this.baseCharacter?.HandlePressedJump();
    }
    
    protected virtual void PressSprint()
    {
        this.baseCharacter?.HandlePressSprint();
    }
    
    protected virtual void ReleaseSprint()
    {
        this.baseCharacter?.HandleReleaseSprint();
    }

    protected virtual void PressNormalAttack()
    {
        this.baseCharacter?.HandlePressNormalAttack();
    }

    protected virtual void PressStrongAttack()
    {
        this.baseCharacter?.HandlePressStrongAttack();
    }

    #endregion

}
