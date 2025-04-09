using System;
using System.Collections;
using UnityEngine;

public class StalactitesCollision : MonoBehaviour
{
    [SerializeField] private Animator stalactitesRespawnAnimation;
    
    private Rigidbody2D rb;
    private PlayerDeath playerDeathScript;
    private Vector2 initSpawnPoint;
    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    private void Start()
    {
        initSpawnPoint = _transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        print(collision);
        print(collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Player"))
        {
            playerDeathScript = collision.gameObject.GetComponent<PlayerDeath>();
            playerDeathScript.PlayerDie();
            ResetSpike();
        }
        if (collision.gameObject.CompareTag("Ground"))
        {
            ResetSpike();
        }
    }
    private void ResetSpike()
    {
        _transform.position = initSpawnPoint;
        rb.gravityScale = 0f;
        rb.linearVelocity = Vector2.zero;
        gameObject.SetActive(false);
        transform.localScale = Vector3.zero;
    }

    public IEnumerator StartTrap()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
        rb.gravityScale = 0f;
        transform.localScale = Vector3.zero;
        gameObject.SetActive(true);
        stalactitesRespawnAnimation.SetBool("IsEnter", true);
        yield return new WaitForSeconds(1);
        stalactitesRespawnAnimation.SetBool("IsEnter", false);
        rb.gravityScale = 1f;
    }
}
