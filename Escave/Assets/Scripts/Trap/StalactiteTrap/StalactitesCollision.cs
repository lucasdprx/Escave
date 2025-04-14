using System.Collections;
using UnityEngine;

public class StalactitesCollision : MonoBehaviour
{
    [SerializeField] private Animator stalactitesRespawnAnimation;
    [SerializeField] private TriggerBox triggerBox;

    private Rigidbody2D rb;
    private PlayerDeath playerDeathScript;
    private Vector2 initSpawnPoint;
    private Transform _transform;

    [HideInInspector] public bool isStarted;
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
            collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Trap"))
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
        //ResetSike();
        yield return new WaitForSeconds(1f);
        stalactitesRespawnAnimation.SetBool("IsEnter", false);
        isStarted = false;

        if (triggerBox != null && triggerBox.IsPlayerStillInside())
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

    private void ResetSike()
    {
        rb.gravityScale = 0f;
        rb.linearVelocity = Vector2.zero;
        stalactitesRespawnAnimation.SetBool("IsEnter", true);
    }
}
