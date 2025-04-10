using UnityEngine;
public class PatrolEnemy : MonoBehaviour
{
    [SerializeField] private float _idleSpeed;
    [SerializeField] private GameObject _firstPoint;
    [SerializeField] private GameObject _secondPoint;
    [SerializeField] private float _approximateDistance = 0.5f;
    [SerializeField] private float _enemyDistanceTrigger;
    private Transform _targetPoint;
    private float _actualSpeed;

    private void Start()
    {
        _targetPoint = _secondPoint.transform;
        _actualSpeed = _idleSpeed;
    }

    private void Update()
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

        transform.position = Vector2.MoveTowards(transform.position, _targetPoint.position, _actualSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _actualSpeed = 4;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _actualSpeed = _idleSpeed;
        }
    }
}
