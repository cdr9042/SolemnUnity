using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {
	private bool attacking =false;
	private float attackTimer = 0;
	private float attackCD = 0.4f;
	public Collider2D AttackTrigger;

	private Animator anim;

	void Awake () {
		anim = gameObject.GetComponent<Animator>();
		AttackTrigger.enabled = false;
	}

	void Update(){
		if (Input.GetKey("c") && !attacking){
			attacking = true;
			attackTimer = attackCD;
			AttackTrigger.enabled = true;
		}
		if (attacking) {
			if (attackTimer > 0) {
				attackTimer -= Time.deltaTime;
			} else {
				attacking = false;
				AttackTrigger.enabled = false;
			}
		}
		anim.SetBool("Attack",attacking);
	}
}
