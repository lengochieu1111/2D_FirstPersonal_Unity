using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : BaseCharacter
{
    protected override void Start()
    {
        base.Start();

        FirstHUD.Intance.PlayerWidget.UpdateHealthBar_Player(
            this._characterHealth.FHealth,
            this._characterHealth.FMaxHealth
            );
    }

    protected override void HandleTakePoinDamage(BaseCharacter characterCauses, float fDamage)
    {
        base.HandleTakePoinDamage (characterCauses, fDamage);

        FirstHUD.Intance.PlayerWidget.UpdateHealthBar_Player(
            this._characterHealth.FHealth,
            this._characterHealth.FMaxHealth
            );
    }

    public override void I_EnterCombat(BaseCharacter character)
    {
        base.I_EnterCombat(character);

        FirstHUD.Intance.PlayerWidget.UpdateHealthBar_Enemy(
            character.CharacterHealth.FHealth,
            character.CharacterHealth.FMaxHealth);

        FirstHUD.Intance.PlayerWidget.SetActiveHealthBar_Enemy(true);
    }

    public override void I_HitTarget(float Health_Target, float MaxHealth_Target) 
    {
        FirstHUD.Intance.PlayerWidget.UpdateHealthBar_Enemy(Health_Target, MaxHealth_Target);
    }

    public override void I_ExitCombat()
    {
        base.I_ExitCombat();

        FirstHUD.Intance.PlayerWidget.SetActiveHealthBar_Enemy(false);
    }


}
