using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerOneWayThroughPlatform : MonoBehaviour
{
    private bool _isOnPlatform;
    public bool isGoingDown;
    private PlatformEffector2D _platformEffector;

    private void OnCollisionEnter2D(Collision2D _other)
    {
        if (_other.gameObject.layer == LayerMask.NameToLayer("OneWayPlatform"))
        {
            _isOnPlatform = true;
            _platformEffector = _other.gameObject.GetComponent<PlatformEffector2D>();
        }
    }

    private void OnCollisionExit2D(Collision2D _other)
    {
        if (_other.gameObject.layer == LayerMask.NameToLayer("OneWayPlatform"))
        {
            _isOnPlatform = false;
            _platformEffector.rotationalOffset = 0;
        }
    }

    public void Look(InputAction.CallbackContext _ctx)
    {
        Vector2 _look = _ctx.ReadValue<Vector2>();
        if (_ctx.performed)
        {
            if (_look.y < -0.95f && _isOnPlatform)
            {
                isGoingDown = true;
                _platformEffector.rotationalOffset = 180;
            }
            else
            {
                isGoingDown = false;
            }
        }
    }
}
