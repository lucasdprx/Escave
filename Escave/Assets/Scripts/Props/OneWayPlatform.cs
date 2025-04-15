using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    private PlatformEffector2D _platformEffector;
    private PlayerOneWayThroughPlatform _playerOneWayThroughPlatform;

    private void Start()
    {
        _platformEffector = GetComponent<PlatformEffector2D>();
    }

    private void OnCollisionEnter2D(Collision2D _other)
    {
        if (_other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _playerOneWayThroughPlatform = _other.gameObject.GetComponent<PlayerOneWayThroughPlatform>();
        }
    }

    private void Update()
    {
        if (!_playerOneWayThroughPlatform || !_playerOneWayThroughPlatform.isGoingDown) return;

        _platformEffector.rotationalOffset = 180;
        _playerOneWayThroughPlatform.isGoingDown = false;
        _playerOneWayThroughPlatform = null;
    }

    private void OnCollisionExit2D(Collision2D _other)
    {
        if (_other.gameObject.layer != LayerMask.NameToLayer("Player")) return;
        
        if(_playerOneWayThroughPlatform != null)
            _playerOneWayThroughPlatform.isGoingDown = false;
            
        _playerOneWayThroughPlatform = null;
        _platformEffector.rotationalOffset = 0;
    }
}