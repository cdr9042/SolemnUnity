using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour {
	//public bool isEnemyShot = false;
	//private float damage;
	
	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.name == "Player"){
			//EnemyStats enemy = GetComponent<EnemyStats>();
               // enemy.enemyAttack 
			Destroy(this.gameObject);
		}
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
