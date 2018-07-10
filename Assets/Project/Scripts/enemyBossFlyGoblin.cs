using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyBossFlyGoblin : MonoBehaviour
{
    EnemyFlyer flyScript;
    EnemyShoot shootScript;
    EnemyBullet chaseAtk;
    private Animator m_Anim;
    private Rigidbody2D m_Rigidbody2D;
    //public NavMeshAgent nav;
    public float apearLength, atkLength;
    public float Timer, stunTimer;
    // Use this for initialization
    private GameObject target;
    public float speed;
    private float _endPos;
    public int UnitsToMove;
    private bool m_GetDown = true;
    private Vector2 originPos;
    public GameObject player; //ghetto
    private GameObject[] gameObjects = new GameObject[20];
    private EnemyScript m_EnemyScript;
    public string phase = "wait";
    private void Awake()
    {
        m_Anim = GetComponent<Animator>();
        flyScript = GetComponent<EnemyFlyer>();
        shootScript = GetComponent<EnemyShoot>();
        chaseAtk = GetComponent<EnemyBullet>();
        m_EnemyScript = GetComponent<EnemyScript>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        _endPos = transform.position.y + UnitsToMove;
        originPos = transform.position;


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
                case "Apear":
                    apearLength = clip.length + 0.1f;
                    // Debug.Log("attack CD " + attackCD);
                    break;

            }
        }
    }
    void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_GetDown = !m_GetDown;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        //theScale.y *= -1;
        transform.localScale = theScale;
        //Debug.Log(theScale);
    }

    // Update is called once per frame
    void Update()
    {

        switch (phase)
        {
            case "wait":
                //GHETTO CODE, NEED FIX
                if (player == null)
                {
                    gameObjects = GameObject.FindGameObjectsWithTag("Player") as GameObject[];

                    foreach (GameObject gO in gameObjects)
                    {
                        if (gO.name.Contains("Player"))
                        {
                            player = gO;
                        }
                    }
                }
                else
                {
                    if (Vector2.Distance(transform.position, player.transform.position) <= 20f)
                    {
                        if (Timer > 0)
                        {
                            Timer -= Time.deltaTime;
                        }
                        else
                        {
                            phase = "appear";
                            Timer = apearLength;
                        }
                    }
                }

                break;
            case "appear":
                Timer -= Time.deltaTime;
                if (Timer <= 0)
                {
                    m_Anim.SetBool("Apear", true);
                    flyScript.enabled = true;
                    shootScript.enabled = true;
                    phase = "attack";
                    Timer = apearLength;

                }
                break;

            case "attack":

                if (Timer > 0)
                {
                    Timer -= Time.deltaTime;
                }
                else
                {
                    m_Anim.SetBool("P_attack", true);
                    //m_Anim.SetBool("Apear", false);
                    flyScript.speed = new Vector2(0, 0);
                    shootScript.enabled = false;
                    chaseAtk.enabled = true;
                    chaseAtk.direction.y = 1;
                    chaseAtk.speed = new Vector2(0, 10);

                    if (m_Rigidbody2D.position.y < _endPos)
                    {
                        if (target == null) target = m_EnemyScript.thePlayer;
                        transform.position = Vector2.MoveTowards(transform.position, new Vector2(target.transform.position.x, transform.position.y), speed * Time.deltaTime);
                    }
                    else
                    {
                        chaseAtk.direction.y = -1;
                        chaseAtk.speed = new Vector2(0, 20);
                        //target = null;
                        if (m_GetDown)
                        {
                            Flip();
                        }
                        stunTimer = 2f;
                        phase = "isGround";
                    }
                }

                break;

            case "isGround":

                if (stunTimer > 0)
                {
                    stunTimer -= Time.deltaTime;
                }
                else
                    phase = "stun";

                break;
            case "stun":

                m_Anim.SetBool("P_attack", false);
                m_Anim.SetBool("Stun", true);
                if (stunTimer > 0)
                    stunTimer -= Time.deltaTime;
                else
                    stunTimer = 3f;
                phase = "idle";
                break;
            case "idle":
                stunTimer -= Time.deltaTime;
                if (stunTimer <= 0)
                {
                    m_Anim.SetBool("Stun", false);
                    chaseAtk.speed = new Vector2(0, -10);
                    if (!m_GetDown)
                    {
                        Flip();

                    }
                    if (transform.position.y >= originPos.y)
                    {
                        chaseAtk.speed = new Vector2(0, 0);
                        phase = "attackShoot";
                        flyScript.enabled = false;
                    }


                    //phase ="appear";
                }
                break;
            case "attackShoot":
                flyScript.enabled = true;
                flyScript.speed = new Vector2(10, 10);
                shootScript.enabled = true;
                chaseAtk.enabled = false;
                Timer = 3f;
                phase = "attack";
                break;
        }
    }
    private void FixedUpdate()
    {


    }
}
