using UnityEngine;
using Prime31;

public class StateMachine : MonoBehaviour
{
    private class ChangeStateCommand
    {
        private StateMachine m_StateMachine;
        private PlayerStateFactory m_StateFactory;
        private BaseState m_NewState;

        public ChangeStateCommand(StateMachine stateMachine, PlayerStateFactory stateFactory, BaseState newState)
        {
            m_StateMachine = stateMachine;
            m_StateFactory = stateFactory;
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

            m_StateFactory.OnStateChanged(m_NewState);
            m_StateMachine.m_CurrentState?.OnEnter();
        }
    }

    private BaseState m_CurrentState = null;
    private ChangeStateCommand m_ChangeState = null;
    private PlayerStateFactory m_PlayerStateFactory = null;
    private CharacterController2D m_Controller;

    [SerializeField] private BaseState m_StartState;
    [SerializeField] private Player m_Player;
    [SerializeField] private Transform m_SpawnPoint;

    private void Awake()
    {
        m_PlayerStateFactory = GetComponent<PlayerStateFactory>();
        m_CurrentState = m_StartState;
        m_Player.damagePercent = 0.0f;
        m_Player.stocks = 4;

        m_Controller = GetComponent<CharacterController2D>();
    }

    private void Start()
    {
        m_Controller.onTriggerExitEvent += OnColliderTriggerExit;

        m_CurrentState = m_PlayerStateFactory.GroundedState;
        m_CurrentState.StateFactory = m_PlayerStateFactory;

        if (m_CurrentState != null)
        {
            m_CurrentState.StateManager = this;
            m_CurrentState.OnEnter();
        }
    }

    private void OnColliderTriggerExit(Collider2D obj)
    {
        if (obj.gameObject.layer == LayerMask.NameToLayer("BlastZone"))
        {
            m_Player.stocks--;

            if (m_Player.stocks == 0)
            {
                Destroy(gameObject);
                return;
            }

            m_Player.damagePercent = 0;
            transform.position = m_SpawnPoint.position;
            m_Controller.move(Vector2.zero);
            ChangeState(m_PlayerStateFactory.FallState);
            m_ChangeState.Execute();
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
        m_ChangeState = new ChangeStateCommand(this, m_PlayerStateFactory, state);
    }

    //TODO: Add arguments
    public void TakeDamage(Vector2 baseDirection, float baseKnockback, float knockbackGrowth)
    {
        //Calculate knockback
        m_PlayerStateFactory.KnockbackState.SetKnockback(baseDirection.normalized * (baseKnockback + knockbackGrowth * m_Player.damagePercent));

        //Add damage
        m_Player.damagePercent += 5.0f;

        //Change to knockback state
        ChangeState(m_PlayerStateFactory.KnockbackState);
    }
}
