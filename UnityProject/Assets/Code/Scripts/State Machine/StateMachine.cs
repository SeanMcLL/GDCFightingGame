using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private class ChangeStateCommand
    {
        private StateMachine m_StateMachine;
        private BaseState m_NewState;

        public ChangeStateCommand(StateMachine stateMachine, BaseState newState)
        {
            m_StateMachine = stateMachine;
            m_NewState = newState;
        }

        public void Execute()
        {
            m_StateMachine.m_CurrentState?.OnExit();
            m_StateMachine.m_CurrentState = m_NewState;

            if (m_StateMachine.m_CurrentState != null)
            {
                m_StateMachine.m_CurrentState.StateManager = m_StateMachine;
                m_StateMachine.m_CurrentState.StateFactory = m_StateMachine.m_PlayerStateFactory;
            }

            m_StateMachine.m_CurrentState?.OnEnter();
        }
    }

    private BaseState m_CurrentState = null;
    private ChangeStateCommand m_ChangeState = null;
    private PlayerStateFactory m_PlayerStateFactory = null;

    [SerializeField] private BaseState m_StartState;

    private void Awake()
    {
        m_PlayerStateFactory = GetComponent<PlayerStateFactory>();
        m_CurrentState = m_StartState;
    }

    private void Start()
    {
        m_CurrentState = m_PlayerStateFactory.GroundedState;
        m_CurrentState.StateFactory = m_PlayerStateFactory;

        if (m_CurrentState != null)
        {
            m_CurrentState.StateManager = this;
            m_CurrentState.OnEnter();
        }
    }

    private void FixedUpdate()
    {
        m_CurrentState?.OnUpdate();

        m_ChangeState?.Execute();
        m_ChangeState = null;
    }

    public void ChangeState(BaseState state)
    {
        m_ChangeState = new ChangeStateCommand(this, state);
    }
}
