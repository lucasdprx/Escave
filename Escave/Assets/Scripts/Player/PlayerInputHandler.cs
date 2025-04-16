using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private Vector2 _moveInput;
    private bool _jumpPressed;
    private bool _jumpHeld;
    private bool _jumpReleased;
    private float _verticalInput;

    public Vector2 MoveInput => _moveInput;
    public float VerticalInput => _verticalInput;

    public bool JumpPressed => _jumpPressed;
    public bool JumpHeld => _jumpHeld;
    public bool JumpReleased => _jumpReleased;

    public event Action OnJumpPressed;
    public event Action OnJumpReleased;
    public event Action OnMove;
    

    private void LateUpdate()
    {
        _jumpPressed = false;
        _jumpReleased = false;
    }

    public void Move(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
        _verticalInput = _moveInput.y;
        OnMove?.Invoke();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _jumpPressed = true;
            _jumpHeld = true;
            OnJumpPressed?.Invoke();
        }
        else if (context.canceled)
        {
            _jumpHeld = false;
            _jumpReleased = true;
            OnJumpReleased?.Invoke();
        }
    }
}
