using UnityEngine;
using UnityEngine.InputSystem;

public class Grappler : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    public SpringJoint2D _springJoint;
    public Transform target;
    
    //player movement changes during grappling
    private PlayerMovement _playerMovement;
    private Rigidbody2D _rb;
    private float _originalSpeed;
    private float _originalGravityScale;

    public float newSpeed;
    public float newGravityScale;

    public bool _isGrappled = false;

    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _springJoint = transform.parent.GetComponent<SpringJoint2D>();

        _playerMovement = transform.parent.GetComponent<PlayerMovement>();
        _rb = transform.parent.GetComponent<Rigidbody2D>();

        _originalSpeed = _playerMovement.moveSpeed;
        _originalGravityScale = _rb.gravityScale;

        _lineRenderer.enabled = false;
        _springJoint.enabled = false;

    }

    private void Update()
    {
        if (_isGrappled)
        {
            _lineRenderer.SetPosition(0, target.position);
            _lineRenderer.SetPosition(1, transform.position);
        }
    }

    public void FireGrappler(InputAction.CallbackContext ctxx)
    {
        if(ctxx.performed)
        {
            if(!_isGrappled)
            {
                _springJoint.connectedAnchor = target.position;

                //_springJoint.autoConfigureDistance = false;
                _springJoint.distance = Vector2.Distance(transform.position, target.position);
                //_springJoint.dampingRatio = 0f;
                //_springJoint.frequency = 1.5f;

                _springJoint.enabled = true;
                _lineRenderer.enabled = true;

                _isGrappled = true;
            }
            else
            {
                _springJoint.enabled = false;
                _lineRenderer.enabled = false;

                _isGrappled = false;
            }

        }
    }

}
