using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlyer : MonoBehaviour
{
    // 1 - Designer variables
    public Vector2 speed = new Vector2(10, 10);
    public Vector2 direction;
    private Vector2 movement;
    private bool m_FacingRight;
    private Rigidbody2D rigidbodyComponent;
    private float _startPos;
    private float _endPos;
    public int UnitsToMove = 10;
    public bool _moveRight = true;

    private Transform _player;

    public void Awake()
    {
        rigidbodyComponent = GetComponent<Rigidbody2D>();
        _startPos = transform.position.x;
        _endPos = _startPos + UnitsToMove;
        _player = GameObject.Find("DataMaster").GetComponent<GameInit>().m_PlayerPrefab.transform;

    }
    void Update()
    {
        //2 - Movement

        Move(direction);
    }

    public void Move(Vector2 move)
    {
        movement = new Vector2(
        speed.x * direction.x,
        speed.y * direction.y);

        float velocityx = Mathf.Lerp(rigidbodyComponent.velocity.x, speed.x, 5f * Time.deltaTime);
		Vector2 moveLeft = new Vector2(speed.x*-1,0);
        if (_moveRight)
        {
            movement = new Vector2(velocityx * 1, 0);
            if (!m_FacingRight)
                // ... flip the player.
                Flip();

        }
        else if(!_moveRight)
        {
            movement = moveLeft;

            if (m_FacingRight)
                // ... flip the player.
                Flip();
        }
        if (rigidbodyComponent.position.x >= _endPos)
            _moveRight = false;
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (rigidbodyComponent.position.x <= _startPos)
            _moveRight = true;

    }
    void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        //Debug.Log(theScale);
    }
    void FixedUpdate()
    {
        if (rigidbodyComponent == null) rigidbodyComponent = GetComponent<Rigidbody2D>();

        // Apply movement to the rigidbody
        rigidbodyComponent.velocity = movement;
    }
}
