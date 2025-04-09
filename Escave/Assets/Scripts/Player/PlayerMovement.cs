using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    [Header("Ground Check")]
    [SerializeField] private Transform _groundCheck;
    private float _groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask _groundLayer;

    private Rigidbody2D _rb;
    private Vector2 _moveInput;
    private bool _isGrounded;
    private SpriteRenderer _spriteRenderer;
    private bool _hasLanded;
    
    private PlayerWallJump _playerWallJump;
    private PlayerSFX      _playerSFX;

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
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _playerWallJump = GetComponent<PlayerWallJump>();
        _playerSFX      = GetComponent<PlayerSFX>();
    }

    private void Update()
    {
        _isGrounded = Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, _groundLayer);

        HandleSpriteFlip();
        //Debug.DrawRay(transform.position, DirectionToVector2(LastDirection), Color.red);
    }

    private void FixedUpdate()
    {
        if (_playerWallJump._isWallClimbingLeft || _playerWallJump._isWallClimbingRight || _playerWallJump._isWallJumping) return;
        

        _rb.linearVelocity = new Vector2(_moveInput.x * moveSpeed, _rb.linearVelocity.y);

        // Handle step SFX
        if (_rb.linearVelocity.x != 0 && _rb.linearVelocity.y == 0) _playerSFX.PlayWalkSFX();

        // Check if the player's landing after a jump to play the SFX
        if (_hasLanded) return;
        if (_isGrounded)
        {
            _hasLanded = _isGrounded;
            _playerSFX.PlayJumpLandSFX();
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();

        if (_moveInput.sqrMagnitude > 0.01f)
        {
            LastDirection = GetEightDirection(_moveInput.normalized);
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && _isGrounded)
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpForce);
            StartCoroutine(TestRoutine());
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

    //private Vector2 DirectionToVector2(Direction dir)
    //{
    //    switch (dir)
    //    {
    //        case Direction.Up: return Vector2.up;
    //        case Direction.UpRight: return new Vector2(1, 1).normalized;
    //        case Direction.Right: return Vector2.right;
    //        case Direction.DownRight: return new Vector2(1, -1).normalized;
    //        case Direction.Down: return Vector2.down;
    //        case Direction.DownLeft: return new Vector2(-1, -1).normalized;
    //        case Direction.Left: return Vector2.left;
    //        case Direction.UpLeft: return new Vector2(-1, 1).normalized;
    //        default: return Vector2.zero;
    //    }
    //}

    private void HandleSpriteFlip()
    {
        if (_moveInput.x > 0 && _spriteRenderer.flipX)
            _spriteRenderer.flipX = false;
        else if (_moveInput.x < 0 && !_spriteRenderer.flipX)
            _spriteRenderer.flipX = true;
    }

    private IEnumerator TestRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        _hasLanded = false;
    }
}
