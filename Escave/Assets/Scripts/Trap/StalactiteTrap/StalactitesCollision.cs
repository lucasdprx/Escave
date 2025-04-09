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
        rb = GetComponent<Rigidbody2D>();
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

    public IEnumerator StartStalactite()
    {
        rb.gravityScale = 0f;
        stalactitesRespawnAnimation.SetBool("IsEnter", true);
        yield return null;
        yield return new WaitForSeconds(1);
        stalactitesRespawnAnimation.SetBool("IsEnter", false);
        rb.gravityScale = 1f;
    }
}
