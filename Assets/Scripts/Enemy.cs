using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
	
	public float HP=1;
	public float currentHP=1;
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
