using System;
using UnityEngine;
using System.Collections;


namespace UnityStandardAssets._2D
{
    public class PlayerScript : MonoBehaviour
    {
        [SerializeField] private float m_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
        [SerializeField] private float m_JumpForce = 700f;                  // Amount of force added when the player jumps.
        [SerializeField] private float m_wallJumpXForce = 600f;
        [SerializeField] private float m_wallJumpYForce = 600f;
        [Range(0, 1)] private float m_CrouchSpeed = .36f;  // Amount of maxSpeed applied to crouching movement. 1 = 100%
        private bool m_AirControl = true;                 // Whether or not a player can steer while jumping;
        [SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character
        [SerializeField] private LayerMask m_WhatIsWall;
        [SerializeField] private LayerMask m_WhatIsEnemy;                  // A mask determining what is enemy to the character

        [SerializeField] private float m_ProtectTime = 2f;      //Thời gian bảo vệ sau khi mất máu ( nhấp nháy )
        [SerializeField] private float m_StaggerTime = 0.7f;    //Thời gian bị mất điều khiển khi mất máu
        [SerializeField] private float m_KnockBack = 2.2f;      //Khoảng cách bị bay ra khi mất máu

        [SerializeField] private float m_AirJump = 1;           //Số lần nhảy trên không
        [SerializeField] private bool canWallJump = false;

        public float m_HealthMax = 100f;      //Máu tối đa người chơi
        public float m_HealthLeft;                    // The fastest the player can travel in the x axis.
        private float m_AirJumpLeft = 0f;                   // //Số lần nhảy trên không hiện tại

        private Collider2D m_Collider;
        private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
        private Vector2 k_GroundedRadius; // Radius of the overlap circle to determine if grounded
        private Transform m_WallCheck;    // A position marking where to check if the player is facing wall.
        private Vector2 k_WallCheckRadius;
        private bool m_Grounded;            // Whether or not the player is grounded.
        private bool m_OnWall;              // Check if on wall
        private bool m_OnPlatform;          // Kiểm tra có đang đứng trên platform không
        private Transform m_CeilingCheck;   // A position marking where to check for ceilings
        const float k_CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up

        private Animator m_Anim;            // Reference to the player's animator component.
        private Rigidbody2D m_Rigidbody2D;
        private float m_RigidbodyOldGravity;

        private float m_Move;

        private bool m_FacingRight = true;  // For determining which way the player is currently facing.
        private bool m_LockFacing = false;

        private bool disableControl = false;  //nếu đang taking damage = true thì không thể điều khiển
        private float t_currentState = 0f;
        public enum State { normal, lostControl, invicible, die, revive}
        public State state = State.normal;              //trạng thái của người chơi
        private string mylog = "";

        private int attackMode = 0;
        // private bool airAttack;
        private float attackTimer = 0;
        private float attackCD = 0.5f;
        private float airAttackCD = 0.5f;
        private float[] clipLength = new float[10];
        public Collider2D AttackTrigger;

        SpriteRenderer m_SpriteRenderer;

        GameObject gameOverUI;

        public Transform lastCheckpoint;

        private void Awake()
        {
            // Setting up references.
            m_GroundCheck = transform.Find("GroundCheck");
            m_CeilingCheck = transform.Find("CeilingCheck");
            m_WallCheck = transform.Find("WallCheck");
            k_WallCheckRadius = m_WallCheck.GetComponent<BoxCollider2D>().size;
            m_Anim = GetComponent<Animator>();
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
            m_RigidbodyOldGravity = m_Rigidbody2D.gravityScale;
            m_Collider = GetComponent<Collider2D>();
            k_GroundedRadius = m_GroundCheck.GetComponent<BoxCollider2D>().size;

            m_HealthLeft = m_HealthMax;
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("EnemyLayer"), gameObject.layer, false);
            m_AirJumpLeft = m_AirJump;

            AttackTrigger.enabled = false;
            UpdateAnimClipTimes();
            m_SpriteRenderer = GetComponent<SpriteRenderer>();

            gameOverUI = GameObject.Find("GameOverUI");
        }

        public void UpdateAnimClipTimes()
        {
            AnimationClip[] clips = m_Anim.runtimeAnimatorController.animationClips;
            foreach (AnimationClip clip in clips)
            {
                switch (clip.name)
                {
                    case "sogetsu_attack":
                        attackCD = clip.length + 0.1f;
                        // .Log("attack CD " + attackCD);
                        break;
                    case "sogetsu_jump_attack":
                        airAttackCD = clip.length + 0.1f;
                        // .Log("attack CD " + attackCD);
                        break;
                    // default: .Log(clip.name); break;
                    case "sogetsu_die":
                        clipLength[4] = clip.length;
                        break;
                    case "revive":
                        clipLength[5] = clip.length;
                        break;
                }
            }
        }


        private void FixedUpdate()
        {
            // .Log(transform.parent);
            if (transform.parent.parent == null) //check if on moving platform
                m_Grounded = false;
            m_OnWall = false;
            // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
            // This can be done using layers instead but Sample Assets will not overwrite your project settings.
            // Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
            Collider2D[] colliders = Physics2D.OverlapBoxAll(m_GroundCheck.position, k_GroundedRadius, 0, m_WhatIsGround);

            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                {
                    m_Grounded = true;
                    if (colliders[i].CompareTag("MovingPlatform"))
                    {
                        // if (colliders[i].transform.Find("Collider") != null)
                        //     transform.position = new Vector2(transform.position.x,colliders[i].transform.Find("Collider").transform.position.y);
                        // else {
                        //     .Log("platform doesn't have collider");
                        // }
                        if (m_GroundCheck.position.y >= colliders[i].transform.position.y)
                        {
                            transform.parent.parent = colliders[i].transform;
                        }
                        // else { m_Grounded = false; }
                        // .Log(colliders[i].bounds.ClosestPoint(transform.position));

                        // .Break();
                        // .Log(colliders[i].transform.position);
                    }
                }
            }
            m_Anim.SetBool("Ground", m_Grounded);

            if (canWallJump)
                m_OnWall = Physics2D.OverlapBox(m_WallCheck.position, k_WallCheckRadius, 0, m_WhatIsWall);

            if (m_Grounded)
            {
                m_AirJumpLeft = m_AirJump;
            }
            // colliders = Physics2D.OverlapBoxAll(transform.position, k_GroundedRadius, m_WhatIsEnemy);

            // Set the vertical animation
            m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);
            //Hurt
            if (state == State.lostControl || state == State.invicible)
            {
                if (Math.Round(t_currentState * 100) % 8 == 0)
                    m_SpriteRenderer.material.SetFloat("_FlashAmount", .8f);
                if ((Math.Round(t_currentState * 100) + 4) % 8 == 0)
                    m_SpriteRenderer.material.SetFloat("_FlashAmount", 0);
            }
            switch (state)
            {
                case State.lostControl:
                    t_currentState += Time.deltaTime;
                    if (Mathf.Abs(m_Rigidbody2D.velocity.x) > 0.1f)
                    {
                        m_Rigidbody2D.AddForce(new Vector2(-Mathf.Sign(m_Rigidbody2D.velocity.x) * 3, 0));
                    };
                    if (t_currentState >= m_StaggerTime)
                    { //stop staggering
                        disableControl = false;
                        m_Anim.SetBool("isHurt", false);
                        t_currentState = 0;
                        state = State.invicible;
                    }
                    break;
                case State.invicible:
                    t_currentState += Time.deltaTime;
                    if (t_currentState >= m_ProtectTime)
                    {
                        t_currentState = 0;
                        m_SpriteRenderer.material.SetFloat("_FlashAmount", 0);
                        //Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("EnemyLayer"), gameObject.layer, false);
                        //gameObject.layer = LayerMask.NameToLayer("PlayerLayer"); //playerLayer
                        state = 0;
                    }
                    break;
                case State.die:
                    t_currentState += Time.deltaTime;
                    m_Rigidbody2D.velocity = new Vector2(Mathf.Lerp(m_Rigidbody2D.velocity.x, 0, 4f * Time.deltaTime), m_Rigidbody2D.velocity.y);
                    if (t_currentState >= clipLength[4] + 1f)
                    {
                        state = State.revive;
                        t_currentState = 0;
                        Debug.Log("done dying, now revive");
                        // Debug.Break();
                        m_Rigidbody2D.velocity = new Vector2(0, 0);
                    }
                    break;
                case State.revive:
                    if (!m_Anim.GetBool("isReviving"))
                    {
                        GameObject lastCheckPoint;
                        Debug.Log(GameData.current._Progress.checkPoint);
                        if (GameData.current._Progress.checkPoint != null && GameData.current._Progress.checkPoint != "")
                        {
                            lastCheckPoint = GameObject.Find(GameData.current._Progress.checkPoint);
                            Debug.Log("respawn to check point");
                        }
                        else
                        {
                            lastCheckPoint = GameObject.Find("SpawnPoint");
                            Debug.Log("respawn to spawn point");
                        }
                        m_Anim.SetBool("Fall", false);
                        m_Anim.SetBool("isReviving", true);
                        transform.position = lastCheckPoint.transform.position;
                        m_Rigidbody2D.gravityScale = 0;
                        disableControl = true;
                    }
                    else
                    {
                        t_currentState += Time.deltaTime;
                        if (t_currentState >= clipLength[5])
                        {
                            t_currentState = 0;
                            m_Anim.SetBool("isReviving", false);
                            state = State.invicible;
                            disableControl = false;
                            m_HealthLeft = m_HealthMax;
                            m_Rigidbody2D.gravityScale = m_RigidbodyOldGravity;

                            try {
                                //GameObject.Find("DataMaster").GetComponent<GameInit>().enemyManagerPrefab.GetComponent<EnemyMasterScript>().resetSpawner();
                                GameData.current.TriggerEvent(GameData.GameEvent.playerDie);
                            }
                            catch (Exception e)
                            {
                                Debug.Log("Can't find DataMaster, failed to reset spawner");
                            }

                        }
                    }

                    break;
                default: break;
            }

            UpdateAttack();
            if (transform.position.y < -1000 && state != State.revive)
            {
                state = State.die;
            }
        }

        public void setMove(float move)
        {
            m_Move = move;
        }
        public void Move(float move, bool crouch, bool jump)
        {
            // If crouching, check to see if the character can stand up
            if (!crouch && m_Anim.GetBool("Crouch"))
            {
                // If the character has a ceiling preventing them from standing up, keep them crouching
                if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
                {
                    crouch = true;
                }
            }

            // Set whether or not the character is crouching in the animator
            m_Anim.SetBool("Crouch", crouch);

            // If the player should jump...
            if (jump)
                if (m_Grounded && m_Anim.GetBool("Ground"))
                {
                    // Add a vertical force to the player.
                    m_Grounded = false;
                    m_Anim.SetBool("Ground", false);
                    m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
                }
                else if (m_AirJumpLeft > 0 || m_OnWall)
                {
                    Vector2 v = m_Rigidbody2D.velocity;
                    v.y = m_wallJumpYForce / m_Rigidbody2D.mass * Time.fixedDeltaTime;
                    //v.y = m_JumpForce * .8f / m_Rigidbody2D.mass * Time.fixedDeltaTime;
                    if (m_OnWall)
                    {
                        //bật tường sẽ đẩy người chơi lùi lại
                        float WJdirection = m_FacingRight ? -1 : 1;
                        v.x = m_wallJumpXForce / m_Rigidbody2D.mass * Time.fixedDeltaTime * WJdirection;
                        m_AirJumpLeft = m_AirJump;
                        setMove(v.x);
                        Flip();
                        StartCoroutine(LockFacingFor(0.5f));
                    }
                    m_Rigidbody2D.velocity = v;

                    if (!m_OnWall) m_AirJumpLeft--;
                    // m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce*1.1f));
                }

            //only control the player if grounded or airControl is turned on
            if (!disableControl && attackMode != 2)
            { //nếu đang nhận thiệt hại thì ko thể điều khiển
                if (m_Grounded || m_AirControl)
                {
                    // Reduce the speed if crouching by the crouchSpeed multiplier
                    // move = (crouch ? move*m_CrouchSpeed : move);

                    // The Speed animator parameter is set to the absolute value of the horizontal input.
                    m_Anim.SetFloat("Speed", Mathf.Abs(m_Move));

                    // Move the character
                    float velocityx_modifier = m_Grounded ? 8f : 3f; //hạn chế di chuyển trên không
                    float targetVelocityX = m_Move * m_MaxSpeed;
                    if (attackMode == 1)
                    {
                        targetVelocityX = 0;
                    }
                    float velocityx = Mathf.Lerp(m_Rigidbody2D.velocity.x, targetVelocityX, velocityx_modifier * Time.deltaTime);
                    m_Rigidbody2D.velocity = new Vector2(velocityx, m_Rigidbody2D.velocity.y);
                    if (!m_LockFacing && attackMode == 0) // If the input is moving the player right and the player is facing left...
                    {
                        Flip();
                    }
                }
            }
            // mylog = m_Grounded.ToString();
        }
        private void Flip()
        {
            Vector3 theScale = transform.localScale;
            if (m_Move > 0)
            {
                m_FacingRight = true;
                theScale.x = 1;
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (m_Move < 0)
            {
                m_FacingRight = false;
                theScale.x = -1;
            }

            // Multiply the player's x local scale by -1.
            transform.localScale = theScale;
        }

        IEnumerator LockFacingFor(float seconds)
        {
            m_LockFacing = true;
            yield return new WaitForSeconds(seconds);
            m_LockFacing = false;
        }


        public void Attack(bool attack)
        {
            if (attack && attackMode == 0)
            {
                if (m_Grounded)
                {
                    setMove(0f);
                    attackMode = 1;
                    attackTimer = attackCD;
                    // m_Rigidbody2D.velocity = new Vector2(0, m_Rigidbody2D.velocity.y);
                }
                else
                {
                    attackMode = 2;
                    attackTimer = airAttackCD;
                }
            }
        }

        private void UpdateAttack()
        {
            if (attackMode != 0)
            {
                if (attackTimer > 0)
                {
                    switch (attackMode)
                    {
                        case 2:
                            { //air attack
                                if (attackTimer < airAttackCD - 0.1f)
                                {
                                    AttackTrigger.enabled = true;
                                }
                                if (m_Grounded) attackTimer = 0;
                                break;
                            }
                        case 1: //ground attack
                            if (attackTimer < attackCD - 0.1f)
                            {
                                AttackTrigger.enabled = true;
                            }
                            if (!m_Grounded) attackTimer = 0;
                            break;
                    }
                    attackTimer -= Time.deltaTime;
                }
                else
                {
                    attackMode = 0;
                    AttackTrigger.enabled = false;
                }
            }
            m_Anim.SetInteger("Attack", attackMode);
        }

        void OnCollisionEnter2D(Collision2D collider)
        {

            if (collider.gameObject.tag == "Enemy")// LayerMask.NameToLayer("EnemyLayer"))
            {
                EnemyStats enemy = (collider.gameObject.GetComponent<EnemyStats>());
                if (enemy != null)
                {
                    TakeDamage(enemy.enemyAttack);
                }
                else
                {
                    Debug.Log("Script 'Enemy' not present in " + collider.gameObject);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("BulletLayer"))
            {
                EnemyStats enemy = (collision.gameObject.GetComponent<EnemyStats>());
                if (enemy != null)
                {
                    TakeDamage(enemy.enemyAttack);
                }
                else
                {
                    Debug.Log("Script 'Enemy' not present in " + collision.GetComponent<Collider>().gameObject);
                }
            }
        }

        void OnTriggerStay2D(Collider2D collider)
        {
            if (collider.gameObject.tag == "Spike")
            {
                EnemyStats enemy = (collider.gameObject.GetComponent<EnemyStats>());
                if (enemy != null)
                {
                    TakeDamage(enemy.enemyAttack);
                }
                else
                {
                    Debug.Log("Script 'Enemy' not present in " + collider.gameObject);
                }
            }
            else if (collider.tag == "Checkpoint")
            {
                lastCheckpoint = collider.transform;
            }
        }

        void OnCollisionExit2D(Collision2D col)
        {
            if (col.collider.CompareTag("MovingPlatform"))
            {
                transform.parent.parent = null;
            }
        }

        private void TakeDamage(float dmg)
        {
            if (state == State.normal)
            {
                m_HealthLeft -= dmg;
                Debug.Log("Took " + dmg + "damage! Health left: " + m_HealthLeft);
                //Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("EnemyLayer"), gameObject.layer);
                //gameObject.layer = LayerMask.NameToLayer("PlayerInvicible"); //playerInvicible layer
                disableControl = true;
                float flyDirection;

                flyDirection = (m_FacingRight) ? -1f : 1f;
                m_Rigidbody2D.velocity = new Vector2(flyDirection * m_KnockBack, 1.5f); //m_Rigidbody2D.velocity.y
                if (m_HealthLeft > 0)
                {
                    t_currentState = 0;
                    state = State.lostControl;
                    m_Anim.SetBool("isHurt", true);
                }
                else
                {
                    t_currentState = 0;
                    state = State.die;
                    m_Anim.SetBool("Fall", true);
                    // gameOverUI.GetComponent<GameOverUIScript>().active();
                    //gameOverUI.SetActive(true);
                }
            }
        }

        // private IEnumerator Flasher() 
        // {

        // }
        private void Death()
        {

        }
        void OnGUI()
        {
            GUILayout.Label(""
            // "!disableControl && !attacking" + (!disableControl && attackMode==0).ToString() + "\n" +
            // "speed " + m_Rigidbody2D.velocity + "\n" +
            // m_Anim.GetInteger("Attack")
            // "attacking " +attackMode.ToString()+"\n"+
            // "ground "+m_Grounded.ToString()
            // state.ToString()
            // +"\ntime current state" + t_currentState.ToString()
            // +"\n" + disableControl.ToString()
            // +"\n" + t_currentState.ToString()
            );
        }
    }
}