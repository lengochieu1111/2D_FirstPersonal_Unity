using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : BaseCharacter, IEnemyInterface
{
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

}
