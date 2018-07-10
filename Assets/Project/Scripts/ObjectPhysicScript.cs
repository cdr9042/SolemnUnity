using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPhysicScript : MonoBehaviour {

    [SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character
    [SerializeField] private LayerMask m_WhatIsWall;

    private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
    private Vector2 k_GroundedRadius; // Radius of the overlap circle to determine if grounded
    private Transform m_WallCheck;    // A position marking where to check if the player is facing wall.
    private Vector2 k_WallCheckRadius;
    private bool m_Grounded;            // Whether or not the player is grounded.
    private bool m_OnWall;              // Check if on wall
    private bool m_OnPlatform;          // Kiểm tra có đang đứng trên platform không
    private Transform m_CeilingCheck;   // A position marking where to check for ceilings
    const float k_CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
    private Collider2D m_Collider;
    private Animator m_Anim;            // Reference to the player's animator component.
    private Rigidbody2D m_Rigidbody2D;
    private float m_RigidbodyOldGravity;


    // Use this for initialization
    void Start () {
        m_GroundCheck = transform.Find("GroundCheck");
        m_CeilingCheck = transform.Find("CeilingCheck");
        m_WallCheck = transform.Find("WallCheck");
        k_WallCheckRadius = m_WallCheck.GetComponent<BoxCollider2D>().size;
        m_Anim = GetComponent<Animator>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_RigidbodyOldGravity = m_Rigidbody2D.gravityScale;
        m_Collider = GetComponent<Collider2D>();
        k_GroundedRadius = m_GroundCheck.GetComponent<BoxCollider2D>().size;
    }
	
	void FixedUpdate() {
        if (transform.parent == null) //check if on moving platform
            m_Grounded = false;
        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapBoxAll(m_GroundCheck.position, k_GroundedRadius, 0, m_WhatIsGround);
        //.DrawLine(m_GroundCheck.position, new Vector3(m_GroundCheck.position.x, m_GroundCheck.position.y + k_GroundedRadius.y));
        //.Log(colliders.Length);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
                //.Log(colliders[i]);
                if (colliders[i].CompareTag("MovingPlatform"))
                {
                    if (m_GroundCheck.position.y >= colliders[i].transform.position.y)
                    {
                        transform.parent = colliders[i].transform;
                    }
                }
            }
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.collider.CompareTag("MovingPlatform"))
        {
            transform.parent = null;
        }
    }
}
