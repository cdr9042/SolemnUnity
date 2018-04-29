using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
	
	[SerializeField] private float HP=1;
	[SerializeField] private float currentHP=1;
	public float enemyAttack = 10;
	// Use this for initialization
	void Awake() {
		currentHP = HP;
	}
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (currentHP <= 0){
			Destroy(gameObject);
		}
	}

	public void Damage (int idamage) {
		currentHP -= idamage;
		Debug.Log(currentHP);
	}
}
