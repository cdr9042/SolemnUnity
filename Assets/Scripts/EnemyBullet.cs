using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour {
	
	public Vector2 speed = new Vector2(10, 10);
    public Vector2 direction ;
    private Vector2 movement;
	private Rigidbody2D rigidbodyComponent;

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.name == "Player"){
			
			Destroy(this.gameObject);
		}
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		movement = new Vector2(
          speed.x * direction.x,
          speed.y * direction.y);
		
	}
	 void FixedUpdate()
    {
        if (rigidbodyComponent == null) rigidbodyComponent = GetComponent<Rigidbody2D>();

        // Apply movement to the rigidbody
        rigidbodyComponent.velocity = movement;
    }
}
