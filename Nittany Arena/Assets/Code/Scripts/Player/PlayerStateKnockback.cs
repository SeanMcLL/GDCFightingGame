using Prime31;
using UnityEngine;
using System.Collections.Generic;

public class PlayerKnockbackState : BaseState
{
    private float m_HitstunTime = 0.0f;
    private Vector2 m_KnockbackVelocity = Vector2.zero;

    private CharacterController2D m_Controller;

    private void Awake()
    {
        m_Controller = GetComponent<CharacterController2D>();
    }

    public override void OnEnter()
    {

    }

    public override void OnExit()
    {

    }

    public override void OnUpdate()
    {
        if (m_HitstunTime <= 0.0f)
        {
            if (m_Controller.isGrounded)
                StateManager.ChangeState(StateFactory.GroundedState);
            else
                StateManager.ChangeState(StateFactory.FallState);

            return;
        }

        //TODO: Use weight to calculate knockback speed and how fast the knockback approaches zero
    }
}
