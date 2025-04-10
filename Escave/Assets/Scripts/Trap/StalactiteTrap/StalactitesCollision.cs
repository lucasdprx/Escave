using System.Collections;
using UnityEngine;

public class StalactitesCollision : MonoBehaviour
{
    [SerializeField] private Animator stalactitesRespawnAnimation;
    
    private Rigidbody2D rb;
    private PlayerDeath playerDeathScript;
    private Vector2 initSpawnPoint;
    private Transform _transform;
    
    [HideInInspector] public bool isStarted;

    private void Awake()
    {
        _transform = transform;
    }

    private void Start()
    {
        initSpawnPoint = _transform.position;
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerDeathScript = collision.gameObject.GetComponent<PlayerDeath>();
            playerDeathScript.PlayerDie();
            StartCoroutine(ResetSpike());
        }
        if (collision.gameObject.CompareTag("Ground"))
        {
            StartCoroutine(ResetSpike());
        }
    }
    private IEnumerator ResetSpike()
    {
        _transform.position = initSpawnPoint;
        rb.gravityScale = 0f;
        rb.linearVelocity = Vector2.zero;
        stalactitesRespawnAnimation.SetBool("IsEnter", true);
        yield return new WaitForSeconds(1f);
        stalactitesRespawnAnimation.SetBool("IsEnter", false);
        isStarted = false;
    }

    public void StartTrap()
    {
        isStarted = true;
        rb.gravityScale = 1f;
    }
}
