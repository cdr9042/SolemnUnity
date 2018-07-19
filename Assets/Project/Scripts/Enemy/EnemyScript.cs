using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Solemn.Audio;
namespace Solemn.Enemy
{
    public class EnemyScript : MonoBehaviour
    {

        [SerializeField] private float HP = 1;
        [SerializeField] private float currentHP = 0;
        public float CurrentHP
        {
            get { return currentHP; }
            set { currentHP = (value > HP ? HP : value); }
        }
        [SerializeField] private bool canBeKnockBack = false;
        SpriteRenderer m_SpriteRenderer;
        //public int state = 0;
        private float t_state, t_flash, flashTime = .6f, knockBackTime = .3f;
        private float deathLength;
        private float deathTimer;
        public Collider2D m_Collider;
        public Transform thePlayer;

        Animator m_Anim;

        private Rigidbody2D m_Rigidbody2D;
        // Use this for initialization
        public enum State { idle, hurt, recover, dying }
        public State currentState;
        private Vector3 startPos;

        enum EnemyType { minion, boss }
        [SerializeField] EnemyType m_Type = EnemyType.minion;
        public enum ArmorType { naked, metal}
        public ArmorType m_ArmorType = ArmorType.naked;
        void Awake()
        {
            if (currentHP == 0) { currentHP = HP; }
            m_SpriteRenderer = GetComponent<SpriteRenderer>();
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
            m_Anim = GetComponent<Animator>();
            //m_Collider = GetComponent<Collider2D>();
            startPos = transform.position;
        }
        void Start()
        {
            UpdateAnimClipTimes();
        }
        void UpdateAnimClipTimes()
        {
            AnimationClip[] clips = m_Anim.runtimeAnimatorController.animationClips;
            foreach (AnimationClip clip in clips)
            {
                switch (clip.name)
                {
                    case "death":
                        deathLength = clip.length + 0.1f;
                        // Debug.Log("attack CD " + attackCD);
                        break;
                    case "hurt":
                        t_flash = clip.length;
                        break;
                        // default: Debug.Log(clip.name); break;
                }
            }
        }

        // Update is called once per frame
        void Update()
        {



            if (t_flash > 0)
            {
                t_flash -= Time.deltaTime;
                m_SpriteRenderer.material.SetFloat("_FlashAmount", t_flash / flashTime);
            }

            switch (currentState)
            {
                case State.hurt:
                    t_state -= Time.deltaTime;
                    if (t_state <= 0)
                    {
                        t_state = flashTime;

                        currentState = State.recover;
                    }
                    break;
                case State.recover:
                    t_state -= Time.deltaTime;//chấm dứt nhấp nháy
                    if (t_state <= 0)
                    {
                        m_SpriteRenderer.material.SetFloat("_FlashAmount", 0);
                        m_Anim.SetBool("isHurt", false);
                        currentState = State.idle;
                    }
                    break;
                case State.dying:
                    gameObject.layer = LayerMask.NameToLayer("Background");
                    //m_Collider.enabled = false;
                    //foreach (Collider2D c in GetComponents<Collider2D>())
                    //{
                    //    c.enabled = false;
                    //}
                    switch (m_Type)
                    {
                        case EnemyType.minion:
                            deathTimer -= Time.deltaTime;
                            if (deathTimer <= 0)
                            {
                                Destroy(gameObject);
                                return;
                            }
                            break;
                        case EnemyType.boss:
                            break;
                    }
                    
                    break;
            }

        }

        public void Damage(object[] param)
        {
            //int incomeDamage = System.Convert.ToInt32(param[0]);
            float incomeDamage = (float)param[0];
            // double direction = System.Convert.ToDouble(param[1]);
            float direction = (float)param[1];
            //Transform damageSource = param[2];
            currentHP -= incomeDamage;
            Debug.Log(transform.name + "hp left:" + currentHP);
            t_flash = flashTime;
            t_state = 0;
            if (canBeKnockBack)
            {
                t_state = knockBackTime;
                //float knockDirection = transform.position.x - thePlayer.transform.position.x;
                m_Rigidbody2D.AddForce(new Vector2(direction * 100f, m_Rigidbody2D.velocity.y));
            }

            if (currentHP <= 0)
            {
                currentState = State.dying;
                m_Anim.SetBool("Dying", true);
                deathTimer = deathLength;
            }
            else
            {
                currentState = State.hurt;
                m_Anim.SetBool("isHurt", true); //start hurt animation
            }

            //AudioManager.instance.RandomizeSfxHitSound(m_ArmorType);
        }
        public void Respawn()
        {
            currentHP = HP;
            transform.position = startPos;
        }
        void OnGUI()
        {
            // GUILayout.Label(""+ (t_state/flashTime).ToString());
        }
    }
}