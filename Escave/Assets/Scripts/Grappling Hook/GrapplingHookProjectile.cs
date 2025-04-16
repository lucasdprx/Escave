using System;
using UnityEngine;

public class GrapplerProjectile : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Vector2 _direction;
    private float _speed;
    private float _maxDistance;
    private Vector2 _startPosition;
    private bool _isReturning;
    private float _hookTime;
    
    public LayerMask layerMask;
    private bool _isAttached;

    private GameObject _currentHookedObject;
    private bool _isOnEnemy;
    private BreakingPlatform _currentBreakingPlatform;
    private Transform _grapplingHookTransform;

    public static event Action OnProjectileReturned;
    public static event Action OnDetachGrapplingHook;
    public static event Action OnSurfaceTouched;

    public void Initialize(Vector2 dir, float speed, float maxDist, Transform grapplingHook, float hookTime)
    {
        _rb = GetComponent<Rigidbody2D>();
        _direction = dir.normalized;
        _speed = speed;
        _hookTime = hookTime;
        _maxDistance = maxDist;
        _startPosition = transform.position;
        _grapplingHookTransform = grapplingHook;
        _rb.linearVelocity = _direction * _speed;
    }

    private void Update()
    {
        if (_isReturning)
        {
            _currentHookedObject = null;
            Vector2 toPlayer = (_grapplingHookTransform.transform.position - transform.position).normalized;
            _rb.linearVelocity = toPlayer * _speed;

            if (Vector2.Distance(transform.position, _grapplingHookTransform.transform.position) < 0.75f)
            {
                OnProjectileReturned?.Invoke();
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
            if(_currentHookedObject)
                transform.position = _currentHookedObject.transform.position;
        }
    }

    public void StartReturn()
    {
        if (_isReturning) return;

        _isOnEnemy = false;
        _isReturning = true;
        _rb.bodyType = RigidbodyType2D.Dynamic;

        if (_currentBreakingPlatform)
        {
            _currentBreakingPlatform.OnBroken -= DetachGrapplingHook;
            _currentBreakingPlatform = null;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isReturning || _isAttached) return;

        if (collision.CompareTag("Player") || collision.CompareTag("IgnoreGrappling")) return; //pass through object

        if (IsPartOfTrap(collision.transform)) StartReturn(); //grappling come back to player

        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") ||
            collision.gameObject.layer == LayerMask.NameToLayer("OneWayPlatform") || collision.CompareTag("Enemy") || 
            collision.gameObject.layer == LayerMask.NameToLayer("UnclimbableWall")) //Attach to object
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

                _hookTime = surface.HookSurfaceTime;
                OnSurfaceTouched?.Invoke();

                //set is grapple to surface 
                if(collision.CompareTag("BreakingPlateform"))
                {
                    BreakingPlatform platform = collision.GetComponent<BreakingPlatform>();
                    if (platform != null)
                    {
                        _currentBreakingPlatform = platform;
                        _currentBreakingPlatform.OnBroken += DetachGrapplingHook;
                    }

                }
            }
            else
            {
                StartReturn();
            }
        }
    }
    private void DetachGrapplingHook()
    {
        OnDetachGrapplingHook?.Invoke();
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
