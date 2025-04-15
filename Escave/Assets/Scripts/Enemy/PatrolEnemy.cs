using System;
using UnityEngine;
public class PatrolEnemy : MonoBehaviour
{
    [SerializeField] private float _idleSpeed;
    [SerializeField] private GameObject _firstPoint;
    [SerializeField] private GameObject _secondPoint;
    [SerializeField] private float _approximateDistance = 0.5f;
    private Transform _targetPoint;
    private bool _isPlayerAhead = false;
    private float _actualSpeed;
    private Vector2 _enemyDirection = new();
    private Vector2 _playerPosition = new();

    private void Start()
    {
        _targetPoint = _secondPoint.transform;
        _actualSpeed = _idleSpeed;
    }

    private void Update()
    {
        if (!_isPlayerAhead)
        {
            if ((_firstPoint.transform.position.x + _approximateDistance >= transform.position.x))
            {
                _targetPoint = _secondPoint.transform;
                transform.localScale = new Vector3(-1, 1, 0);
            }        
            if((_secondPoint.transform.position.x - _approximateDistance <= transform.position.x))
            {
                _targetPoint = _firstPoint.transform;
                transform.localScale = new Vector3(1, 1, 0);
            }
            _enemyDirection = Vector2.MoveTowards(transform.position, _targetPoint.position, _actualSpeed * Time.deltaTime);
            transform.position = new Vector3(_enemyDirection.x,transform.position.y,transform.position.z);
        }
        else
        {
            _enemyDirection = Vector2.MoveTowards(transform.position, _playerPosition, _actualSpeed * Time.deltaTime);
            transform.position = new Vector3(_enemyDirection.x,transform.position.y,transform.position.z);
        }
    }

    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.CompareTag("Player"))
    //     {
    //         _isPlayerAhead = true;
    //         _actualSpeed = 4;
    //     }
    // }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerAhead = true;
            _actualSpeed = 4;
            _playerPosition = other.transform.position;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerAhead = false;
            _actualSpeed = _idleSpeed;
        }
    }
}
