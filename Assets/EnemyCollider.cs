using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollider : MonoBehaviour {

    private void Update()
    {
        
            

    }
    void OnTriggerEnter2D(Collider2D col)
    {
        HealBarScript.health -= 40f;
    }
}
