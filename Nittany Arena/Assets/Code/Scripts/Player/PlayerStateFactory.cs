using UnityEngine;

public class PlayerStateFactory : MonoBehaviour
{
    private PlayerStateGrounded m_GroundedState;
    private PlayerStateFall m_FallState;
    private PlayerStateJumpSquat m_JumpSquatState;
    private PlayerStateDefaultAttackState m_DefaultAttackState;
    private BaseState m_PreviousState = null, m_CurrentState = null;

    public PlayerStateGrounded GroundedState => m_GroundedState;
    public PlayerStateFall FallState => m_FallState;
    public PlayerStateJumpSquat JumpSquatState => m_JumpSquatState;
    public PlayerStateDefaultAttackState DefaultAttackState => m_DefaultAttackState;
    public BaseState PreviousState => m_PreviousState;
    public BaseState CurrentState => m_CurrentState;

    private void Awake()
    {
        m_GroundedState = gameObject.GetOrAddComponent<PlayerStateGrounded>();
        m_FallState = gameObject.GetOrAddComponent<PlayerStateFall>();
        m_JumpSquatState = gameObject.GetOrAddComponent<PlayerStateJumpSquat>();
        m_DefaultAttackState = gameObject.GetOrAddComponent<PlayerStateDefaultAttackState>();
    }

    public void OnStateChanged(BaseState newState)
    {
        m_PreviousState = m_CurrentState;
        m_CurrentState = newState;
    }
}
