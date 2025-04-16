using System.Collections;
using UnityEngine;

public class StalactitesCollision : MonoBehaviour
{
    [SerializeField] private Animator stalactitesRespawnAnimation;

    private Rigidbody2D rb;
    private PlayerDeath playerDeathScript;
    private Vector2 initSpawnPoint;
    private Transform _transform;
    private bool _isPlayerInside;
    private bool isStarted;
    private AudioManager _audioManager;

    private void Awake()
    {
        _transform = transform;
    }

    private void Start()
    {
        initSpawnPoint = _transform.position;
        rb = GetComponent<Rigidbody2D>();
        _audioManager = AudioManager.Instance;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerDeathScript = collision.gameObject.GetComponent<PlayerDeath>();
            playerDeathScript.PlayerDie();
            StartCoroutine(ResetSpike());
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") ||
            collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Trap")
            || collision.gameObject.CompareTag("IgnoreGrappling"))
        {
            StartCoroutine(ResetSpike());
        }
    }

    private IEnumerator ResetSpike()
    {
        _audioManager.PlaySound(AudioType.stalactiteRegrow);
        _transform.position = initSpawnPoint;
        rb.gravityScale = 0f;
        rb.linearVelocity = Vector2.zero;
        stalactitesRespawnAnimation.SetBool("IsEnter", true);
        yield return new WaitForSeconds(1f);
        stalactitesRespawnAnimation.SetBool("IsEnter", false);
        isStarted = false;

        if (_isPlayerInside)
        {
            StartTrap();
        }
    }

    public void StartTrap()
    {
        _audioManager.PlaySound(AudioType.stalactiteFall);
        isStarted = true;
        rb.gravityScale = 1f;
    }
    public void SetIsPlayerInside(bool isPlayerInside)
    {
        _isPlayerInside = isPlayerInside;
    }
    public bool GetIsStarted()
    {
        return isStarted;
    }
}
