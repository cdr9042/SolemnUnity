using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyAtk : MonoBehaviour {
	 public GameObject target;
    public float attackTimer;
    public float coolDown;

// Use this for initialization

    void Start(){
        attackTimer = 0;
        coolDown = 2.0f;
    }

// Update is called once per frame

    void Update(){
        if (attackTimer > 0)
            attackTimer -= Time.deltaTime;
        
        if (attackTimer < 0)
            attackTimer = 0;
        
        if (attackTimer == 0){
            Attack();
            attackTimer = coolDown;
        }
        

    }

    private void Attack(){

        float distance = Vector3.Distance(target.transform.position, transform.position);
        
        Vector3 dir = (target.transform.position - transform.position).normalized;
        
        float direction = Vector3.Dot(dir, transform.forward);

        Debug.Log (direction);

        if (distance < 2.5f) {
            if(direction > 0){
            Debug.Log("a");

                
            }
        }

    }
    

}
