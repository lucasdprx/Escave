using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CheckpointScript : MonoBehaviour
{
    private Animator _animator;
    [SerializeField] private GameObject _light;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        //_light = GetComponentInChildren<Light2D>();
        _light.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _other.GetComponent<PlayerDeath>().SetCheckpoint(gameObject);
            _animator.Play("Activation");
            _animator.SetBool("Activated", true);

            _light.SetActive(true);
        }
    }
}
