using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrapplingHook : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    public SpringJoint2D _springJoint;
    public Transform target;
    public float hookTime;

    [SerializeField] private GameObject _grapplingHookProjectile;

    public bool _isGrappled = false;
    public bool canShoot = true;

    public Transform firePoint;
    public float hookSpeed = 20f;
    public float maxDistance = 10f;

    public GameObject _currentProjectile;
    private GrapplerProjectile _projectileScript;

    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _springJoint = transform.parent.GetComponent<SpringJoint2D>();

        _lineRenderer.enabled = false;
        _springJoint.enabled = false;
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

        if (!_isGrappled && canShoot)
        {
            var playerMovement = transform.parent.GetComponent<PlayerMovement>();
            Vector2 shootDir = playerMovement.DirectionToVector2(playerMovement.LastDirection);

            _currentProjectile = Instantiate(_grapplingHookProjectile, firePoint.position, Quaternion.identity);
            _projectileScript = _currentProjectile.GetComponent<GrapplerProjectile>();
            _projectileScript.Initialize(shootDir, hookSpeed, maxDistance, this);

            _lineRenderer.enabled = true;
            canShoot = false;
        }
        else if (_isGrappled)
        {
            DetachGrapplingHook();
        }
    }

    public void AttachToPoint(Vector2 point)
    {
        target.position = point;
        _springJoint.connectedAnchor = point;
        _springJoint.distance = Vector2.Distance(transform.position, point);
        _springJoint.enabled = true;

        _isGrappled = true;

        StartCoroutine(AutoDetachAfterTime());
    }

    private IEnumerator AutoDetachAfterTime()
    {
        yield return new WaitForSeconds(hookTime);
        DetachGrapplingHook();
    }

    public void DetachGrapplingHook()
    {
        _springJoint.enabled = false;
        _isGrappled = false;

        if (_projectileScript != null)
        {
            _projectileScript.StartReturn(); // Hook visuel revient au joueur
        }
    }

    public void OnProjectileReturned()
    {
        _lineRenderer.enabled = false;
        _currentProjectile = null;
        _projectileScript = null;
        canShoot = true;
    }
}
