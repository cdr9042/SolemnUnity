using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof (EnemyScript))]
public class EnemyWalker : MonoBehaviour {
	[SerializeField] private float m_MaxSpeed = 10f;
	private bool m_Grounded; 
	private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private Animator m_Anim;            // Reference to the player's animator component.
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = false;  // For determining which way the player is currently facing.
	[SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character
	float direction, aimDirection;
	GameObject target;
	private float turnTime = 1f, turnTimeCD, m_Velocity;
	private int walkState;
	// Use this for initialization
	public EnemyScript m_Self;
	private void Awake()
        {
            // Setting up references.
            m_GroundCheck = transform.Find("GroundCheck");
            // m_CeilingCheck = transform.Find("CeilingCheck");
            m_Anim = GetComponent<Animator>();
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
			target = GameObject.Find("Player");
			turnTimeCD = turnTime;
			if (target != null) {
				aimDirection = Mathf.Sign(target.transform.position.x - transform.position.x);
			} else {
				Debug.Log("Can't find Player");
			}
			direction = aimDirection;
			m_Velocity = m_MaxSpeed;
			m_Self = GetComponent<EnemyScript>();
        }
		
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		// Debug.Log(m_Self.walkState);
		if (target != null) {
			aimDirection = Mathf.Sign(target.transform.position.x - transform.position.x);
		} else {target = GameObject.Find("Player");}
		switch (walkState){
			case 0: //accelerate
				if (m_Velocity < m_MaxSpeed) {
					m_Velocity += 0.05f;
				}
				if (aimDirection != direction) {
					walkState = 1;
				}
			break;
			case 1: //turn
				if (m_Velocity > 0) {
					m_Velocity -= 0.02f;
				}
				else {
					direction = aimDirection;
					walkState = 0;
				}
			break;
		}

		if (m_Self.state != 1)
			Move(m_Velocity * direction);
	}

	
	private void FixedUpdate() {
		m_Grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
				m_Grounded = true;
		}
		m_Anim.SetBool("Ground", m_Grounded);
		if (m_Rigidbody2D.velocity.magnitude == 0) {
			m_Velocity = 0;
		}
	}
	public void Move(float move)
	{
		
		//only control the player if grounded or airControl is turned on
		if (m_Grounded)
		{
			// The Speed animator parameter is set to the absolute value of the horizontal input.
			m_Anim.SetFloat("Speed", Mathf.Abs(move));
			// Move the character
			m_Rigidbody2D.velocity = new Vector2(move, m_Rigidbody2D.velocity.y);
			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
				// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
		}
		// If the player should jump...
		// Debug.Log(m_Grounded);
		// Debug.Log(m_Anim.GetBool("Ground"));
		// if (m_Grounded && jump && m_Anim.GetBool("Ground"))
		// {
		//     // Add a vertical force to the player.
		//     m_Grounded = false;
		//     m_Anim.SetBool("Ground", false);
		//     m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
		// }
	}

	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
		
	}

	// IEnumerator FindPlayer(){
		
	// 	target = GameObject.Find("Player");
	// 	yield return new WaitForSeconds(5);
	// }

	void OnGUI() {
		GUILayout.Label( m_Self.state.ToString()
			// m_Rigidbody2D.velocity.magnitude.ToString()
			// direction.ToString()+"\n"+
			// aimDirection.ToString()+"\n"+
			// walkState
		);
	}
}
