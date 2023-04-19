using Prime31;
using UnityEngine;
using System.Collections.Generic;

public class PlayerStateDefaultAttackState : BaseState
{
    private uint m_CurrentFrame = 0;
    private CharacterController2D m_Controller;
    private Vector2 m_Velocity;
    private LayerMask m_HurtboxLayerMask;
    private readonly List<GameObject> m_MarkedPlayers = new List<GameObject>();
    private SpriteRenderer m_Renderer;
    private bool m_FacingDirection = false;

    private PlayerData m_PlayerData;

    public AttackData CurrentAttack;

    private void Awake()
    {
        m_Controller = GetComponent<CharacterController2D>();
        m_HurtboxLayerMask = 1 << LayerMask.NameToLayer("Hurtboxes");
        m_Renderer = GetComponentInChildren<SpriteRenderer>();
        m_PlayerData = GetComponent<PlayerData>();
    }

    public override void OnEnter()
    {
        m_CurrentFrame = 0;
        m_Velocity = m_Controller.velocity;
        m_MarkedPlayers.Clear();
        m_FacingDirection = m_Renderer.flipX;
    }

    public override void OnExit()
    {

    }

    public override void OnUpdate()
    {
        if (m_CurrentFrame == CurrentAttack.TotalFrames)
            StateManager.ChangeState(StateFactory.PreviousState);

        if (m_CurrentFrame < CurrentAttack.StartupFrames) { }
        else if (m_CurrentFrame < CurrentAttack.StartupFrames + CurrentAttack.Frames.Length)
        {
            int frame = (int)m_CurrentFrame - CurrentAttack.StartupFrames;
            FrameData currentFrame = CurrentAttack.Frames[frame];
            Vector2 scaling = new Vector2(m_FacingDirection ? -1.0f : 1.0f, 1.0f);

            List<GameObject> markedThisFrame = new List<GameObject>();

            foreach (HitboxData hitbox in currentFrame.Hitboxes)
            {
                Collider2D[] collided = null;

                switch (hitbox.Shape)
                {
                    case HitboxData.HitboxShape.Box:
                    {
                        collided = Physics2D.OverlapBoxAll((Vector2)transform.position + hitbox.Center * scaling, hitbox.Size,
                            hitbox.Rotation, m_HurtboxLayerMask);
                        break;
                    }
                    case HitboxData.HitboxShape.Circle:
                    {
                        collided = Physics2D.OverlapCircleAll((Vector2)transform.position + hitbox.Center * scaling, hitbox.Size.x,
                            m_HurtboxLayerMask);
                        break;
                    }
                }

                if (collided == null)
                    return;

                foreach (Collider2D collider in collided)
                {
                    GameObject parent = collider.transform.parent.gameObject;

                    if (!parent.TryGetComponent(out StateMachine stateMachine))
                        continue;

                    if (parent == gameObject)
                        continue;

                    if (m_MarkedPlayers.Contains(parent) || markedThisFrame.Contains(parent))
                        continue;
    
                    if (!hitbox.Multihit)
                        m_MarkedPlayers.Add(parent);

                    markedThisFrame.Add(parent);

                    stateMachine.TakeDamage(hitbox.Damage, hitbox.KnockbackDirection * scaling, hitbox.BaseKnockback, hitbox.KnockbackGrowth);
                }
            }
        }

        float gravityScale = m_PlayerData.MovementData.GravityScale;
        PlayerControlsData controlsData = m_PlayerData.PlayerControlsData;

        if (m_Velocity.y > 0.0f && !controlsData.CurrentFrame.Jump)
            gravityScale *= m_PlayerData.MovementData.LowJumpMultiplier;

        if (m_Velocity.y < 0.0f)
        {
            gravityScale *= m_PlayerData.MovementData.FallMultiplier;

            //TODO: Implement fastfalling
        }

        m_Velocity.y += Physics2D.gravity.y * Time.fixedDeltaTime * gravityScale;
        
        if (m_Controller.isGrounded)
            m_Velocity.x = Mathf.MoveTowards(m_Velocity.x, 0.0f, Time.fixedDeltaTime / m_PlayerData.MovementData.Friction);

        m_Controller.move(m_Velocity * Time.fixedDeltaTime);

        m_CurrentFrame++;
    }
}
