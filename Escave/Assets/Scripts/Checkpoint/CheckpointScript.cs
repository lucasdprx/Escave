using UnityEngine;

public class CheckpointScript : MonoBehaviour
{
    private Animator _animator;
    [SerializeField] private GameObject _light;
    private int _triggerEnterCounter = 0;

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

            if (_triggerEnterCounter == 0)
            {
                AudioManager.Instance.PlaySound(AudioType.checkpointReach);
            }

            _triggerEnterCounter++;
            _animator.SetBool("Activated", true);
            _light.SetActive(true);
        }
    }
}
