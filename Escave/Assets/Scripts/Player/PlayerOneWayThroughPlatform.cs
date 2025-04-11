using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerOneWayThroughPlatform : MonoBehaviour
{
    private bool _isLookingDown;
    private bool _isOnPlatform;
    public bool isGoingDown;

    private void OnCollisionEnter2D(Collision2D _other)
    {
        if(_other.gameObject.layer == LayerMask.NameToLayer("OneWayPlatform"))
            _isOnPlatform = true;
    }

    private void OnCollisionExit2D(Collision2D _other)
    {
        if(_other.gameObject.layer == LayerMask.NameToLayer("OneWayPlatform"))
            _isOnPlatform = false;
    }


    public void Look(InputAction.CallbackContext _ctx)
    {
        Vector2 _look = _ctx.ReadValue<Vector2>();

        if (_look.y < -0.95f)
        {
            _isLookingDown = true;
            print("lookingDown");
        }
        else
        {
            _isLookingDown = false;
            isGoingDown = false;
        }
    }

    public void JumpAction(InputAction.CallbackContext _ctx)
    {
        if (!_isOnPlatform) return;
        
        if (_isLookingDown)
        {
            isGoingDown = true;
        }
    }
}
