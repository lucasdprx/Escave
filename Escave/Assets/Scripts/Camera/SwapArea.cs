using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwapArea : MonoBehaviour
{
    private int levelIndex;
    
    [SerializeField] private Direction _switchAreaDirection;
    [SerializeField] private GameObject _downLeftCheckpoint;
    [SerializeField] private GameObject _upRightCheckpoint;
    [SerializeField] private GameObject _levelDownLeft;
    [SerializeField] private GameObject _levelUpRight;
    private GameObject _levelToUnload;
    
    private PlayerDeath _playerDeath;
    private AudioManager _audioManager;

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
    
    private Vector3 _newCameraPosition = Vector3.zero;
    private Camera _camera;
    private Vector2 _playerDir;

    private void Start()
    {
        levelIndex = SceneManager.GetActiveScene().buildIndex - 1;
        _camera = Camera.main;
        _targetCameraPosition.position = _camera.transform.position;
        _audioManager = AudioManager.Instance;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(!other.CompareTag("Player"))
            return;
        
        Vector2 _player = (Vector2)transform.position - (Vector2)other.gameObject.transform.position;
        if (Mathf.Approximately(Mathf.Sign(_playerDir.x), Mathf.Sign(_player.x)) && 
            Mathf.Approximately(Mathf.Sign(_playerDir.y), Mathf.Sign(_player.y)))
            return;
        
        if (_switchAreaDirection == Direction.Horizontal)
        {
            if (Mathf.Approximately(Mathf.Sign(_playerDir.x), Mathf.Sign(_player.x)))
                return;
        }
        else if (_switchAreaDirection == Direction.Vertical)
        {
            if (Mathf.Approximately(Mathf.Sign(_playerDir.y), Mathf.Sign(_player.y)))
                return;
        }
        
        _playerDeath = other.GetComponent<PlayerDeath>();
        FindDirection(other.gameObject.transform.position);
        StartCoroutine(MoveCameraCoroutine());
        _audioManager.PlaySound(AudioType.areaTransition);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.CompareTag("Player"))
            return;
        
        _playerDir = (Vector2)transform.position - (Vector2)other.gameObject.transform.position;
    }

    private void FindDirection(Vector2 _playerTransform)
    {
        Vector2 _playerPos = (Vector2)transform.position - _playerTransform;
        Vector2 _direction = Vector2.zero;

        switch (_switchAreaDirection)
        {
            case Direction.Horizontal :
                _direction.x = Mathf.Sign(-_playerPos.x);
                if (_direction.x < 0)
                {
                    _playerDeath.SetCheckpoint(_downLeftCheckpoint);
                    _levelToUnload = _levelUpRight;
                    _levelDownLeft.SetActive(true);
                }
                else if (_direction.x > 0)
                {
                    _playerDeath.SetCheckpoint(_upRightCheckpoint);
                    _levelUpRight.SetActive(true);
                    _levelToUnload = _levelDownLeft;
                }
                _targetCameraPosition.position += new Vector3(_direction.x * _cameraScale.x,0,0);
                break;
            case Direction.Vertical :
                _direction.y = Mathf.Sign(-_playerPos.y);
                if (_direction.y < 0)
                {
                    _playerDeath.SetCheckpoint(_downLeftCheckpoint);
                    _levelToUnload = _levelUpRight;
                    _levelDownLeft.SetActive(true);
                }
                else if (_direction.y > 0)
                {
                    _playerDeath.SetCheckpoint(_upRightCheckpoint);
                    _levelUpRight.SetActive(true);
                    _levelToUnload = _levelDownLeft;
                }
                _targetCameraPosition.position += new Vector3(0,_direction.y * _cameraScale.y,0);
                break;
        }

        if (DataPersistenceManager.instance.gameData.cameraPos.Count <= levelIndex)
        {
            DataPersistenceManager.instance.gameData.cameraPos.Add(_targetCameraPosition.position);
        }else
            DataPersistenceManager.instance.gameData.cameraPos[levelIndex] = _targetCameraPosition.position;
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
        _levelToUnload.SetActive(false);
        _actualCameraTransitionTime = 0;
        StopCoroutine(MoveCameraCoroutine());
    }
}
