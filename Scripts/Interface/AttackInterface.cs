using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackInterface
{
    public abstract void I_PlayAttackAnim(int iStateAttack);
    public abstract void I_AE_AttackEnd();
    public abstract void I_AE_AttackCombo();

    public abstract void I_AE_TraceStart();
    public abstract void I_AE_TraceEnd();
    public abstract void I_AE_TraceHit();

    public abstract void I_EnterCombat(BaseCharacter character);

    public virtual void I_HitTarget(float Health_Target, float MaxHealth_Target) { }

}
