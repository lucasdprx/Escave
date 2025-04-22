using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerWallJump : MonoBehaviour
{
    #region Variables

    public bool _isGrabUnlock;
    [Header("Checks")]
    [SerializeField] private Transform _wallCheckRight;
    [SerializeField] private Transform _wallCheckLeft;
    [SerializeField] private Transform _groundCheck;
    private const float _checkRadius = 0.2f;

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
    private float _initialGravityScale;
    private Vector2 _moveInput;
    
    [SerializeField] private GameObject _staminaEffect;
    private bool _isStaminaActive;
    private Animator _animator;
    private AudioManager _audioManager;
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
    private void Start()
    {
        _audioManager = AudioManager.Instance;
        _animator = _staminaEffect.GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _initialGravityScale = _rb.gravityScale;

        if (SceneManager.GetActiveScene().buildIndex >= 2)
            _isGrabUnlock = true;
    }

    private void Update()
    {
        if (IsOnPlatform(_groundCheck.position, _wallLayer))
        {
            _canWallClimb = true;
            _isWallClimbing = false;
            _wallStayTimer = 0;
            _isWallClimbingLeft = false;
            _isWallClimbingRight = false;
        }
        
        if (_isInputDone && IsOnPlatform(_wallCheckRight.position, _wallLayer) && _moveInput.x > 0.1f && !IsOnPlatform(_groundCheck.position, _wallLayer))
        {
            _isWallClimbingRight = true;
            _isWallClimbing = true;
        } else if (_isInputDone && IsOnPlatform(_wallCheckLeft.position, _wallLayer) && _moveInput.x < 0.1f && !IsOnPlatform(_groundCheck.position, _wallLayer)) {
            _isWallClimbingLeft = true;
            _isWallClimbing = true;
        }

        if (_isWallClimbingLeft || _isWallClimbingRight)
        {
            if (!_isStaminaActive)
            {
                _isStaminaActive = true;
                _staminaEffect.SetActive(true);
            }

            _wallStayTimer += Time.deltaTime;

            // Animation speed augment de x1 � x5 salon _wallStayTimer
            _animator.speed = Mathf.Lerp(1f, 2.5f, _wallStayTimer / _wallStayTime);

            if (_wallStayTimer >= _wallStayTime)
            {
                _isWallClimbingLeft = false;
                _isWallClimbingRight = false;
                _canWallClimb = false;
                _rb.gravityScale = _initialGravityScale;
                _audioManager.PlaySound(AudioType.enduranceRunOut);
                _isInputDone = false;
            }
        }



        if (!_isWallClimbing && !_isGravitySet)
        {
            _isGravitySet = true;
            _isWallClimbingLeft = false;
            _isWallClimbingRight = false;
            _rb.gravityScale = _initialGravityScale;
        }

        if (IsOnPlatform(_groundCheck.position, _wallLayer) || IsOnPlatform(_groundCheck.position, 1 << LayerMask.NameToLayer("OneWayPlatform")))
        {
            _isWallClimbingLeft = false;
            _isWallClimbingRight = false;
            _isWallClimbing = false;
            _isInputDone = false;
            _rb.gravityScale = _initialGravityScale;
        }
        {
            _canWallClimb = true;
            _isWallClimbing = false;
            _wallStayTimer = 0;

            if (_isStaminaActive)
            {
                _animator.speed = 1f;
                _staminaEffect.SetActive(false);
                _isStaminaActive = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if(IsOnPlatform(_groundCheck.position, _wallLayer)) return;
        if (!_canWallClimb) return;
        if (_isWallJumping) return;
        if (!_isWallClimbing) return;

        _isGravitySet = false;

        if ((_isWallClimbingLeft && !IsOnPlatform(_wallCheckLeft.position, _wallLayer) || _isWallClimbingRight && !IsOnPlatform(_wallCheckRight.position, _wallLayer)) && !_isWallJumping)
        {
            _rb.AddForce(_onTopWallForce, ForceMode2D.Impulse);
        }

        if (_isWallClimbingRight && IsOnPlatform(_wallCheckRight.position, _wallLayer))
        {
            _rb.gravityScale = 0;
            if (_moveInput.y != 0)
            {
                OperateWallClimb();
                return;
            }
            _rb.linearVelocity = new Vector2(0f, 0f);
        }
        else if (_isWallClimbingLeft && IsOnPlatform(_wallCheckLeft.position, _wallLayer))
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
            _rb.gravityScale = _initialGravityScale;
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
    }

    public void Climb(InputAction.CallbackContext _ctx)
    {
        if (!_isGrabUnlock) return;
        
        if (_ctx.started)
        {
            _isInputDone = true;
        }
        else if (_ctx.canceled)
        {
            _isInputDone = false;
            _isWallClimbing = false;
            _isWallClimbingLeft = false;
            _isWallClimbingRight = false;
        }
    }
    
    #endregion

    #region WallJump
    
    private void SetJumpingValues()
    {
        _isWallJumping = true;
        _isWallClimbingRight = false;
        _isWallClimbingLeft = false;
        _rb.gravityScale = _initialGravityScale;
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
        } 
        else if (_moveInput.y < 0)
        {
            _rb.linearVelocity = new Vector2(0f, -_wallClimbSpeed);
        }
    }
    
    #endregion
    
    #region Bool Functions
    private static bool IsOnPlatform(Vector3 position, LayerMask layerMask)
    {
        return Physics2D.OverlapCircle(position, _checkRadius, layerMask);
    }
    
    #endregion
}
