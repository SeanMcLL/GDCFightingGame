using UnityEngine;

[RequireComponent(typeof(PlayerData))]
public class InputDataTracker : MonoBehaviour
{
    private PlayerData m_PlayerData;

    private void Awake() 
    {
        m_PlayerData = GetComponent<PlayerData>();
    }

    private void FixedUpdate()
    {
        m_PlayerData.PlayerControlsData.PreviousFrame = m_PlayerData.PlayerControlsData.CurrentFrame.Copy();

        Vector2[] sticks = new Vector2[(int)ControlLayoutData.StickControls.Count];
        bool[] buttons = new bool[(int)ControlLayoutData.ButtonControls.Count];

        //Sticks
        sticks[(int)m_PlayerData.ControlLayoutData.LeftStick] = m_PlayerData.InputData.CurrentFrame.StickLeft;
        sticks[(int)m_PlayerData.ControlLayoutData.RightStick] = m_PlayerData.InputData.CurrentFrame.StickRight;

        m_PlayerData.PlayerControlsData.CurrentFrame.Movement = sticks[(int)ControlLayoutData.StickControls.Movement];

        //Buttons
        buttons[(int)m_PlayerData.ControlLayoutData.ButtonNorth] |= m_PlayerData.InputData.CurrentFrame.ButtonNorth;
        buttons[(int)m_PlayerData.ControlLayoutData.ButtonEast] |= m_PlayerData.InputData.CurrentFrame.ButtonEast;
        buttons[(int)m_PlayerData.ControlLayoutData.ButtonSouth] |= m_PlayerData.InputData.CurrentFrame.ButtonSouth;
        buttons[(int)m_PlayerData.ControlLayoutData.ButtonWest] |= m_PlayerData.InputData.CurrentFrame.ButtonWest;

        m_PlayerData.PlayerControlsData.CurrentFrame.Jump = buttons[(int)ControlLayoutData.ButtonControls.Jump];
        m_PlayerData.PlayerControlsData.CurrentFrame.Attack = buttons[(int)ControlLayoutData.ButtonControls.Attack];
    }
}
