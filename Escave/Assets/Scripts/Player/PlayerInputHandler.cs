using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private Vector2 _moveInput;

    public Vector2 MoveInput
    {
        get { return _moveInput; }
    }
    public float VerticalInput { get; private set; }
    public event Action OnJumpPressed;
    public event Action OnJumpReleased;
    public event Action OnMove;

    public void Move(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
        VerticalInput = _moveInput.y;
        OnMove?.Invoke();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            OnJumpPressed?.Invoke();
        }
        else if (context.canceled)
        {
            OnJumpReleased?.Invoke();
        }
    }
}
