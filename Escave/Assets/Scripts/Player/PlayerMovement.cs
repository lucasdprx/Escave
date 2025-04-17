using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed;
    public float jumpForce;
    [Space] private float baseMoveSpeed;
    [Space] private float baseJumpForce;

    [Header("Ground Check")]
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private LayerMask _groundLayer;

    [Header("Heavy Fall Detection")]
    [SerializeField] private float _heavyFallYVelocity; 
    private bool _hasLanded;
    private bool _inHeavyFall;
    
    [Header("Input Buffer")]
    [SerializeField] private float _inputBufferTime = 0.15f;

    private float _inputActionTime;
    private bool _isInputBuffering;

    [Header("Variable Jump Settings")]
    [SerializeField] private float _maxJumpDuration = 0.35f;
    
    private bool _isJumping;
    private float _jumpTimeCounter;
    
    [Header("Grappling Hook")]
    public UnityEvent _onJumpWhileGrappling;

    [Header("Knockback")]
    [SerializeField] private float _knockbackDuration = 0.2f;

    public Direction LastDirection { get; private set; } = Direction.Right;
    public enum Direction
    {
        Right,
        UpRight,
        Up,
        UpLeft,
        Left,
        DownLeft,
        Down,
        DownRight
    }

    public static event Action OnStepSound;
    public static bool _isGrounded;
    
    private PlayerWallJump _playerWallJump;
    private bool _isOnOneWayPlatform;
    private bool _isGrappling;
    private bool _justDetachedFromHook;
    private bool _isKnockback;
    private float _knockbackTimer;
    private Vector2 _boxSize;
    private Rigidbody2D _rb;
    private PlayerInputHandler _playerInputHandler;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerWallJump = GetComponent<PlayerWallJump>();
        _playerInputHandler = GetComponent<PlayerInputHandler>();
        
        baseMoveSpeed = moveSpeed;
        baseJumpForce = jumpForce;

        _boxSize = new Vector2(1.2f, 0.3f);
        
        _playerInputHandler.OnJumpPressed += OnJumpPressed;
        _playerInputHandler.OnJumpReleased += OnJumpRelease;
        _playerInputHandler.OnMove += OnMove;
    }

    public void Reset()
    {
        moveSpeed = baseMoveSpeed;
        jumpForce = baseJumpForce;
        _rb.gravityScale = 4;
    }

    private void Update()
    {

        if (_isKnockback)
        {
            _knockbackTimer -= Time.deltaTime;
            if (_knockbackTimer <= 0f)
            {
                _isKnockback = false;
            }
            return;
        }

        _isGrounded = Physics2D.OverlapBox(_groundCheck.position, _boxSize, 0f, _groundLayer);

        if (_isGrounded && _justDetachedFromHook)
        {
            _justDetachedFromHook = false;
        }

        if (_isGrounded && _inputActionTime > 0) 
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpForce);
            _inputActionTime = -1;
            _isJumping = true;
            _jumpTimeCounter = _maxJumpDuration;
        }

        if (_isInputBuffering)
        {
            _inputActionTime -= Time.deltaTime;
        }
       
    }

    private void FixedUpdate()
    {
       if (_isKnockback) return;
        HandleGrapplingMovement();
        HandleGroundAndAirMovement();
        HandleJump();
        HandleSFX();
    }

    private void HandleGrapplingMovement()
    {
        if (!_isGrappling) return;
        
        Vector2 force = new Vector2(_playerInputHandler.MoveInput.x, 0f) * moveSpeed;
        _rb.AddForce(force, ForceMode2D.Force);
    }

    private void HandleGroundAndAirMovement()
    {

        if (_isGrappling || _playerWallJump._isWallClimbingLeft || _playerWallJump._isWallClimbingRight || _playerWallJump._isWallJumping || _isKnockback)
            return;

        if (_justDetachedFromHook)
        {
            if (Mathf.Abs(_playerInputHandler.MoveInput.x) > 0.01f)
            {
                const float controlFactor = 0.15f;
                _rb.linearVelocity = new Vector2(
                    Mathf.Lerp(_rb.linearVelocity.x, _playerInputHandler.MoveInput.x * moveSpeed, controlFactor),
                    _rb.linearVelocity.y
                );
            }

            if (_isGrounded)
            {
                _justDetachedFromHook = false;
            }
            return;
        }

        if (_isGrounded || Mathf.Abs(_playerInputHandler.MoveInput.x) > 0.01f)
        {
            _rb.linearVelocity = new Vector2(_playerInputHandler.MoveInput.x * moveSpeed, _rb.linearVelocity.y);
        }
        else
        {
            const float dampFactor = 0.9f;
            Vector2 linearVelocity = _rb.linearVelocity;
            linearVelocity = new Vector2(linearVelocity.x * dampFactor, linearVelocity.y);
            _rb.linearVelocity = linearVelocity;
        }
    }

    private void HandleJump()
    {
        if (!_isJumping) return;

        if (_jumpTimeCounter > 0f)
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpForce);
            _jumpTimeCounter -= Time.fixedDeltaTime;
        }
        else
        {
            _isJumping = false;
        }
    }

    private void HandleSFX()
    {
        if (_rb.linearVelocity.x != 0 && _rb.linearVelocity.y == 0)
        {
            OnStepSound?.Invoke();
        }

        if (_rb.linearVelocity.y < 0 && !_isGrounded)
        {
            _hasLanded = false;
        }

        if (_hasLanded) return;

        if (_rb.linearVelocity.y <= _heavyFallYVelocity)
        {
            _inHeavyFall = true;
        }

        if (!_isGrounded) return;
        
        _hasLanded = true;
        AudioManager.Instance.PlaySound(_inHeavyFall ? AudioType.jumpHeavyLand : AudioType.jumpLand);
        _inHeavyFall = false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("OneWayPlatform"))
            _isOnOneWayPlatform = true;
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("OneWayPlatform"))
            _isOnOneWayPlatform = false;
    }
    
    private void OnMove()
    {
        if (_playerInputHandler.MoveInput.sqrMagnitude > 0.01f)
        {
            LastDirection = GetEightDirection(_playerInputHandler.MoveInput.normalized);
        }
    }

    private void OnJumpPressed()
    {
        if (Time.timeScale <= 0) return;
        
        if (_playerInputHandler.MoveInput.y < -0.95f && _isOnOneWayPlatform) return;

        _onJumpWhileGrappling.Invoke();
        _inputActionTime = _inputBufferTime;
        _isInputBuffering = true;

        if (!_isGrounded) return;
        
        _isJumping = true;
        _jumpTimeCounter = _maxJumpDuration;
    }
    private void OnJumpRelease()
    {
        _isJumping = false;
    }

    private Direction GetEightDirection(Vector2 input)
    {
        float angle = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg;
        angle = (angle + 360f) % 360f;

        if (angle is >= 337.5f or < 22.5f)  return Direction.Right;
        if (angle is >= 22.5f and < 67.5f)  return Direction.UpRight;
        if (angle is >= 67.5f and < 112.5f) return Direction.Up;
        if (angle is >= 112.5f and < 157.5f) return Direction.UpLeft;
        if (angle is >= 157.5f and < 202.5f) return Direction.Left;
        if (angle is >= 202.5f and < 247.5f) return Direction.DownLeft;
        if (angle is >= 247.5f and < 292.5f) return Direction.Down;
        if (angle is >= 292.5f and < 337.5f) return Direction.DownRight;

        return Direction.Right;
    }

    public Vector2 DirectionToVector2(Direction dir)
    {
        switch (dir)
        {
            case Direction.Up: return Vector2.up;
            case Direction.UpRight: return new Vector2(1, 1).normalized;
            case Direction.Right: return Vector2.right;
            case Direction.DownRight: return new Vector2(1, -1).normalized;
            case Direction.Down: return Vector2.down;
            case Direction.DownLeft: return new Vector2(-1, -1).normalized;
            case Direction.Left: return Vector2.left;
            case Direction.UpLeft: return new Vector2(-1, 1).normalized;
            default: return Vector2.right;
        }
    }
    
    public void OnDetachedFromHook()
    {
        _justDetachedFromHook = true;
    }

    public float GetVerticalInput() => _playerInputHandler.VerticalInput;
    
    public void SetIsGrappling(bool value)
    {
        _isGrappling = value;
    }
    private void OnDestroy()
    {
        if (_playerInputHandler == null) return;

        _playerInputHandler.OnJumpPressed -= OnJumpPressed;
        _playerInputHandler.OnJumpReleased -= OnJumpRelease;
        _playerInputHandler.OnMove -= OnMove;
    }
    public bool IsWallClimb()
    {
        return _playerWallJump._isWallClimbingLeft || _playerWallJump._isWallClimbingRight;
    }

    public void ApplyKnockback(Vector2 direction, float force)
    {
        _isKnockback = true;
        _knockbackTimer = _knockbackDuration;
        _rb.linearVelocity = Vector2.zero;// Optional 
        _rb.AddForce(direction * force, ForceMode2D.Impulse);
    }
}
