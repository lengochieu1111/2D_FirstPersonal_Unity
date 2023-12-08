using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : BaseCharacter_Old
{
    protected override void Start()
    {
        base.Start();

        FirstHUD.Intance.PlayerWidget.UpdateHealthBar_Player(
            this._characterHealth.Health,
            this._characterHealth.MaxHealth
            );
    }

    protected override void HandleTakePointDamage(BaseCharacter_Old characterCauses, float fDamage)
    {
        base.HandleTakePointDamage (characterCauses, fDamage);

        FirstHUD.Intance.PlayerWidget.UpdateHealthBar_Player(
            this._characterHealth.Health,
            this._characterHealth.MaxHealth
            );
    }

    public override void I_EnterCombat(BaseCharacter_Old character)
    {
        base.I_EnterCombat(character);

        FirstHUD.Intance.PlayerWidget.UpdateHealthBar_Enemy(
            character.CharacterHealth.Health,
            character.CharacterHealth.MaxHealth
            );

        FirstHUD.Intance.PlayerWidget.ShowHealthBar_Enemy(true);
    }

    public override void I_ExitCombat()
    {
        base.I_ExitCombat();

        FirstHUD.Intance.PlayerWidget.ShowHealthBar_Enemy(false);
    }

    public override void I_HitTarget(float Health_Target, float MaxHealth_Target) 
    {
        FirstHUD.Intance.PlayerWidget.UpdateHealthBar_Enemy(Health_Target, MaxHealth_Target);
    }


}
