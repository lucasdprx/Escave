using UnityEngine;

public class CheckpointScript : MonoBehaviour
{
    private Animator _animator;
    [SerializeField] private GameObject _light;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _light.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            other.GetComponent<PlayerDeath>().SetCheckpoint(gameObject);
            if (!_animator)
            {
                _animator = GetComponent<Animator>();
            }
            
            _animator.SetBool("Activated", true);
            _light.SetActive(true);
        }
    }
}
