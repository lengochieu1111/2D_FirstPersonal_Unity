using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackInterface
{
    public abstract void I_PlayAttackAnim(int iStateAttack);
    public abstract void I_AN_StartAttack();
    public abstract void I_AN_AttackEnd();
    public abstract void I_AN_AtackCombo();

    public abstract void I_AN_RunAttackToRun();

    public abstract void I_AN_AttackStopMoving();

    public abstract void I_AN_TraceStart();
    public abstract void I_AN_TraceEnd();
    public abstract void I_AN_TraceHit();

    public abstract void I_EnterCombat(BaseCharacter_Old character);
    public abstract void I_ExitCombat();

    public virtual void I_HitTarget(float Health_Target, float MaxHealth_Target) { }

    public abstract void I_HandleTargetDestroyed();


}
