using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyAtks : MonoBehaviour
{
    public Transform target;
    public float attackTimer;
    public float coolDown;
    float atkLength;
    private Animator m_Anim;
    public string phase = "cooldown";
    public bool can_attack;

    // Use this for initialization
    private void Awake()
    {
        m_Anim = GetComponent<Animator>();
        // Debug.Log (m_Anim);
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
                case "attack":
                    atkLength = clip.length + 0.1f;
                    // Debug.Log("attack CD " + attackCD);
                    break;
                case "hurt":

                    break;
                    // default: Debug.Log(clip.name); break;
            }
        }
    }

    // Update is called once per frame

    void Update()
    {
        float distance = 0;
        if (target != null)
            distance = Vector3.Distance(target.transform.position, transform.position);
        else
        {
            target = GameData.current.players[0];
        }

        Vector3 dir = (target.transform.position - transform.position).normalized;


        can_attack = (distance < 5f);
        if (gameObject.activeSelf)
        {
            if (can_attack)
            {
                switch (phase)
                {
                    case "cooldown":
                        if (attackTimer > 0)
                        {
                            attackTimer -= Time.deltaTime;
                        }
                        else
                        {
                            phase = "attack";
                            attackTimer = atkLength;
                            // Debug.Log(atkLength);
                        }
                        break;
                    case "attack":
                        {
                            m_Anim.SetBool("P_attack", true);
                            // Debug.Log("player connect");
                            attackTimer -= Time.deltaTime;

                            if (attackTimer <= 0)
                            {
                                m_Anim.SetBool("P_attack", false);
                                attackTimer = coolDown;
                                // Debug.Log("COOL");
                                phase = "cooldown";
                            }
                            break;
                        }
                }
            }
            else
            {
                m_Anim.SetBool("P_attack", false);
            }
        }
    }


    void OnGUI()
    {
        // GUILayout.Label(Vector3.Distance(target.transform.position, transform.position).ToString()
        // );
    }

}
