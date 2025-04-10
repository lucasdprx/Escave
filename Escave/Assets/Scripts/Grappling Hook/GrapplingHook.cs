using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrapplingHook : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    private SpringJoint2D _springJoint;
    public Transform _target;
    public float hookTime;

    [SerializeField] private GameObject _grapplingHookProjectile;

    public bool _isGrappled = false;
    private bool _canShoot = true;

    [SerializeField] private Transform firePoint;
    public float hookSpeed = 20f;
    public float maxDistance = 10f;

    private GameObject _currentProjectile;
    private GrapplerProjectile _projectileScript;

    private Coroutine _detachCoroutine;

    private AudioManager _audioManager;

    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _springJoint = transform.parent.GetComponent<SpringJoint2D>();

        _lineRenderer.enabled = false;
        _springJoint.enabled = false;

        _audioManager = AudioManager.Instance;
    }

    private void Update()
    {
        if (_currentProjectile != null)
        {
            _lineRenderer.SetPosition(0, transform.position);
            _lineRenderer.SetPosition(1, _currentProjectile.transform.position);
        }
    }

    public void FireGrappler(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        if (!_isGrappled && _canShoot)
        {
            _audioManager.PlaySound(AudioType.grapplingHookThrow);
            var playerMovement = transform.parent.GetComponent<PlayerMovement>();
            Vector2 shootDir = playerMovement.DirectionToVector2(playerMovement.LastDirection);

            _currentProjectile = Instantiate(_grapplingHookProjectile, firePoint.position, Quaternion.identity);
            _projectileScript = _currentProjectile.GetComponent<GrapplerProjectile>();
            _projectileScript.Initialize(shootDir, hookSpeed, maxDistance, this);

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

    public void AttachToPoint(Vector2 point)
    {
        _target.position = point;

        _springJoint.connectedAnchor = point;
        _springJoint.distance = Vector2.Distance(transform.position, point);
        _springJoint.enabled = true;

        _isGrappled = true;
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
        _springJoint.enabled = false;
        _isGrappled = false;

        GetComponentInParent<PlayerMovement>()?.OnDetachedFromHook();

        if (_projectileScript != null)
        {
            _projectileScript.StartReturn();
        }
    }


    public void OnProjectileReturned()
    {
        _lineRenderer.enabled = false;
        _currentProjectile = null;
        _projectileScript = null;
        _canShoot = true;
    }
}
