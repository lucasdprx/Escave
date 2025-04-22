using UnityEngine;

public class EnemyJumpBoost : MonoBehaviour
{
    [SerializeField] private float _jumpPower;
    private AudioManager _audioManager;

    private void Start()
    {
        _audioManager = AudioManager.Instance;
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.rigidbody.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
            _audioManager.PlaySound(AudioType.ratBounce);
        }
    }
}
