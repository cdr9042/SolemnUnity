using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackTrigger : MonoBehaviour {
	public int damage = 20;
	void OnCollisionEnter2D(Collision2D collider){
		Debug.Log(collider);
	}


	void OnTriggerEnter2D(Collider2D collider) {
		// Debug.Log("collider" + collider.isTrigger);
		// Debug.Log("tag" + collider.CompareTag("Enemy"));
		if (collider.isTrigger != true && collider.CompareTag("Enemy")) {
			Debug.Log("contact enemy");
			object[] param = new object[4];
			param[0] = damage;
			param[1] = Mathf.Sign(collider.transform.position.x - transform.position.x);
			collider.SendMessageUpwards("Damage", param);
		}
	}
}
