using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Solemn.Enemy;

public class FlyerPatrol : MonoBehaviour
{
    [SerializeField] private Transform[] patrolPoints;
    private int gotoPoint = 0;
    [SerializeField] private float m_FlyForce = 400f;
    //[SerializeField] private float flapInterval = 0.5f;
    [SerializeField] private float hSpeed = 3f;
    private enum ObjType { enemy,platform }
    [SerializeField] ObjType objType = ObjType.enemy;
    private Rigidbody2D m_Rigidbody;
    private Vector3 setVelocity;
    private bool canFly = true;
    private EnemyScript eScript;
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        if (objType == ObjType.enemy) eScript = GetComponent<EnemyScript>();
    }

    void Update()
    {
        if (canFly)
        {
            Vector3 directionMove = patrolPoints[gotoPoint].position - transform.position;
            //.DrawLine(patrolPoints[gotoPoint].position, transform.position);
            float distanceLeft = Vector3.Magnitude(directionMove);
            if (distanceLeft <= 1f)
            {
                gotoPoint++;
                if (gotoPoint == patrolPoints.Length)
                {
                    gotoPoint = 0;
                }
            }

            //xoay sprite
            if (directionMove.x != 0 && objType != ObjType.platform)
                transform.localScale = new Vector3(Mathf.Sign(-directionMove.x), 1);

            Vector3 targetVelocity = directionMove.normalized * hSpeed;
            setVelocity = Vector3.Lerp(m_Rigidbody.velocity, targetVelocity, hSpeed / 3 * Time.deltaTime);
            m_Rigidbody.velocity = setVelocity;
        }
        if (objType == ObjType.enemy) if (eScript.currentState == EnemyScript.State.dying)
        {
            StopFly();
        }
    }

    public void StopFly()
    {
        canFly = false;
        m_Rigidbody.gravityScale = 1;
    }

    public void FlapWing()
    {
        m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, m_FlyForce);
        //yield return new WaitForSeconds(flapInterval);
        //StartCoroutine(FlapWing());
    }
}
