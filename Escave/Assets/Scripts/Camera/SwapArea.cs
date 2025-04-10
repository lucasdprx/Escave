using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwapArea : MonoBehaviour
{
    [SerializeField] private Direction _switchAreaDirection;

    [SerializeField] private GameObject _downLeftCheckpoint;
    [SerializeField] private GameObject _upRightCheckpoint;
    
    private PlayerDeath _playerDeath;
    private enum Direction
    {
        Horizontal,
        Vertical,
    };
    
    [Header("Switch Area Duration")]
    [SerializeField] private float _switchAreaDuration;
    private float _actualCameraTransitionTime;
    
    [Header("Camera")]
    
    [SerializeField] private Vector2 _cameraScale;
    [SerializeField] private float _cameraTransitionSpeed;
    [SerializeField] private Transform _targetCameraPosition;
    
    private PlayerInput _playerInput;
    private Vector3 _newCameraPosition = Vector3.zero;
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
        _targetCameraPosition.position = _camera.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.CompareTag("Player"))
            return;
        
        _playerInput = other.GetComponent<PlayerInput>();
        _playerDeath = other.GetComponent<PlayerDeath>();
        Vector2 _direction = FindDirection(other.gameObject.transform.position);
        other.transform.position += (Vector3)_direction * 2; 
        //_playerInput.DeactivateInput();
        StartCoroutine(MoveCameraCoroutine());
    }

    private Vector2 FindDirection(Vector2 _playerTransform)
    {
        Vector2 _playerPos = (Vector2)transform.position - _playerTransform;
        Vector2 _direction = Vector2.zero;

        switch (_switchAreaDirection)
        {
            case Direction.Horizontal :
                _direction.x = Mathf.Sign(_playerPos.x);
                if(_direction.x < 0)  _playerDeath.SetCheckpoint(_downLeftCheckpoint);
                if(_direction.x > 0) _playerDeath.SetCheckpoint(_upRightCheckpoint);
                _targetCameraPosition.position += new Vector3(_direction.x * _cameraScale.x,0,0);
                break;
            case Direction.Vertical :
                _direction.y = Mathf.Sign(_playerPos.y);
                if(_direction.y < 0)  _playerDeath.SetCheckpoint(_downLeftCheckpoint);
                if(_direction.y > 0) _playerDeath.SetCheckpoint(_upRightCheckpoint);
                _targetCameraPosition.position += new Vector3(0,_direction.y * _cameraScale.y,0);
                break;
        }
        return _direction;
    }

    private IEnumerator MoveCameraCoroutine()
    {
        while (_actualCameraTransitionTime <= _switchAreaDuration)
        {
            _actualCameraTransitionTime += Time.deltaTime;
            _newCameraPosition = Vector3.Lerp(_camera.transform.position, _targetCameraPosition.position,
                Time.deltaTime * _cameraTransitionSpeed);
            _camera.transform.position = _newCameraPosition;
            yield return null;
        }
        //_playerInput.ActivateInput();
        _actualCameraTransitionTime = 0;
        StopCoroutine(MoveCameraCoroutine());
    }
}
