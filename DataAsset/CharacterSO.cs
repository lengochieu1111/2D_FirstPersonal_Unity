using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RyoScriptableObject", menuName = "Data/Character")]
public class CharacterSO : ScriptableObject
{
    [Header("Move")]
    public float DefaulSpeed = 5f;
    public float AirWalkingSpeed = 10f;
    public float RunSpeed = 8f;
    public float JumpHeight = 20f;
    public float TimeToFlyUp = 0.2f;

}
