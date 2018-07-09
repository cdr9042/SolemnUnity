using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextStageCheck : MonoBehaviour {
	public bool PlayerEnter;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnTriggerEnter2D (Collider2D collider) {
		PlayerEnter = true;
	}
}
