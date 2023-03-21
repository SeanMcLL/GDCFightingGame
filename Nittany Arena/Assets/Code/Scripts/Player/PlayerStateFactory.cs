using UnityEngine;

public class PlayerStateFactory : MonoBehaviour
{
    private PlayerStateGrounded m_GroundedState;
    private PlayerStateFall m_FallState;
    private PlayerStateJumpSquat m_JumpSquatState;

    public PlayerStateGrounded GroundedState => m_GroundedState;
    public PlayerStateFall FallState => m_FallState;
    public PlayerStateJumpSquat JumpSquatState => m_JumpSquatState;

    private void Awake()
    {
        m_GroundedState = gameObject.GetOrAddComponent<PlayerStateGrounded>();
        m_FallState = gameObject.GetOrAddComponent<PlayerStateFall>();
        m_JumpSquatState = gameObject.GetOrAddComponent<PlayerStateJumpSquat>();
    }
}
