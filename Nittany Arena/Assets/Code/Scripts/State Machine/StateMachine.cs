using Prime31;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class StateMachine : MonoBehaviour
{
    GameManager gameManager;
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
    private PlayerData m_Data;

    [SerializeField] private BaseState m_StartState;
    [SerializeField] private Player m_Player;
    [SerializeField] private Transform m_SpawnPoint;
    [SerializeField] private AttackData m_BasicAttack;

    private void Awake()
    {
        m_PlayerStateFactory = GetComponent<PlayerStateFactory>();
        m_CurrentState = m_StartState;
        m_Player.damagePercent = 0.0f;
        m_Player.stocks = 4;

        m_Controller = GetComponent<CharacterController2D>();

        m_Data = GetComponent<PlayerData>();
    }

    private void Start()
    {
        //TODO: get player count reference proper
        //m_PlayerCount = inputMatcher.m_PlayerCount;
        gameManager = GameManager.Instance;

        m_Controller.onTriggerExitEvent += OnColliderTriggerExit;

        m_CurrentState = m_PlayerStateFactory.GroundedState;
        m_CurrentState.StateFactory = m_PlayerStateFactory;

        if (m_CurrentState != null)
        {
            m_CurrentState.StateManager = this;
            m_CurrentState.OnEnter();
        }

        m_PlayerStateFactory.DefaultAttackState.CurrentAttack = m_BasicAttack;
    }

    private void OnColliderTriggerExit(Collider2D obj)
    {
        if (obj.gameObject.layer == LayerMask.NameToLayer("BlastZone"))
        {
            m_Player.stocks--;

            if (m_Player.stocks == 0)
            {
                //Destroy(gameObject);
                //GameData.DisablePlayer();
                gameManager.PlayerDefeated();

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
        if (gameManager.gameOver && m_Data.InputData.CurrentFrame.ButtonSouth)
            gameManager.ReloadScene();
    }

    public void ChangeState(BaseState state)
    {
        m_ChangeState = new ChangeStateCommand(this, m_PlayerStateFactory, state);
    }

    //TODO: Add arguments
    public void TakeDamage(float damage, Vector2 baseDirection, float baseKnockback, float knockbackGrowth)
    {
        //Calculate knockback
        m_PlayerStateFactory.KnockbackState.SetKnockback(baseDirection.normalized * (baseKnockback + 0.12f * knockbackGrowth * m_Player.damagePercent));

        //Add damage
        m_Player.damagePercent += damage;

        //Change to knockback state
        ChangeState(m_PlayerStateFactory.KnockbackState);
    }
}
