using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : BaseCharacter_Old, IEnemyInterface
{
    [Header("")]
    [SerializeField] private EnemyAIController _enemyAIController;

    protected override void LoadComponents()
    {
        base.LoadComponents();

        this._enemyAIController = this.CharacterController.GetComponent<EnemyAIController>();
    }

    public override void I_AN_AttackEnd()
    {
        base.I_AN_AttackEnd();
        this._enemyAIController?.AttackCoolDown();
    }

    public override void I_AN_AtackCombo()
    {
        base.I_AN_AtackCombo();
        this._enemyAIController?.AttackCoolDown();

    }

    protected override void HandleTakePointDamage(BaseCharacter_Old characterCauses, float fDamage)
    {
        base.HandleTakePointDamage(characterCauses, fDamage);

        this.attackInterface_Target?.I_HitTarget(
            this._characterHealth.Health,
            this._characterHealth.MaxHealth
            );
    }

    public void I_HandleSeePlayer(BaseCharacter_Old character)
    {
        this.I_EnterCombat(character);
    }

    public override void I_EnterCombat(BaseCharacter_Old character)
    {
        base.I_EnterCombat(character);

        this.attackInterface_Target?.I_EnterCombat(this);
    }

    public override void I_ExitCombat()
    {
        base.I_ExitCombat();

        this.attackInterface_Target?.I_ExitCombat();
        this._enemyAIController?.BackToPatrol();
    }

    public override void AN_DeadEnd()
    {
        this.attackInterface_Target?.I_HandleTargetDestroyed();
        EnemySpawner.Instance.DestroyObject(this.gameObject);
    }

}
