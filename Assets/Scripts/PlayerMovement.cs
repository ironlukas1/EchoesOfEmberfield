using System;
using UnityEngine;

public enum Direction{
    Up,
    Down,
    Left,
    Right,
    None
}
public enum AnimationDirection{
    Up,
    Down,
    Left,
    Right
}

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _rigidBody;
    private Vector2 _direction;
    public Direction lastDirection = Direction.Right;
    public AnimationDirection animDirection = AnimationDirection.Down;
    public float speed; // change to private before release
    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        _direction = new Vector2(0, 0);

        if (Input.GetKey(KeyCode.W))
        {
            _direction.y = 1;
            lastDirection = Direction.Up;
        }else if (Input.GetKey(KeyCode.S))
        {
            _direction.y = -1;
            lastDirection = Direction.Down;
        }

        if (Input.GetKey(KeyCode.A))
        {
            _direction.x = -1;
            lastDirection = Direction.Left;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            _direction.x = 1;
            lastDirection = Direction.Right;
        }
    }
    private void FixedUpdate()
    {
        if (Input.GetKeyUp(KeyCode.W) && Input.GetKeyUp(KeyCode.S))
        {
            Debug.Log("Direction none");
            _direction.y = 0;
            lastDirection = Direction.None;
            _rigidBody.linearVelocity = Vector2.zero;
        } else
        {
            _rigidBody.AddForce(_direction.normalized * (speed * Time.fixedDeltaTime));
        }
        if (Input.GetKeyUp(KeyCode.A) && Input.GetKeyUp(KeyCode.D))
        {
            Debug.Log("Direction none");
            _direction.x = 0;
            lastDirection = Direction.None;
            _rigidBody.linearVelocity = Vector2.zero;
        }
        else
        {
            _rigidBody.AddForce(_direction.normalized * (speed * Time.fixedDeltaTime));
        }
    }
}
