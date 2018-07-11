using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{

    [SerializeField] private float HP = 1;
    [SerializeField] private float currentHP = 1;
    [SerializeField] private bool canBeKnockBack = false;
    SpriteRenderer m_SpriteRenderer;
    public int state = 0;
    private float t_state, t_flash, flashTime = .6f, knockBackTime = .3f;
    private float deathLength;
    private float deathTimer;
    string phase = "dying";
    public Transform thePlayer;

    Animator m_Anim;

    private Rigidbody2D m_Rigidbody2D;
    // Use this for initialization
    void Awake()
    {
        currentHP = HP;
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_Anim = GetComponent<Animator>();
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

        switch (state)
        {
            case 1:
                t_state -= Time.deltaTime;
                if (t_state <= 0)
                {
                    t_state = flashTime;

                    state = 2;
                }
                break;
            case 2:
                t_state -= Time.deltaTime;//chấm dứt nhấp nháy
                if (t_state <= 0)
                {
                    m_SpriteRenderer.material.SetFloat("_FlashAmount", 0);
                    state = 3;
                }
                break;
            case 3:
                m_Anim.SetBool("isHurt", false);
                state = 0;
                break;
            case 4:
                deathTimer -= Time.deltaTime;
                if (deathTimer <= 0)
                {
                    Destroy(gameObject);
                    return;
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
        currentHP -= incomeDamage;
        Debug.Log(transform.name + "hp left:" + currentHP);
        t_flash = flashTime;
        t_state = 0;
        if (canBeKnockBack)
        {
            t_state = knockBackTime;
            float knockDirection = transform.position.x - thePlayer.transform.position.x;
            m_Rigidbody2D.velocity = new Vector2(direction * 3f, m_Rigidbody2D.velocity.y);
        }

        if (currentHP <= 0)
        {
            state = 4;
            m_Anim.SetBool("Dying", true);
            deathTimer = deathLength;
            phase = "disappear";


        }
        else
        {
            state = 1;
            m_Anim.SetBool("isHurt", true); //start hurt animation
        }
    }

    void OnGUI()
    {
        // GUILayout.Label(""+ (t_state/flashTime).ToString());
    }
}
