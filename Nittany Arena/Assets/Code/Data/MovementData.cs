using UnityEngine;

[CreateAssetMenu(fileName = "New Movement Data", menuName = "Fighting Game/Movement Data")]
public class MovementData : ScriptableObject
{
    [Header("Gravity Settings")]
    [Min(0.0f)] public float GravityScale;

    [Header("Walking Settings")]
    [Min(0.0f)] public float WalkSpeed;
    [Range(0.001f, 1.0f)]  public float Friction;

    [Header("Air Settings")]
    [Range(0.0f, 1.0f)] public float AirSpeed;

    [Header("Jumping Settings")]
    [Min(0.0f)] public float GroundJumpHeight;
    [Min(0.0f)] public float AirJumpHeight;
    [Min(1.0f)] public float LowJumpMultiplier;
    [Min(1.0f)] public float FallMultiplier;
    [Min(1.0f)] public float FastFallMultiplier;
    public uint AirJumps;
}
