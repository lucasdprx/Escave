using UnityEngine;

public class CheckpointScript : MonoBehaviour
{
    private Animator _animator;
    [SerializeField] private GameObject _light;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _light.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            other.GetComponent<PlayerDeath>().SetCheckpoint(gameObject);
            _animator.Play("Activation");
            _animator.SetBool("Activated", true);

            _light.SetActive(true);
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (!_light.activeSelf) return;
        
        other.GetComponent<PlayerDeath>().SetCheckpoint(gameObject);
        _animator.Play("Activation");
        _animator.SetBool("Activated", true);

        _light.SetActive(true);
    }
}
