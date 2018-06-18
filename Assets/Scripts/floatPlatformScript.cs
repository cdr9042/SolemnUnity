using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floatPlatformScript : MonoBehaviour
{
    private bool isStoodOn = false;
    private string phase = "idle";
    private Collider2D m_Collider;
    private float vSpeed = 0f, direction = -1;
	private Vector2 velocity;
    private float max_vSpeed, giaToc;
    private Vector3 _startPosition;
	private Transform m_Transform;
	public float moveRange, moveOffset;
    // Use this for initialization
    void Start()
    {
        m_Collider = transform.GetComponent<Collider2D>();
		m_Transform = transform.parent.transform;
		_startPosition = m_Transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

		m_Transform.position = _startPosition + new Vector3(0.0f, Mathf.Sin(Time.time + moveOffset) * moveRange, 0.0f);

        // switch (phase)
        // {
        //     case "idle":
        //         if (isStoodOn)
        //         {
        //             vSpeed = max_vSpeed;
        //             direction = -1;
        //             phase = "fall";
        //         }
        //         break;
        //     case "fall":

        //         Debug.Log(Mathf.Sign(vSpeed));
        //         // Debug.Log(Time.deltaTime);
        //         // vSpeed -= 0.001f;

        //         if (vSpeed <= 0.0001f)
        //         {
        //             Debug.Log(vSpeed);
        //             direction = 1;
        //             phase = "recover";
        //             vSpeed = 0f;
		// 			velocity = Vector2.zero;
        //         }

        //         vSpeed -= giaToc * Time.deltaTime;
        //         m_Transform.Translate(0, vSpeed * direction, 0);
        //         // Debug.Break();
        //         break;
        //     case "recover":
        //         m_Transform.position = Vector2.SmoothDamp(m_Transform.position, _startPosition, ref velocity, 0.03f,max_vSpeed,Time.deltaTime);
        //         // if (vSpeed < max_vSpeed)
        //         // {
        //         //     vSpeed += giaToc * Time.deltaTime;
        //         // }
        //         if (Mathf.Abs(m_Transform.position.y - _startPosition.y) < 0.1f)
        //         {
        //             m_Transform.position = _startPosition;
        //             vSpeed = 0f;
        //             if (!isStoodOn)
        //             { phase = "idle"; }
        //         }
        //         break;
        // }
        // if (vSpeed != 0)
        //     m_Transform.Translate(0, vSpeed * direction, 0);
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        // Debug.Log(col);
		// if (phase == "idle")
        if (col.collider.CompareTag("Player"))
        {
            isStoodOn = true;
        };
    }
    void OnCollisionExit2D(Collision2D col)
    {
        // Debug.Log(col);
        if (col.collider.CompareTag("Player"))
        {
            isStoodOn = false;
        };
    }
    // void OnClo
}
