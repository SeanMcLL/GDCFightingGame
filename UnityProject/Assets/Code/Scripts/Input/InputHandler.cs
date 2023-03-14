using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputHandler : MonoBehaviour 
{
    private InputData m_InputData = null;

    private void Awake() 
    {
        m_InputData = InputMatcher.Instance.GetInputData();
        
        if (m_InputData == null)
            Destroy(gameObject);
    }

    private void OnEnable() 
    {
        InputData.FrameInputData frame;

        frame.StickLeft = frame.StickRight = Vector2.zero;
        frame.ButtonNorth = frame.ButtonEast = frame.ButtonSouth = frame.ButtonWest = false;
        frame.LeftShoulder = frame.RightShoulder = frame.LeftTrigger = frame.RightTrigger = false;
        frame.Start = false;

        m_InputData.CurrentFrame = frame;
    }

    private void FixedUpdate() => m_InputData.PreviousFrame = m_InputData.CurrentFrame.Copy();

    private void OnStickLeft(InputValue value) => m_InputData.CurrentFrame.StickLeft = value.Get<Vector2>();
    private void OnStickRight(InputValue value) => m_InputData.CurrentFrame.StickRight = value.Get<Vector2>();

    private void OnButtonNorth(InputValue value) => m_InputData.CurrentFrame.ButtonNorth = value.isPressed;
    private void OnButtonEast(InputValue value) => m_InputData.CurrentFrame.ButtonEast = value.isPressed;
    private void OnButtonSouth(InputValue value) => m_InputData.CurrentFrame.ButtonSouth = value.isPressed;
    private void OnButtonWest(InputValue value) => m_InputData.CurrentFrame.ButtonWest = value.isPressed;

    private void OnLeftShoulder(InputValue value) => m_InputData.CurrentFrame.LeftShoulder = value.isPressed;
    private void OnRightShoulder(InputValue value) => m_InputData.CurrentFrame.RightShoulder = value.isPressed;
    private void OnLeftTrigger(InputValue value) => m_InputData.CurrentFrame.LeftTrigger = value.isPressed;
    private void OnRightTrigger(InputValue value) => m_InputData.CurrentFrame.RightTrigger = value.isPressed;

    private void OnStart(InputValue value) => m_InputData.CurrentFrame.Start = value.isPressed;
}
