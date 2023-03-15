using Prime31;
using UnityEngine;
using System.Collections.Generic;

public class PlayerStateDefaultAttackState : BaseState
{
    private uint m_RemainingFrames = 0;
    private const uint k_TotalFrames = 3;
    private CharacterController2D m_Controller;
    private Vector2 m_Velocity;
    private LayerMask m_HurtboxLayerMask;
    private readonly List<GameObject> m_MarkedPlayers = new List<GameObject>();

    private void Awake()
    {
        m_Controller = GetComponent<CharacterController2D>();
        m_HurtboxLayerMask = 1 << LayerMask.NameToLayer("Hurtboxes");
    }

    public override void OnEnter()
    {
        m_RemainingFrames = k_TotalFrames;
        m_Velocity = m_Controller.velocity;
        m_MarkedPlayers.Clear();
    }

    public override void OnExit()
    {

    }

    public override void OnUpdate()
    {
        if (m_RemainingFrames == 0)
            StateManager.ChangeState(StateFactory.PreviousState);

        Collider2D[] collided = Physics2D.OverlapBoxAll(transform.position, Vector2.one * 3.0f, 0.0f, m_HurtboxLayerMask);
        
        foreach (Collider2D collider in collided)
        {
            GameObject parent = collider.transform.parent.gameObject;

            if (!parent.TryGetComponent<StateMachine>(out StateMachine stateMachine))
                continue;

            if (parent == gameObject)
                continue;

            if (m_MarkedPlayers.Contains(parent))
                continue;

            m_MarkedPlayers.Add(parent);

            //TODO: Apply damage and knockback
            stateMachine.TakeDamage();
        }

        m_Controller.move(m_Velocity * Time.fixedDeltaTime);

        m_RemainingFrames--;
    }
}