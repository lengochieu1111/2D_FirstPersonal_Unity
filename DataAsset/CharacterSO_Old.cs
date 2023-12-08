using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RyoScriptableObject", menuName = "SO/Character")]
public class CharacterSO_Old : ScriptableObject
{
    [Header("Move")]
    public float DefaultSpeed = 6f;
    public float CombatSpeed = 4f;
    public float PatrolSpeed = 3f;
    public float SprintSpeed = 8f;

    public float JumpHeight = 26f;

    [Header("Anim")]
    public readonly int Anim_Idle = Animator.StringToHash("Anim_Idle");
    public readonly int Anim_Run = Animator.StringToHash("Anim_Run");
    public readonly int Anim_Jump = Animator.StringToHash("Anim_JumpStart");

    public readonly List<int> Anim_NormalAttacks_Idle = new List<int> {
            Animator.StringToHash("Anim_AttackOne"),
            Animator.StringToHash("Anim_AttackTwo"),
    };
    
    public readonly int Anim_NormalAttack_Run = Animator.StringToHash("Anim_AttackThree");

    public readonly int Anim_StrongAttacks = Animator.StringToHash("Anim_AttackFour");

    public readonly int Anim_Pain = Animator.StringToHash("Anim_Pain");
    public readonly int Anim_Death = Animator.StringToHash("Anim_Death");

    [Header("Stats")]
    public float Damage = 20;
    public float SightRadius = 7;

    [Header("Health")]
    public float Health = 100;
    public float MaxHealth = 100;

    [Header("Sound")]
    public AudioClip PainAudio;
    public AudioClip DeathAudio;
    public AudioClip WeaponTrailAudio;
    public AudioClip WeaponHitAudio;


}
