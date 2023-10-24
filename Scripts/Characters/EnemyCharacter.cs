using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : BaseCharacter, IEnemyInterface
{
    [Header("")]
    [SerializeField] private EnemyAIController _enemyAIController;

    protected override void LoadComponents()
    {
        base.LoadComponents();

        this._enemyAIController = this.CharacterController.GetComponent<EnemyAIController>();
    }

    public override void HandleDeadEnd()
    {
        this.attackInterface_Target?.I_HandleTargetDestroyed();
        EnemySpawner.Instance.DestroyObject(this.gameObject);
    }

    public override void I_AttackEnd()
    {
        this._characterMesh.BIsAttacking = false;
        this._characterAttack.NextAttack();
        this._enemyAIController?.AttackCoolDown();

        if (this._characterController.MoveInput.x != 0)
        {
            this._characterMesh.RequestUpdateState(this._characterSO.Anim_Run);
            this._characterMovement.RequestMove(this._characterController.MoveInput, false);
            this.HandleRequestFlipMesh(this._characterController.MoveInput);
        }
        else
            this._characterMesh.RequestUpdateState(this._characterSO.Anim_Idle);
    }

    public void I_HandleSeePlayer(BaseCharacter character)
    {
        this.I_EnterCombat(character);

    }

    protected override void HandleTakePoinDamage(BaseCharacter characterCauses, float fDamage)
    {
        base.HandleTakePoinDamage(characterCauses, fDamage);

        this.attackInterface_Target?.I_HitTarget(
            this._characterHealth.FHealth,
            this._characterHealth.FMaxHealth
            );
    }

    public override void I_EnterCombat(BaseCharacter character)
    {
        base.I_EnterCombat(character);

        this.attackInterface_Target?.I_EnterCombat(this);
    }

    public override void I_ExitCombat()
    {
        base.I_ExitCombat();

        this.attackInterface_Target?.I_ExitCombat();
        this._enemyAIController.BackToPatrol();
    }

}
