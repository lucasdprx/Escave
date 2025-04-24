using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerOneWayThroughPlatform : MonoBehaviour
{
    private bool _isOnPlatform;
    private PlatformEffector2D _platformEffector;
    private const float _platformRayLength = 0.95f;

    private void OnCollisionEnter2D(Collision2D _other)
    {
        if (_other.gameObject.layer != LayerMask.NameToLayer("OneWayPlatform"))
            return;
        
        _isOnPlatform = true;
        _platformEffector = _other.gameObject.GetComponent<PlatformEffector2D>();
    }

    private void OnCollisionExit2D(Collision2D _other)
    {
        if (_other.gameObject.layer != LayerMask.NameToLayer("OneWayPlatform"))
            return;
        
        _isOnPlatform = false;
        _platformEffector.rotationalOffset = 0;
    }

    public void Look(InputAction.CallbackContext _ctx)
    {
        if (!_ctx.performed)
            return;
        
        Vector2 _look = _ctx.ReadValue<Vector2>();
        
        if (_look.y < -_platformRayLength && _isOnPlatform)
        {
            _platformEffector.rotationalOffset = 180;
        }
    }
}
