using Prime31;
using UnityEngine;
using System.Collections.Generic;

public class PlayerKnockbackState : BaseState
{
    private float m_HitstunTime = 0.0f;
    private Vector2 m_KnockbackVelocity = Vector2.zero;
    private Vector2 m_GravityVelocity = Vector2.zero;
    private float m_InitialKnockbackSpeed;

    private CharacterController2D m_Controller;
    private PlayerData m_PlayerData;

    private void Awake()
    {
        m_Controller = GetComponent<CharacterController2D>();
        m_PlayerData = GetComponent<PlayerData>();
    }

    public void SetKnockback(Vector2 knockback)
    {
        m_KnockbackVelocity = knockback;
        m_InitialKnockbackSpeed = m_KnockbackVelocity.magnitude;
    }

    public override void OnEnter()
    {
        m_GravityVelocity = Vector2.zero;
        m_InitialKnockbackSpeed = m_KnockbackVelocity.magnitude;
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
        m_GravityVelocity.y += Physics2D.gravity.y * Time.fixedDeltaTime * m_PlayerData.MovementData.GravityScale;

        m_KnockbackVelocity = Vector2.MoveTowards(m_KnockbackVelocity, Vector2.zero, m_InitialKnockbackSpeed * Time.fixedDeltaTime);

        m_Controller.move(m_GravityVelocity + m_KnockbackVelocity);
    }
}
