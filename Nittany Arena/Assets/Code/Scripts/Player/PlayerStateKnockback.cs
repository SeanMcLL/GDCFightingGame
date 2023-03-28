using Prime31;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Rendering;

public class PlayerKnockbackState : BaseState
{
    private float m_HitstunTime = 0.0f;
    private Vector2 m_KnockbackVelocity = Vector2.zero;
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
        m_InitialKnockbackSpeed = m_KnockbackVelocity.x;
        m_HitstunTime = 1.0f;
    }

    public override void OnEnter()
    {
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
        m_KnockbackVelocity.x = Mathf.MoveTowards(m_KnockbackVelocity.x, 0.0f, m_InitialKnockbackSpeed * Time.fixedDeltaTime);
        m_KnockbackVelocity.y += Physics2D.gravity.y * m_PlayerData.MovementData.GravityScale * Time.fixedDeltaTime;

        m_HitstunTime -= Time.fixedDeltaTime;

        m_Controller.move(m_KnockbackVelocity * Time.fixedDeltaTime);
    }
}
