using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrapplingHook : MonoBehaviour
{
    public bool _isHookUnlock;
    private LineRenderer _lineRenderer;
    private SpringJoint2D _springJoint;
    public Transform _target;
    public float hookTime;

    [SerializeField] private GameObject _grapplingHookProjectile;

    public bool _isGrappled;
    private bool _canShoot = true;

    [SerializeField] private Transform firePoint;
    public float hookSpeed = 20f;
    public float maxDistance = 10f;

    private GameObject _currentProjectile;
    private GrapplerProjectile _projectileScript;

    private Coroutine _detachCoroutine;
    private AudioManager _audioManager;
    private PlayerMovement _playerMovement;
    
    [SerializeField] private float _climbSpeed = 2f;

    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _springJoint = transform.parent.GetComponent<SpringJoint2D>();
        _playerMovement = GetComponentInParent<PlayerMovement>();
        _lineRenderer.enabled = false;
        _springJoint.enabled = false;

        GrapplerProjectile.OnProjectileReturned += OnProjectileReturned;
        GrapplerProjectile.OnDetachGrapplingHook += DetachGrapplingHook;
        GrapplerProjectile.OnSurfaceTouched += AttachToPoint;
        _audioManager = AudioManager.Instance;
    }

    private void Update()
    {
        if (_currentProjectile)
        {
            _lineRenderer.SetPosition(0, transform.position);
            _lineRenderer.SetPosition(1, _currentProjectile.transform.position);
        }

        //Climb/ Descent
        if (_isGrappled)
        {
            float vertical = _playerMovement.GetVerticalInput();

            if (Mathf.Abs(vertical) > 0.01f)
            {
                float newDistance = _springJoint.distance - vertical * _climbSpeed * Time.deltaTime;

                newDistance = Mathf.Clamp(newDistance, 1f, maxDistance); //Set limite to clibing

                _springJoint.distance = newDistance;

                if (!_currentProjectile)
                {
                    _lineRenderer.SetPosition(0, transform.position);
                    _lineRenderer.SetPosition(1, _springJoint.connectedAnchor);
                }
            }
        }
    }

    public void DestroyProjectile()
    {
        Destroy(_currentProjectile);
        _projectileScript = null;   
        _lineRenderer.enabled = false;
        _springJoint.enabled = false;
        _isGrappled = false;
        _playerMovement.SetIsGrappling(false);
        _playerMovement.OnDetachedFromHook();
        _canShoot = true;
    }


    public void FireGrappler(InputAction.CallbackContext ctx)
    {
        if(!_isHookUnlock)
            return;
        if (!ctx.performed) return;

        if (!_isGrappled && _canShoot)
        {
            _audioManager.PlaySound(AudioType.grapplingHookThrow);
            Vector2 shootDir = _playerMovement.DirectionToVector2(_playerMovement.LastDirection);

            _currentProjectile = Instantiate(_grapplingHookProjectile, firePoint.position, Quaternion.identity);
            _projectileScript = _currentProjectile.GetComponent<GrapplerProjectile>();
            _projectileScript.Initialize(shootDir, hookSpeed, maxDistance, transform, hookTime);

            _lineRenderer.enabled = true;
            _canShoot = false;
        }
        else if (_isGrappled)
        {
            if (_detachCoroutine != null)
            {
                StopCoroutine(_detachCoroutine);
                DetachGrapplingHook();
                _detachCoroutine = null;
            }

            _projectileScript.StartReturn();
        }

    }

    private void AttachToPoint()
    {
        _target.position = _projectileScript.transform.position;

        _springJoint.connectedAnchor = _target.position;
        _springJoint.distance = Vector2.Distance(transform.position, _target.position);
        _springJoint.enabled = true;

        _isGrappled = true;
        _playerMovement.SetIsGrappling(true);
        _audioManager.PlaySound(AudioType.grapplingHookHit);


        if (_detachCoroutine != null) StopCoroutine(_detachCoroutine);
        _detachCoroutine = StartCoroutine(DetachGrapplingHookAfterTime());
    }


    private IEnumerator DetachGrapplingHookAfterTime()
    {
        yield return new WaitForSeconds(hookTime);

        DetachGrapplingHook();

        _detachCoroutine = null;
    }

    public void DetachGrapplingHook()
    {
        if (_detachCoroutine != null)
        {
            StopCoroutine(_detachCoroutine);
            _detachCoroutine = null;
        }

        _springJoint.enabled = false;
        _isGrappled = false;
        _playerMovement.SetIsGrappling(false);

        _playerMovement.OnDetachedFromHook();

        if (_projectileScript)
        {
            _projectileScript.StartReturn();
        }

        _lineRenderer.enabled = false;
    }


    public void OnProjectileReturned()
    {
        _lineRenderer.enabled = false;
        _currentProjectile = null;
        _projectileScript = null;
        _canShoot = true;
    }
}
