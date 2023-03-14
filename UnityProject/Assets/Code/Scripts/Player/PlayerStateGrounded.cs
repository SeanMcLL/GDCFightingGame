using Prime31;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PlayerStateGrounded : BaseState
{
    private CharacterController2D m_Controller;
    private Vector2 m_Velocity;
    private PlayerData m_PlayerData;

    private void Awake()
    {
        m_Controller = GetComponent<CharacterController2D>();
        m_PlayerData = GetComponent<PlayerData>();
    }

    public override void OnEnter()
    {
        m_Velocity = m_Controller.velocity;
    }

    public override void OnExit()
    {

    }

    public override void OnUpdate()
    {
        m_Velocity.y = Physics2D.gravity.y * Time.fixedDeltaTime * m_PlayerData.MovementData.GravityScale;
        PlayerControlsData controlsData = m_PlayerData.PlayerControlsData;

        if (controlsData.CurrentFrame.Jump && !controlsData.PreviousFrame.Jump)
            StateManager.ChangeState(StateFactory.JumpSquatState);

        if (!m_Controller.isGrounded)
            StateManager.ChangeState(StateFactory.FallState);

        float targetSpeed = controlsData.CurrentFrame.Movement.x * m_PlayerData.MovementData.WalkSpeed;
        m_Velocity.x = CalculateVelocity(targetSpeed, m_Velocity.x, m_PlayerData.MovementData.Friction);

        m_Controller.move(m_Velocity * Time.fixedDeltaTime);
    }

    private float CalculateVelocity(float targetSpeed, float currentVelocity, float friction)
    {
        if (Mathf.Approximately(targetSpeed, currentVelocity))
            return currentVelocity;

        if (Mathf.Approximately(currentVelocity, 0.0f))
            return targetSpeed;

        if (targetSpeed == 0.0f)
            return Mathf.MoveTowards(currentVelocity, targetSpeed, Time.fixedDeltaTime / friction);

        if (Mathf.Sign(targetSpeed) == Mathf.Sign(currentVelocity))
        {
            if (Mathf.Abs(targetSpeed) < Mathf.Sign(currentVelocity))
                return Mathf.MoveTowards(currentVelocity, targetSpeed, Mathf.Abs(targetSpeed) * Time.fixedDeltaTime / friction);

            return targetSpeed;
        }

        return Mathf.MoveTowards(currentVelocity, targetSpeed, Mathf.Abs(targetSpeed) * Time.fixedDeltaTime / friction);
    }
}
