using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    //Other Scripts
    public PanelDisplay pd;
    //Components
    private Rigidbody2D m_rb;
    private PlayerController m_pc;
    private PlayerData m_PlayerData;
    public Animator anim;

    public int facingDir;
    public float damage;

    //Attack References
    public LayerMask HurtboxLayer;
    public bool IsAttacking;
    public bool IsFacingRight;
    public AttackData CurrentAttackData;
    public MovelistData MovelistData;
    public int AttackFrameCounter;

    public Transform spawnPoint;
    public int stocks;

    //Draw Data
    private List<AttackDrawData> m_DrawData = new List<AttackDrawData>();

    public struct MarkedForAttackData {
        public Character Character;
        public PlayerController Controller;
        public HitboxData HitboxData;
        public MarkedForAttackData(Character character, PlayerController controller, HitboxData hitboxData) {
            Character = character;
            Controller = controller;
            HitboxData = hitboxData;
        }
    }
    private class AttackDrawData
    {
        public Color DrawColor;
        public Vector2 Origin;

        public AttackDrawData(Color drawColor, Vector2 origin)
        {
            DrawColor = drawColor;
            Origin = origin;
        }

        public virtual void Draw() { }
    }
    private class BoxAttackDrawData : AttackDrawData
    {
        public Vector2 Scale;
        public float Angle;

        public BoxAttackDrawData(Color drawColor, Vector2 origin, Vector2 scale, float angle) : base(drawColor, origin)
        {
            Scale = scale;
            Angle = angle;
        }

        public override void Draw()
        {
            Vector2[] c_BoxIndices = 
            { 
                new Vector2(-0.5f, -0.5f),
                new Vector2( 0.5f, -0.5f),
                new Vector2( 0.5f,  0.5f),
                new Vector2(-0.5f,  0.5f)
            };

            Vector3 position = Origin;
            Quaternion rotation = Quaternion.Euler(0.0f, 0.0f, Angle);
            Vector3 scale = Scale;

            Matrix4x4 transformationMatrix = Matrix4x4.identity;
            transformationMatrix.SetTRS(position, rotation, scale);

            Vector2[] boxIndices = 
            {
                transformationMatrix.MultiplyPoint3x4(c_BoxIndices[0]),
                transformationMatrix.MultiplyPoint3x4(c_BoxIndices[1]),
                transformationMatrix.MultiplyPoint3x4(c_BoxIndices[2]),
                transformationMatrix.MultiplyPoint3x4(c_BoxIndices[3])
            };

            Color color = Gizmos.color;
            Gizmos.color = DrawColor;

            for (int i = 0; i < boxIndices.Length; i++)
            {
                Vector2 startPos = boxIndices[i];
                Vector2 endPos = boxIndices[(i + 1) % boxIndices.Length];

                Gizmos.DrawLine(startPos, endPos);
            }
        
            Gizmos.color = color;
        }
    }
    private class CircleAttackDrawData : AttackDrawData
    {
        public float Radius;

        public CircleAttackDrawData(Color drawColor, Vector2 origin, float radius) : base(drawColor, origin)
        {
            Radius = radius;
        }

        public override void Draw()
        {
            Color color = Gizmos.color;
            Gizmos.color = DrawColor;

            Gizmos.DrawSphere(Origin, Radius);

            Gizmos.color = color;
        }
    }
    private void OnDrawGizmos()
    {
        foreach (AttackDrawData drawData in m_DrawData)
            drawData.Draw();
    }

    void Start() {
        //Get references to components
        pd = GameObject.FindObjectOfType(typeof(PanelDisplay)) as PanelDisplay;
        anim = GetComponent<Animator>();
        m_rb = GetComponent<Rigidbody2D>();
        m_pc = GetComponent<PlayerController>();
        m_PlayerData = GetComponent<PlayerData>();
    }

    void Update() {
        if (m_PlayerData.PlayerControlsData.CurrentFrame.Attack) {
            //ALL ATTACKS
            PlayerController.PlayerState playerState = m_pc.GetCurrentState();
            if (playerState == PlayerController.PlayerState.Jump || playerState == PlayerController.PlayerState.Fall)
            {
                //Aerial Neutral
                CurrentAttackData = MovelistData.NeutralAir;
                Debug.Log("Neutral Air");
            } else
            {
                //Basic Neutral
                CurrentAttackData = MovelistData.NeutralRegular;
                Debug.Log("Neutral Regular");
            }
            Debug.Log("attack");
            StartAttack();
        }
        if (m_PlayerData.PlayerControlsData.CurrentFrame.Movement.x > 0) {
            IsFacingRight = true;
        } else if (m_PlayerData.PlayerControlsData.CurrentFrame.Movement.x < 0) {
            IsFacingRight = false;
        }
    }

    void FixedUpdate() {

        Color c_DrawColor = Color.red;

        m_DrawData.Clear();

        if (AttackFrameCounter >= CurrentAttackData.StartupFrames && AttackFrameCounter < CurrentAttackData.StartupFrames + CurrentAttackData.Frames.Length) 
        {
            Debug.Log(AttackFrameCounter-CurrentAttackData.StartupFrames);
            HitboxData[] frameHitboxes = CurrentAttackData.Frames[AttackFrameCounter-CurrentAttackData.StartupFrames].Hitboxes;
            List<MarkedForAttackData> markedForAttack = new List<MarkedForAttackData>();
            foreach (HitboxData hitbox in frameHitboxes) {
                Vector2 origin = (IsFacingRight)?((Vector2)transform.position + hitbox.Center):((Vector2)transform.position + hitbox.Center*Vector2.left);
                Vector2 size = hitbox.Size;
                float angle = hitbox.Rotation;
                //Draw gizmo
                m_DrawData.Add(new BoxAttackDrawData(c_DrawColor, origin, size, angle));
                switch (hitbox.Shape) {
                    case HitboxData.HitboxShape.Box:
                    {
                        RaycastHit2D[] hits = Physics2D.BoxCastAll(origin, size, angle, Vector2.zero, 1.0f, HurtboxLayer);
                        
                        if (hits.Length == 0) {
                            break;
                        }

                        foreach (RaycastHit2D hit in hits) {
                            Character target =  hit.collider.gameObject.transform.parent.gameObject.GetComponent<Character>();
                            PlayerController controller = hit.collider.transform.parent.GetComponent<PlayerController>();
                            if (target != GetComponent<Character>() && !TargetAlreadyMarked(markedForAttack, target)) {
                                markedForAttack.Add(new MarkedForAttackData(target, controller, hitbox));
                            }
                            break;
                        }
                        break;
                    }
                    case HitboxData.HitboxShape.Circle:
                    {
                        m_DrawData.Add(new CircleAttackDrawData(c_DrawColor, origin, size.x));
                        RaycastHit2D[] hits = Physics2D.CircleCastAll(origin, size.x, Vector2.zero, 1.0f, HurtboxLayer);

                        if (hits.Length == 0) {
                            break;
                        }

                        foreach (RaycastHit2D hit in hits) {
                            Character target =  hit.collider.gameObject.transform.parent.gameObject.GetComponent<Character>();
                            PlayerController controller = hit.collider.transform.parent.GetComponent<PlayerController>();
                            if (target != GetComponent<Character>() && !TargetAlreadyMarked(markedForAttack, target)) {
                                markedForAttack.Add(new MarkedForAttackData(target, controller, hitbox));
                            }
                            break;
                        }
                        break;
                    }
                }
            }
            foreach (MarkedForAttackData target in markedForAttack) {
                target.Character.TakeDamage(target.HitboxData.Damage);

                Vector2 knockbackDirection = target.HitboxData.KnockbackDirection;
                float baseKnockback = target.HitboxData.BaseKnockback;
                float knockbackGrowth = target.HitboxData.KnockbackGrowth;

                Vector2 vel = knockbackDirection * (baseKnockback + knockbackGrowth * target.Character.damage);
                target.Controller.CurrentKnockbackSpeed = new Vector2((IsFacingRight ? 1 : -1) * vel.x, vel.y);
                target.Controller.OnStateChange(PlayerController.PlayerState.Knockback);
            }
        }
        if (AttackFrameCounter <= CurrentAttackData.TotalFrames && IsAttacking) {
            AttackFrameCounter++;
        }
    }
    bool TargetAlreadyMarked(List<MarkedForAttackData> data, Character potentialTarget) {
        foreach (MarkedForAttackData target in data) {
            if (potentialTarget == target.Character) {
                return true;
            }
        }
        return false;
    }
    

    void OnTriggerExit2D(Collider2D col) {
        if (col.gameObject.layer == LayerMask.NameToLayer("BlastZone")) {
            pd.DeleteStock();
            stocks--;
            if (stocks > 0) {
                transform.position = spawnPoint.position;
                m_rb.velocity = Vector2.zero;
                Debug.Log("Stocks="+stocks);
            }
            damage=0;
        }
        
    }

    public void TakeDamage(float d) {
        damage += d;
    }
    public void TakeKnockback(Vector2 knockbackDirection, float baseKnockback, float knockbackGrowth, float facingDir) {
        Vector2 vel = knockbackDirection*(baseKnockback + knockbackGrowth*damage);
        m_rb.velocity = new Vector2(facingDir*vel.x, vel.y);
    }

    public void StartAttack() {
        IsAttacking = true;
        AttackFrameCounter = 0;
    }

    void OnTriggerEnter2D(Collider2D col) {
        //Check to see if collider is disabled
        if (!col.isActiveAndEnabled || !col.isTrigger) {
            return;
        }
        //Check if trigger belongs to player layer
        if (col.gameObject.layer == LayerMask.NameToLayer("Hitboxes")) {
            //Get owner of the hitbox
            Character attacker = col.gameObject.transform.parent.gameObject.GetComponent<Character>();
        }
    }
    
}
