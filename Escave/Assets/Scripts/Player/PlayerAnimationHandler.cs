using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour
{
    [Header("Sprite Animation")]
    [SerializeField] private GameObject _lightHelmet;
    [SerializeField] private Sprite[] runSprites;
    [SerializeField] private float _animationSpeed = 0.1f;
    
    private SpriteRenderer _spriteRenderer;
    private int _currentFrame;
    private float _animationTimer;
    private PlayerInputHandler _playerInputHandler;

    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _playerInputHandler = GetComponent<PlayerInputHandler>();
    }

    private void Update()
    {
        HandleSpriteFlip();
        HandleRunAnimation();
    }

    private void HandleSpriteFlip()
    {
        if (Time.timeScale == 0) return;

        if (_playerInputHandler.MoveInput.x > 0 && _spriteRenderer.flipX)
        {
            _spriteRenderer.flipX = false;
            _lightHelmet.transform.rotation = Quaternion.Euler(0, 0, -90);
        }
        else if (_playerInputHandler.MoveInput.x < 0 && !_spriteRenderer.flipX)
        {
            _spriteRenderer.flipX = true;
            _lightHelmet.transform.rotation = Quaternion.Euler(0, -180, -90);
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
}
