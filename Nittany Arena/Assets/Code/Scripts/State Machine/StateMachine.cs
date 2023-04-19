using UnityEngine;
<<<<<<< Updated upstream

public class StateMachine : MonoBehaviour
{
=======
using Prime31;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class StateMachine : MonoBehaviour
{
    GameManager gameManager;
>>>>>>> Stashed changes
    private class ChangeStateCommand
    
    {
        
        private StateMachine m_StateMachine;
        private BaseState m_NewState;

<<<<<<< Updated upstream
        public ChangeStateCommand(StateMachine stateMachine, BaseState newState)
=======
        

        public ChangeStateCommand(StateMachine stateMachine, PlayerStateFactory stateFactory, BaseState newState)
>>>>>>> Stashed changes
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

    [SerializeField] private InputData m_inputData;
    [SerializeField] private BaseState m_StartState;

    private void Awake()
    {
        m_PlayerStateFactory = GetComponent<PlayerStateFactory>();
        m_CurrentState = m_StartState;
    }

    private void Start()
    {
<<<<<<< Updated upstream
=======
        
        //TODO: get player count reference proper
        //m_PlayerCount = inputMatcher.m_PlayerCount;
        gameManager = GameManager.Instance;

        m_Controller.onTriggerExitEvent += OnColliderTriggerExit;

>>>>>>> Stashed changes
        m_CurrentState = m_PlayerStateFactory.GroundedState;
        m_CurrentState.StateFactory = m_PlayerStateFactory;

        if (m_CurrentState != null)
        {
            m_CurrentState.StateManager = this;
            m_CurrentState.OnEnter();
        }
<<<<<<< Updated upstream
=======
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
>>>>>>> Stashed changes
    }

    private void FixedUpdate()
    {
        m_CurrentState?.OnUpdate();

        m_ChangeState?.Execute();
        m_ChangeState = null;
<<<<<<< Updated upstream
=======
        //Debug.Log("button pressed status: " + m_inputData.CurrentFrame.ButtonSouth);
       
    
        if (gameManager.gameOver && m_inputData.CurrentFrame.ButtonSouth)
        {
            Debug.Log("button was pressed");
            gameManager.ReloadScene();
        }
    
>>>>>>> Stashed changes
    }

    public void ChangeState(BaseState state)
    {
        m_ChangeState = new ChangeStateCommand(this, state);
    }
<<<<<<< Updated upstream
=======

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

    

    
>>>>>>> Stashed changes
}
