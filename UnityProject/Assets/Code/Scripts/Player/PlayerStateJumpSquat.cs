using Prime31;
using UnityEngine;

public class PlayerStateJumpSquat : BaseState
{
    private CharacterController2D m_Controller;
    private Vector2 m_Velocity;
    private PlayerData m_PlayerData;

    private uint m_RemainingJumpSquatFrames = 0;

    private const uint JumpSquatFrames = 5;

    private float GroundedJumpVelocity
    {
        get
        {
            float gravityScale = m_PlayerData.MovementData.GravityScale;
            float targetJumpHeight = m_PlayerData.MovementData.GroundJumpHeight;

            return Mathf.Sqrt(Mathf.Abs(2 * Physics2D.gravity.y * gravityScale * targetJumpHeight));
        }
    }

    private float AirJumpVelocity
    {
        get
        {
            float gravityScale = m_PlayerData.MovementData.GravityScale;
            float targetJumpHeight = m_PlayerData.MovementData.AirJumpHeight;

            return Mathf.Sqrt(Mathf.Abs(2 * Physics2D.gravity.y * gravityScale * targetJumpHeight));
        }
    }

    private float JumpVelocity => m_Controller.isGrounded ? GroundedJumpVelocity : AirJumpVelocity;

    private void Awake()
    {
        m_Controller = GetComponent<CharacterController2D>();
        m_PlayerData = GetComponent<PlayerData>();
    }

    public override void OnEnter()
    {
        m_Velocity = m_Controller.velocity;
        m_RemainingJumpSquatFrames = JumpSquatFrames + 1;
    }

    public override void OnExit()
    {

    }

    public override void OnUpdate()
    {
        //TODO: Calculate Jump Height instead of using liftoff velocity
        m_Velocity.y = JumpVelocity;

        if (m_RemainingJumpSquatFrames > 0) 
            m_RemainingJumpSquatFrames--;
        else 
            StateManager.ChangeState(StateFactory.FallState);
        
        m_Controller.move(m_Velocity * Time.fixedDeltaTime);
    }
}
