using UnityEngine;

[CreateAssetMenu(menuName = "Fighting Game/Control Layout Data")]
public class ControlLayoutData : ScriptableObject
{
    public enum StickControls
    {
        None,

        Movement,

        Count,
    }

    public enum ButtonControls
    {
        None,

        Jump,
        Attack,

        Count,
    }

    [Header("Stick Controls")]
    public StickControls LeftStick;
    public StickControls RightStick;

    [Header("Button Controls")]
    public ButtonControls ButtonNorth;
    public ButtonControls ButtonEast;
    public ButtonControls ButtonSouth;
    public ButtonControls ButtonWest;

    [Header("Other Settings")]
    [Range(0.0f, 1.0f)] public float StickSmashSensitivity;
}
