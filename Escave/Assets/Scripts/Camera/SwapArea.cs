using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class SwapArea : MonoBehaviour
{
    [SerializeField] private Direction _switchAreaDirection;
    private enum Direction
    {
        Horizontal,
        Vertical,
    };
    
    [Header("Switch Area Duration")]
    [SerializeField] private float _switchAreaDuration;
    private float _actualCameraTransitionTime;
    
    [Header("Camera")]
    [SerializeField] private Camera _camera;
    [SerializeField] private Vector2 _cameraScale;
    [SerializeField] private float _cameraTransitionSpeed;
    
    private PlayerInput _playerInput;
    private Vector3 _targetCameraPosition, _newCameraPosition = Vector3.zero;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.CompareTag("Player"))
            return;
        _playerInput = other.GetComponent<PlayerInput>();
        Vector2 _direction = FindDirection(other.gameObject.transform.position);
        other.transform.position += (Vector3)_direction * 2; 
        _playerInput.DeactivateInput();
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
                _targetCameraPosition = new Vector3(_camera.transform.position.x + _direction.x *_cameraScale.x,_camera.transform.position.y,_camera.transform.position.z);
                break;
            case Direction.Vertical :
                _direction.y = Mathf.Sign(_playerPos.y);
                _targetCameraPosition = new Vector3(_camera.transform.position.x,_camera.transform.position.y + _direction.y * _cameraScale.y,_camera.transform.position.z);
                break;
        }
        return _direction;
    }

    private IEnumerator MoveCameraCoroutine()
    {
        while (_actualCameraTransitionTime <= _switchAreaDuration)
        {
            _actualCameraTransitionTime += Time.deltaTime;
            _newCameraPosition = Vector3.Lerp(_camera.transform.position, _targetCameraPosition,
                Time.deltaTime * _cameraTransitionSpeed);
            _camera.transform.position = _newCameraPosition;
            yield return null;
        }
        _playerInput.ActivateInput();
        _actualCameraTransitionTime = 0;
        StopCoroutine(MoveCameraCoroutine());
    }
}
