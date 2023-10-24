using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackInterface
{
    public abstract void I_PlayAttackAnim(int iStateAttack);
    public abstract void I_AttackEnd();
    public abstract void I_AtackCombo();

    public abstract void I_TraceStart();
    public abstract void I_TraceEnd();
    public abstract void I_AE_TraceHit();

    public abstract void I_EnterCombat(BaseCharacter character);
    public abstract void I_ExitCombat();

    public virtual void I_HitTarget(float Health_Target, float MaxHealth_Target) { }

    public abstract void I_HandleTargetDestroyed();


}
