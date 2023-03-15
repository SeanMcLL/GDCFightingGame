using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct HitboxData {
    public enum HitboxShape {
        Box,
        Circle
    }
    //Hitbox Drawing Data
    public HitboxShape Shape;
    public Vector2 Size;
    public Vector2 Center;
    public float Rotation;
    //Hitbox Effect Data
    public float Damage;
    public Vector2 KnockbackDirection;
    public float BaseKnockback;
    public float KnockbackGrowth;
    public int ShieldAdvantage;
    public float ShieldDamage;
}

[System.Serializable]
public struct FrameData {
    public HitboxData[] Hitboxes;
}

[CreateAssetMenu(fileName = "AttackData", menuName = "GDCFightingGame/AttackData", order = 0)]
public class AttackData : ScriptableObject {
    //Frames
    public int StartupFrames;
    public int TotalFrames;
    public FrameData[] Frames;
}
