using Prime31;
using UnityEngine;

public class PlayerStateFall : BaseState
{
    private CharacterController2D m_Controller;
    private Vector2 m_Velocity;
    private PlayerData m_PlayerData;

    private float m_MaxVelocity;
    private float m_VelocitySign;

    private uint m_RemainingAirJumps;

    private void Awake()
    {
        m_Controller = GetComponent<CharacterController2D>();
        m_PlayerData = GetComponent<PlayerData>();
        m_RemainingAirJumps = m_PlayerData.MovementData.AirJumps;
    }

    public override void OnEnter()
    {
        m_Velocity = m_Controller.velocity;
        m_MaxVelocity = m_Velocity.x;
        m_VelocitySign = Mathf.Sign(m_Velocity.x);
    }

    public override void OnExit()
    {

    }

    public override void OnUpdate()
    {
        float gravityScale = m_PlayerData.MovementData.GravityScale;
        PlayerControlsData controlsData = m_PlayerData.PlayerControlsData;

        if (m_Velocity.y > 0.0f && !controlsData.CurrentFrame.Jump)
            gravityScale *= m_PlayerData.MovementData.LowJumpMultiplier;
        
        if (m_Velocity.y < 0.0f)
        {
            gravityScale *= m_PlayerData.MovementData.FallMultiplier;

            //TODO: Implement fastfalling
        }

        m_Velocity.y += Physics2D.gravity.y * Time.fixedDeltaTime * gravityScale;

        if (controlsData.CurrentFrame.Attack && !controlsData.PreviousFrame.Attack)
            StateManager.ChangeState(StateFactory.DefaultAttackState);

        if (m_Controller.isGrounded)
        {
            m_RemainingAirJumps = m_PlayerData.MovementData.AirJumps;
            StateManager.ChangeState(StateFactory.GroundedState);
        }

        if (controlsData.CurrentFrame.Jump && !controlsData.PreviousFrame.Jump)
        {
            if (m_RemainingAirJumps > 0)
            {
                m_RemainingAirJumps--;
                StateManager.ChangeState(StateFactory.JumpSquatState);

                float speed = controlsData.CurrentFrame.Movement.x * m_PlayerData.MovementData.WalkSpeed;
                m_Velocity.x = speed;

                m_Controller.move(m_Velocity * Time.fixedDeltaTime);
            }
        }

        float maxSpeedChange = m_PlayerData.MovementData.WalkSpeed * m_PlayerData.MovementData.AirSpeed;
        float targetSpeed = controlsData.CurrentFrame.Movement.x * maxSpeedChange;
        float movementSign = Mathf.Sign(controlsData.CurrentFrame.Movement.x);
        float maxSpeed = (Mathf.Abs(m_MaxVelocity) + maxSpeedChange) * m_VelocitySign;

        if (controlsData.CurrentFrame.Movement.x != 0.0f)
            m_Velocity.x = Mathf.MoveTowards(m_Velocity.x, movementSign == m_VelocitySign ? maxSpeed
                : -maxSpeed, Mathf.Abs(targetSpeed) * Time.fixedDeltaTime);

        m_Controller.move(m_Velocity * Time.fixedDeltaTime);
    }
}
