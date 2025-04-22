using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour
{
    [Header("Sprite Animation")]
    [SerializeField] private GameObject _lightHelmet;
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform[] _switchTransformsInRotation;

    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private PlayerInputHandler _playerInputHandler;
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private GrapplingHook _grapplingHook;
    private Vector3[] _initialLocalPositions;

    private bool _isFacingRight;

    private void Awake()
    {
        //_spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        //_grapplingHook = GetComponentInChildren<GrapplingHook>();
        //_playerInputHandler = GetComponent<PlayerInputHandler>();
        //_rb = GetComponent<Rigidbody2D>();
        //_playerMovement = GetComponent<PlayerMovement>();
        
        _initialLocalPositions = new Vector3[_switchTransformsInRotation.Length];

        for (int i = 0; i < _switchTransformsInRotation.Length; i++)
        {
            _initialLocalPositions[i] = _switchTransformsInRotation[i].localPosition;
        }
    }

    private void Update()
    {
        CheckForFlipTrigger();
        HandlePlayerAnimation();
    }

    private void CheckForFlipTrigger()
    {
        if (Time.timeScale == 0 || _playerMovement.IsWallClimb()) return;

        float moveX = _playerInputHandler.MoveInput.x;

        if (moveX > 0 && !_isFacingRight)
        {
            _animator.SetTrigger("Flip");
            _isFacingRight = true;
        }
        else if (moveX < 0 && _isFacingRight)
        {
            _animator.SetTrigger("Flip");
            _isFacingRight = false;
        }
    }

    public void HandleSpriteFlip()
    {
        _spriteRenderer.flipX = !_isFacingRight;
        _lightHelmet.transform.rotation = _isFacingRight ? Quaternion.Euler(0, 0, -90) : Quaternion.Euler(0, -180, -90);

        for (int i = 0; i < _switchTransformsInRotation.Length; i++)
        {
            Vector3 originalPos = _initialLocalPositions[i];
            _switchTransformsInRotation[i].localPosition = _isFacingRight ? originalPos : new Vector3(-originalPos.x, originalPos.y, originalPos.z);

            _switchTransformsInRotation[i].localRotation = _isFacingRight ? Quaternion.Euler(0, 0, originalPos.z) : Quaternion.Euler(0, 180, originalPos.z);
        }
    }

    private void HandlePlayerAnimation()
    {
        _animator.SetFloat("Speed", Mathf.Abs(_playerInputHandler.MoveInput.x));
        _animator.SetBool("IsJumping", !PlayerMovement._isGrounded && !_playerMovement.IsWallClimb() && !_grapplingHook._isGrappled);
        _animator.SetBool("IsClimbing", _playerMovement.IsWallClimb());
        _animator.SetBool("IsGrappling", _grapplingHook._isGrappled);

        if (PlayerMovement._isGrounded && Mathf.Abs(_playerInputHandler.MoveInput.x) < 0.01f && _playerInputHandler.MoveInput.y < -0.5f)
        {
            _animator.SetBool("Crouch", true);
        }
        else
            _animator.SetBool("Crouch", false);


        if (_animator.GetBool("IsClimbing"))
        {
            if (_spriteRenderer.flipX == false && _playerMovement._isClimbingFlip == false)
            {
                _spriteRenderer.flipX = true;
                _lightHelmet.transform.rotation = Quaternion.Euler(0, -180, -90);
            }
            float climbSpeed = Mathf.Abs(_rb.linearVelocity.y);
            _animator.SetFloat("ClimbSpeed", climbSpeed);
        }
    }
}
