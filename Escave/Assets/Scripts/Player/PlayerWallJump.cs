using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWallJump : MonoBehaviour
{
    #region Variables

    public bool _isGrabUnlock;
    [Header("Checks")]
    [SerializeField] private Transform _wallCheckRight;
    [SerializeField] private Transform _wallCheckLeft;
    [SerializeField] private Transform _groundCheck;
    private float _checkRadius = 0.2f;

    [Header("OnJump")]
    [SerializeField] private Vector2 _wallJumpForce;
    [SerializeField] private float _wallJumpTime;
    
    [Header("OnWall")]
    [SerializeField] private float _wallClimbSpeed;
    [SerializeField] private float _wallStayTime;
    
    [Space(10)]
    [SerializeField] private LayerMask _wallLayer;
    
    [SerializeField] private Vector2 _onTopWallForce;
    
    private Rigidbody2D _rb;
    private Vector2 _moveInput;

    private PlayerSFX _playerSFX;

    #endregion

    #region BoolChecks

    public bool _isWallClimbingRight { get; private set; }
    public bool _isWallClimbingLeft { get; private set; }
    public bool _isWallJumping { get; private set; }
    private bool _canWallClimb;
    private bool _isWallClimbing;

    private bool _isGravitySet;
    private bool _isInputDone;
    
    #endregion
    
    #region Timer

    private float _wallStayTimer;
    private float _wallJumpTimer;
    
    #endregion

    #region Unity Callbacks
    
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerSFX = GetComponent<PlayerSFX>();
    }

    private void Update()
    {
        if (IsGrounded())
        {
            _canWallClimb = true;
            _isWallClimbing = false;
            _wallStayTimer = 0;
            _isWallClimbingLeft = false;
            _isWallClimbingRight = false;
        }
        
        if (_isInputDone && IsWallRight() && _moveInput.x > 0.1f && !IsGrounded())
        {
            _isWallClimbingRight = true;
            _isWallClimbing = true;
        } else if (_isInputDone && IsWallLeft() && _moveInput.x < 0.1f && !IsGrounded()) {
            _isWallClimbingLeft = true;
            _isWallClimbing = true;
        }

        if (_isWallClimbingLeft || _isWallClimbingRight)
        {
            _wallStayTimer += Time.deltaTime;
            if (_wallStayTimer >= _wallStayTime)
            {
                _isWallClimbingLeft = false;
                _isWallClimbingRight = false;
                _canWallClimb = false;
                _rb.gravityScale = 4;
                _playerSFX.PlayEnduranceRunOutSFX();
                _isInputDone = false;
            }
        }

        if (!_isWallClimbing && !_isGravitySet)
        {
            _isGravitySet = true;
            _isWallClimbingLeft = false;
            _isWallClimbingRight = false;
            _rb.gravityScale = 4;
        }
    }
    
    private void FixedUpdate()
    {
        if(IsGrounded()) return;
        if (!_canWallClimb) return;
        if (_isWallJumping) return;
        if (!_isWallClimbing) return;

        _isGravitySet = false;

        if (((_isWallClimbingLeft && !IsWallLeft()) || (_isWallClimbingRight && !IsWallRight())) && !_isWallJumping)
        {
            _rb.AddForce(_onTopWallForce, ForceMode2D.Impulse);
        }

        if (_isWallClimbingRight && IsWallRight())
        {
            _rb.gravityScale = 0;
            if (_moveInput.y != 0)
            {
                OperateWallClimb();
                return;
            }
            _rb.linearVelocity = new Vector2(0f, 0f);
        }
        else if (_isWallClimbingLeft && IsWallLeft())
        {
            _rb.gravityScale = 0;
            if (_moveInput.y != 0)
            {
                OperateWallClimb();
                return;
            }
            
            _rb.linearVelocity = new Vector2(0f, 0f);
        }
        else
        {
            _isWallClimbingRight = false;
            _isWallClimbingLeft = false;
            _rb.gravityScale = 4;
        }
    }

    #endregion
    
    #region ActionMap Calls
    public void OperateWallAction(InputAction.CallbackContext _ctx)
    {
        _moveInput = _ctx.ReadValue<Vector2>();
    }

    public void OperateWallJumpAction(InputAction.CallbackContext _ctx)
    {
        if (!_ctx.started) return;

        if (_isWallClimbingRight)
        {
            _rb.linearVelocity = new Vector2(-_wallJumpForce.x, _wallJumpForce.y);
            SetJumpingValues();
        } 
        else if (_isWallClimbingLeft)
        {
            _rb.linearVelocity = _wallJumpForce;
            SetJumpingValues();
        }
        _playerSFX.PlayWallJumpSFX();
    }

    public void Climb(InputAction.CallbackContext _ctx)
    {
        if (_isGrabUnlock)
        {
            if (_ctx.started)
            {
                if (_isWallClimbing)
                {
                    _isWallClimbing = false;
                    _isWallClimbingLeft = false;
                    _isWallClimbingRight = false;
                    return;
                }
                
                _isInputDone = true;
            }
            else if(_ctx.canceled) _isInputDone = false;
        }
    }
    
    #endregion

    #region WallJump
    
    private void SetJumpingValues()
    {
        _isWallJumping = true;
        _isWallClimbingRight = false;
        _isWallClimbingLeft = false;
        _rb.gravityScale = 4;
        _wallStayTimer += 3;
        StartCoroutine(WallJumpTime());
    }

    private IEnumerator WallJumpTime()
    {
        float _elapsedTime = 0;

        while (_elapsedTime < _wallJumpTime)
        {
            _elapsedTime += Time.deltaTime;
            yield return null;
        }
        _isWallJumping = false;
    }
    
    #endregion

    #region WallClimb
    private void OperateWallClimb()
    {
        if (_moveInput.y > 0)
        {
            _rb.linearVelocity = new Vector2(0f, _wallClimbSpeed);
            return;
        } else if (_moveInput.y < 0)
        {
            _rb.linearVelocity = new Vector2(0f, -_wallClimbSpeed);
            return;
        }
    }
    
    #endregion
    
    #region Bool Functions

    public bool IsWallRight()
    {
        return Physics2D.OverlapCircle(_wallCheckRight.position, _checkRadius, _wallLayer);
    }

    public bool IsWallLeft()
    {
        return Physics2D.OverlapCircle(_wallCheckLeft.position, _checkRadius, _wallLayer);
    }

    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(_groundCheck.position, _checkRadius, _wallLayer);
    }
    
    #endregion
}
