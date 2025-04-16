using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour
{
    [Header("Sprite Animation")]
    [SerializeField] private GameObject _lightHelmet;
    [SerializeField] private Sprite[] runSprites;
    [SerializeField] private float _animationSpeed = 0.1f;
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform[] _switchTransformsInRotation;
    
    private SpriteRenderer _spriteRenderer;
    private int _currentFrame;
    private float _animationTimer;
    private PlayerInputHandler _playerInputHandler;
    private PlayerMovement _playerMovement;
    private Rigidbody2D _rb;
    private GrapplingHook _grapplingHook;
    private Vector3[] _initialLocalPositions;

    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _grapplingHook = GetComponentInChildren<GrapplingHook>();
        _playerInputHandler = GetComponent<PlayerInputHandler>();
        _rb = GetComponent<Rigidbody2D>();
        _playerMovement = GetComponent<PlayerMovement>();
        
        _initialLocalPositions = new Vector3[_switchTransformsInRotation.Length];

        for (int i = 0; i < _switchTransformsInRotation.Length; i++)
        {
            _initialLocalPositions[i] = _switchTransformsInRotation[i].localPosition;
        }
    }

    private void Update()
    {
        HandleSpriteFlip();
        HandleRunAnimation();
        HandlePlayerAnimation();
    }

    private void HandleSpriteFlip()
    {
        if (Time.timeScale == 0 || _playerMovement.IsWallClimb()) return;

        if (_playerInputHandler.MoveInput.x > 0 && _spriteRenderer.flipX)
        {
            _spriteRenderer.flipX = false;
            _lightHelmet.transform.rotation = Quaternion.Euler(0, 0, -90);
            
            for (int i = 0; i < _switchTransformsInRotation.Length; i++)
            {
                var originalPos = _initialLocalPositions[i];

                _switchTransformsInRotation[i].localPosition = originalPos;
                _switchTransformsInRotation[i].localRotation = Quaternion.Euler(0, 0, _switchTransformsInRotation[i].localPosition.z);
            }
        }
        else if (_playerInputHandler.MoveInput.x < 0 && !_spriteRenderer.flipX)
        {
            _spriteRenderer.flipX = true;
            _lightHelmet.transform.rotation = Quaternion.Euler(0, -180, -90);
            
            for (int i = 0; i < _switchTransformsInRotation.Length; i++)
            {
                var originalPos = _initialLocalPositions[i];

                _switchTransformsInRotation[i].localPosition = new Vector3(-originalPos.x, originalPos.y, originalPos.z);
                _switchTransformsInRotation[i].localRotation = Quaternion.Euler(0, 180, _switchTransformsInRotation[i].localPosition.z);
            }
        }
    }

    private void HandleRunAnimation()
    {
        if (!PlayerMovement._isGrounded || Mathf.Abs(_playerInputHandler.MoveInput.x) < 0.01f)
        {
            _currentFrame = 0;
            _spriteRenderer.sprite = runSprites[_currentFrame]; // idle = frame 0
            return;
        }

        _animationTimer += Time.deltaTime;
        if (!(_animationTimer >= _animationSpeed)) return;
        
        _animationTimer = 0f;
        _currentFrame = (_currentFrame + 1) % runSprites.Length;
        _spriteRenderer.sprite = runSprites[_currentFrame];
    }
    private void HandlePlayerAnimation()
    {
        _animator.SetFloat("Speed", Mathf.Abs(_playerInputHandler.MoveInput.x));
        _animator.SetBool("IsJumping", !PlayerMovement._isGrounded && !_playerMovement.IsWallClimb() && !_grapplingHook._isGrappled);
        
        _animator.SetBool("IsClimbing", _playerMovement.IsWallClimb());
        
        if (_animator.GetBool("IsClimbing"))
        {
            float climbSpeed = Mathf.Abs(_rb.linearVelocity.y);
            _animator.SetFloat("ClimbSpeed", climbSpeed);
        }
        
        _animator.SetBool("IsGrappling", _grapplingHook._isGrappled);
    }
}
