using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour {
	
	[SerializeField] private float HP=1;
	[SerializeField] private float currentHP=1;
	[SerializeField] private bool canBeKnockBack = false;
	public float enemyAttack = 10;
	SpriteRenderer m_SpriteRenderer;
	public int state = 0;
	private float t_state, t_flash, flashTime = .6f, knockBackTime = .3f;

	private Rigidbody2D m_Rigidbody2D;
	// Use this for initialization
	void Awake() {
		currentHP = HP;
		m_SpriteRenderer = GetComponent<SpriteRenderer>();
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
	}
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (currentHP <= 0){
			Destroy(gameObject);
		}

		if (t_flash > 0) {
			t_flash -= Time.deltaTime;
			m_SpriteRenderer.material.SetFloat("_FlashAmount",t_flash/flashTime);
		}

		switch (state) {
			case 1: 
				t_state -= Time.deltaTime;
				if (t_state <= 0) {
					t_state = flashTime;
					state = 2;
				}
			break;
			case 2:
				t_state -= Time.deltaTime;
				if (t_state <= 0) {
					m_SpriteRenderer.material.SetFloat("_FlashAmount",0);
					state = 0;
				}
			break;
		}
		
	}

	public void Damage (object[] param) {
		int incomeDamage = System.Convert.ToInt32(param[0]);
		// double direction = System.Convert.ToDouble(param[1]);
		float direction = (float) param[1];
		currentHP -= incomeDamage;
		Debug.Log(transform.name +"hp left:"+ currentHP);
		t_flash = flashTime;
		t_state = 0;
		state = 1;
		if (canBeKnockBack) {
			t_state = knockBackTime;
			GameObject player = GameObject.Find("Player");
			float knockDirection = transform.position.x - player.transform.position.x;
			m_Rigidbody2D.velocity = new Vector2(direction*3f, m_Rigidbody2D.velocity.y);
		}
	}

	void OnGUI() {
            // GUILayout.Label(""+ (t_state/flashTime).ToString());
	}
}
