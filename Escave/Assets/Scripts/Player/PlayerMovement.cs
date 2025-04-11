using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed;
    public float jumpForce; [Space]

    [Header("Ground Check")]
    [SerializeField] private Transform _groundCheck;
    private float _groundCheckRadius = 0.45f;
    [SerializeField] private LayerMask _groundLayer; [Space]
    private bool _isGrounded;


    private Rigidbody2D _rb;
    private Vector2 _moveInput;
    private SpriteRenderer _spriteRenderer;

    [Header("Heavy Fall Detection")]
    [SerializeField] private float _heavyFallYVelocity; 
    private bool _hasLanded;
    private bool _inHeavyFall;


    private PlayerWallJump _playerWallJump;
    private PlayerSFX      _playerSFX;
    
    bool _isOnOneWayPlatform;


    private GrapplingHook _grapplingHook;
    private bool _justDetachedFromHook;
    private float _detachGraceTime = .3f;
    private float _detachTimer;

    private float _verticalInput;

    private Vector2 boxSize;

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


    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _playerWallJump = GetComponent<PlayerWallJump>();
        _grapplingHook = GetComponentInChildren<GrapplingHook>();
        _playerSFX = GetComponent<PlayerSFX>();

        boxSize = new Vector2(1.2f, 0.3f);
    }

    private void Update()
    {
        _isGrounded = Physics2D.OverlapBox(_groundCheck.position, boxSize, 0f, _groundLayer);

        if (_isGrounded && _justDetachedFromHook)
        {
            _justDetachedFromHook = false;
        }

        HandleSpriteFlip();
        //Debug.DrawRay(transform.position, DirectionToVector2(LastDirection), Color.red);
    }

    private void FixedUpdate()
    {
        if (_grapplingHook != null && _grapplingHook._isGrappled)
        {
            Vector2 force = new Vector2(_moveInput.x, 0f) * moveSpeed;
            _rb.AddForce(force, ForceMode2D.Force);
        }
        else if (!_playerWallJump._isWallClimbingLeft && !_playerWallJump._isWallClimbingRight && !_playerWallJump._isWallJumping)
        {
            //If just detached from hook, wait till player use input (else use balancing velocity
            if (_justDetachedFromHook)
            {
                if (Mathf.Abs(_moveInput.x) > 0.01f)
                {
                    //Player use input -> autorize air control
                    float controlFactor = 0.15f; //adjust speed
                    _rb.linearVelocity = new Vector2(
                        Mathf.Lerp(_rb.linearVelocity.x, _moveInput.x * moveSpeed, controlFactor),
                        _rb.linearVelocity.y
                    );
                }

                //back to base movement
                if (_isGrounded)
                {
                    _justDetachedFromHook = false;
                }
            }
            else
            {
                if (_isGrounded)
                {
                    _rb.linearVelocity = new Vector2(_moveInput.x * moveSpeed, _rb.linearVelocity.y);
                }
                else
                {
                    if (Mathf.Abs(_moveInput.x) > 0.01f)
                    {
                        _rb.linearVelocity = new Vector2(_moveInput.x * moveSpeed, _rb.linearVelocity.y);
                    }
                    else
                    {
                        float dampFactor = 0.9f;
                        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x * dampFactor, _rb.linearVelocity.y);
                    }
                }
            }
        }



        // Handle step SFX
        if (_rb.linearVelocity.x != 0 && _rb.linearVelocity.y == 0) _playerSFX.PlayWalkSFX();

        // Handle jump landing SFX
        if (_rb.linearVelocity.y < 0 && !_isGrounded) //Check if the player is falling
        {
            _hasLanded = false;
        }

        if (_hasLanded) return; //If the player was falling, check if it landed
        if (_rb.linearVelocity.y <= _heavyFallYVelocity) _inHeavyFall = true;
        if (_isGrounded)
        {
            _hasLanded = true;

            if (_inHeavyFall) _playerSFX.PlayHeavyJumpLandSFX();
            else              _playerSFX.PlayJumpLandSFX();

            _inHeavyFall = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D _other)
    {
        if (_other.gameObject.layer == LayerMask.NameToLayer("OneWayPlatform"))
            _isOnOneWayPlatform = true;
    }

    private void OnCollisionExit2D(Collision2D _other)
    {
        if (_other.gameObject.layer == LayerMask.NameToLayer("OneWayPlatform"))
            _isOnOneWayPlatform = false;
    }
    
    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();

        _verticalInput = _moveInput.y;

        if (_moveInput.sqrMagnitude > 0.01f)
        {
            LastDirection = GetEightDirection(_moveInput.normalized);
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (_moveInput.y < -0.95f && _isOnOneWayPlatform) return;
        
        if (context.performed && _isGrounded)
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpForce);
        }
    }

    private Direction GetEightDirection(Vector2 input)
    {
        float angle = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg;
        angle = (angle + 360f) % 360f;

        if (angle >= 337.5f || angle < 22.5f)  return Direction.Right;
        if (angle >= 22.5f  && angle < 67.5f)  return Direction.UpRight;
        if (angle >= 67.5f  && angle < 112.5f) return Direction.Up;
        if (angle >= 112.5f && angle < 157.5f) return Direction.UpLeft;
        if (angle >= 157.5f && angle < 202.5f) return Direction.Left;
        if (angle >= 202.5f && angle < 247.5f) return Direction.DownLeft;
        if (angle >= 247.5f && angle < 292.5f) return Direction.Down;
        if (angle >= 292.5f && angle < 337.5f) return Direction.DownRight;

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

    private void HandleSpriteFlip()
    {
        if (Time.timeScale == 0) return;

        if (_moveInput.x > 0 && _spriteRenderer.flipX)
            _spriteRenderer.flipX = false;
        else if (_moveInput.x < 0 && !_spriteRenderer.flipX)
            _spriteRenderer.flipX = true;
    }
    
    public void OnDetachedFromHook()
    {
        _justDetachedFromHook = true;
        _detachTimer = _detachGraceTime;
    }

    public float GetVerticalInput() => _verticalInput;
}
