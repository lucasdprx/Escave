using UnityEngine;

public class GrapplerProjectile : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Vector2 _direction;
    private float _speed;
    private float _maxDistance;
    private Vector2 _startPosition;
    private bool _isReturning = false;
    
    public LayerMask layerMask;

    private GrapplingHook _grapplingHook;
    private bool _isAttached = false;

    private GameObject _currentHookedObject;
    private bool _isOnEnemy;

    public void Initialize(Vector2 dir, float speed, float maxDist, GrapplingHook hook)
    {
        _rb = GetComponent<Rigidbody2D>();
        _direction = dir.normalized;
        _speed = speed;
        _maxDistance = maxDist;
        _grapplingHook = hook;
        _startPosition = transform.position;

        _rb.linearVelocity = _direction * _speed;
    }

    private void Update()
    {
        if (_isReturning)
        {
            _currentHookedObject = null;
            Vector2 toPlayer = (_grapplingHook.transform.position - transform.position).normalized;
            _rb.linearVelocity = toPlayer * _speed;

            if (Vector2.Distance(transform.position, _grapplingHook.transform.position) < 0.75f)
            {
                _grapplingHook.OnProjectileReturned();
                Destroy(gameObject);
            }
        }
        else if (!_isAttached)
        {
            float traveled = Vector2.Distance(_startPosition, transform.position);
            if (traveled >= _maxDistance)
            {
                StartReturn();
            }
        }

        if (_isAttached && _isOnEnemy)
        {
            if(_currentHookedObject != null)
                transform.position = _currentHookedObject.transform.position;
        }
    }

    public void StartReturn()
    {
        if (_isReturning) return;

        _isOnEnemy = false;
        _isReturning = true;
        _rb.bodyType = RigidbodyType2D.Dynamic;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isReturning || _isAttached) return;
        if (collision.CompareTag("Player") || IsPartOfTrap(collision.transform)) return;
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") ||
            collision.gameObject.layer == LayerMask.NameToLayer("OneWayPlatform") || collision.CompareTag("Enemy"))
        {
            print("OnGround");
            
            SurfaceInfo surface = collision.GetComponent<SurfaceInfo>();
            if (surface != null)
            {
                if (collision.CompareTag("Enemy"))
                    _isOnEnemy = true;
                _currentHookedObject = collision.gameObject;
                
                _rb.linearVelocity = Vector2.zero;
                _rb.bodyType = RigidbodyType2D.Kinematic;

                transform.position = collision.ClosestPoint(transform.position);
                _isAttached = true;

                _grapplingHook.hookTime = surface.HookSurfaceTime;
                _grapplingHook.AttachToPoint(transform.position);
            }
            else
            {
                StartReturn();
            }
        }
    }

    private bool IsPartOfTrap(Transform obj)
    {
        Transform current = obj;

        while (current != null)
        {
            if (current.CompareTag("Trap"))
                return true;

            current = current.parent;
        }

        return false;
    }

}
