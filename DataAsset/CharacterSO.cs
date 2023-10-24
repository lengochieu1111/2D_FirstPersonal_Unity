using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RyoScriptableObject", menuName = "SO/Character")]
public class CharacterSO : ScriptableObject
{
    [Header("Move")]
    public float FDefaultSpeed = 11f;
    public float FCombatSpeed = 6f;
    public float FDefaultJumpHeight = 40f;
    public float FAttackJumpHeight = 35f;

    [Header("State")]
    public readonly int Anim_Idle = Animator.StringToHash("Anim_Idle");
    public readonly int Anim_Run = Animator.StringToHash("Anim_Run");
    public readonly int Anim_Jump = Animator.StringToHash("Anim_JumpStart");

    public readonly List<int> Anim_NormalAttacks = new List<int> {
            Animator.StringToHash("Anim_AttackOne"),
            Animator.StringToHash("Anim_AttackTwo"),
            Animator.StringToHash("Anim_AttackThree")
    };

    public readonly int Anim_StrongAttacks = Animator.StringToHash("Anim_AttackFour");
            //Animator.StringToHash("Anim_AttackThree"),

    public readonly int Anim_Pain = Animator.StringToHash("Anim_Pain");
    public readonly int Anim_Death = Animator.StringToHash("Anim_Death");

    [Header("Health")]
    public float FDamage = 20;
    public float FHealth = 100;
    public float FMaxHealth = 100;

    public float FSightRadius = 7;

    [Header("Sound")]
    public AudioClip PainAudio;
    public AudioClip DeathAudio;
    public AudioClip WeaponTrailAudio;
    public AudioClip WeaponHitAudio;


}
