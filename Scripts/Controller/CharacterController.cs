using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : RyoMonoBehaviour
{
    [Header("Character Controller")]
    [SerializeField] protected BaseCharacter baseCharacter;
    [SerializeField] protected Vector2 moveInput;
    public Vector2 MoveInput => moveInput;
    public BaseCharacter BaseCharacter => baseCharacter;

    protected override void LoadComponents()
    {
        base.LoadComponents();

        if (this.baseCharacter == null)
            this.baseCharacter = GetComponent<BaseCharacter>();
    }

    #region Handle Input

    protected virtual void HandleMoveInput()
    {
        this.baseCharacter?.HandlePressedMove(this.moveInput);
    }

    protected virtual void HandleJumpInput()
    {
        this.baseCharacter?.HandlePressedJump();
    }

    protected virtual void HandleNormalAttackInput()
    {
        this.baseCharacter?.HandlePressedAttack(EAttackType.Normal);
    }

    protected virtual void HandleStrongAttackInput()
    {
        this.baseCharacter?.HandlePressedAttack(EAttackType.Strong);
    }

    #endregion

}
