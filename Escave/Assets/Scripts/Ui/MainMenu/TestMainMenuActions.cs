using UnityEngine;
using UnityEngine.InputSystem;

public class TestMainMenuActions : MonoBehaviour
{
    public void Move(InputAction.CallbackContext _ctx)
    {
        if (_ctx.started)
        {
            float _moveVector = _ctx.ReadValue<float>();
            Debug.Log(_moveVector);
        }
    }
    
    public void Look(InputAction.CallbackContext _ctx)
    {
        if (_ctx.started)
        {
            float _moveVector = _ctx.ReadValue<float>();
            Debug.Log(_moveVector);
        }
    }

    public void Jump(InputAction.CallbackContext _ctx)
    {
        if (_ctx.started)
        {
            Debug.Log("Jump");
        }
    }

    public void Hook(InputAction.CallbackContext _ctx)
    {
        if (_ctx.started)
        {
            Debug.Log("Hook");
        }
    }
}
