using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJumper : MonoBehaviour {
	public float forceY ;
	private Rigidbody2D m_Rigidbody;
	private Animator m_anim;
	// Use this for initialization
	void Awake(){
		m_Rigidbody = GetComponent<Rigidbody2D> ();
		m_anim = GetComponent<Animator> ();
	}
	void Start () {
		StartCoroutine(Jump());
	}
	IEnumerator Jump(){
		yield return new WaitForSeconds(Random.Range(2,4));
		//forceY = Random.Range(300,700);
		m_Rigidbody.AddForce(new Vector2(0,forceY));
		m_anim.SetBool("jumping",true);
		yield return new WaitForSeconds (1.5f);
		m_anim.SetBool("jumping",false);
		StartCoroutine (Jump());
	}	
	void OnTriggerEnter2D(Collider2D target){
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
