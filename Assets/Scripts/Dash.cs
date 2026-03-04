using System;
using UnityEngine;



public class Dash : MonoBehaviour
{
    private Rigidbody2D _rigidBody;
    private Vector2 _dashDirection;
    private PlayerMovement _playerMovement;
    private float _dashForce = 50000f;
    private int _maxDashCount = 3;
    private int _dashCount;
    private float _dashCooldown = 1.0f;
    private float _dashCooldownTimer = 0f;
    
    // Dash delay to prevent spam
    private float _dashDelay = 0.2f;
    private float _dashDelayTimer = 0f;
    
    // Dash duration state
    public bool isDashing = false;
    private float _dashDuration = 0.15f;
    private float _dashDurationTimer = 0f;
    
    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _playerMovement = GetComponent<PlayerMovement>();
        _dashCount = _maxDashCount;
    }
    void Update()
    {
        UpdateDashState();
        PerformDash();
        ReplenishDash();
    }
    
    void UpdateDashState()
    {
        // Update dash delay timer
        if (_dashDelayTimer > 0)
        {
            _dashDelayTimer -= Time.deltaTime;
        }
        
        // Update dash duration state
        if (isDashing)
        {
            _dashDurationTimer -= Time.deltaTime;
            if (_dashDurationTimer <= 0)
            {
                isDashing = false;
            }
        }
    }
    
    void PerformDash()
    {
        // Check if can dash: has dashes available, dash delay has passed, and input pressed
        if (_dashCount > 0 && _dashDelayTimer <= 0 && Input.GetKeyDown(KeyCode.Space))
        {
            // Get current input direction for diagonal dashing
            _dashDirection = Vector2.zero;
            
            if (Input.GetKey(KeyCode.W))
            {
                _dashDirection.y = 1;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                _dashDirection.y = -1;
            }
            
            if (Input.GetKey(KeyCode.A))
            {
                _dashDirection.x = -1;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                _dashDirection.x = 1;
            }
            
            // If no input, use last direction from PlayerMovement
            if (_dashDirection == Vector2.zero)
            {
                Direction lastDir = _playerMovement.lastDirection;
                switch (lastDir)
                {
                    case Direction.Up:
                        _dashDirection = new Vector2(0, 1);
                        break;
                    case Direction.Down:
                        _dashDirection = new Vector2(0, -1);
                        break;
                    case Direction.Left:
                        _dashDirection = new Vector2(-1, 0);
                        break;
                    case Direction.Right:
                        _dashDirection = new Vector2(1, 0);
                        break;
                    default:
                        _dashDirection = new Vector2(1, 0);
                        break;
                }
            }
            
            _rigidBody.AddForce(_dashDirection.normalized * (_dashForce * Time.fixedDeltaTime));
            _dashCount--;
            
            // Start dash duration state
            isDashing = true;
            _dashDurationTimer = _dashDuration;
            
            // Set dash delay timer
            _dashDelayTimer = _dashDelay;
            
            // Start cooldown timer if this is the first dash used
            if (_dashCooldownTimer <= 0)
            {
                _dashCooldownTimer = _dashCooldown;
            }
            
            Debug.Log("Dashed! Remaining dashes: " + _dashCount);
        }
    }
    
    void ReplenishDash()
    {
        if (_dashCount < _maxDashCount)
        {
            _dashCooldownTimer -= Time.deltaTime;
            
            if (_dashCooldownTimer <= 0)
            {
                _dashCount++;
                Debug.Log("One dash replenished! Current dashes: " + _dashCount);
                
                if (_dashCount < _maxDashCount)
                {
                    _dashCooldownTimer = _dashCooldown;
                }
            }
        }
    }
}
