using System;
using UnityEngine;

public class SandTrapCollision : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float sinkDelay = 1f;
    [SerializeField] private float gravityScale = 0.1f;
    [SerializeField] private float slowScale = 0.5f;
    [SerializeField] private float sinkingDurationBeforeDeath = 3f;
    
    private bool isSinking;
    private float enterTimer;
    private float sinkingTimer;
    private Collider2D trapCollider;
    private PlayerMovement playerMovement;
    [SerializeField] private PlayerDeath playerDeath;
    private Rigidbody2D playerRigidbody;
    private float initGravityScale;
    private float initMoveSpeed;
    private float initJumpForce;
    private void Awake()
    {
        trapCollider = GetComponent<Collider2D>();
        
        playerDeath.OnDeath2 += Reset;
    }

    private void Reset(object _sender, EventArgs _e)
    {
        sinkingTimer = 0f;
        trapCollider.isTrigger = false;
        playerRigidbody.gravityScale = initGravityScale;
        playerMovement.moveSpeed = initMoveSpeed;
        playerMovement.jumpForce = initJumpForce;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        
        if (!playerMovement)
        {
            playerMovement = other.gameObject.GetComponent<PlayerMovement>();
            initMoveSpeed = playerMovement.moveSpeed;
            initJumpForce = playerMovement.jumpForce;
        }
        if (!playerDeath)
        {
            playerDeath = other.gameObject.GetComponent<PlayerDeath>();
        }
        if (!playerRigidbody)
        {
            playerRigidbody = other.gameObject.GetComponent<Rigidbody2D>();
            initGravityScale = playerRigidbody.gravityScale;
        }
        
        isSinking = true;
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        
        isSinking = false;
        enterTimer = 0f;
    }
    private void Update()
    {
        if (!isSinking) return;
            
        enterTimer += Time.deltaTime;
        if (enterTimer >= sinkDelay)
        {
            enterTimer = 0f;
            trapCollider.isTrigger = true;
            isSinking = false;
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        
        playerRigidbody.gravityScale = gravityScale;
        playerMovement.moveSpeed = slowScale;
        playerMovement.jumpForce = 0;
        sinkingTimer += Time.deltaTime;
        if (sinkingTimer >= sinkingDurationBeforeDeath)
        {
            sinkingTimer = 0f;
            trapCollider.isTrigger = false;
            playerDeath.PlayerDie();
            playerRigidbody.gravityScale = initGravityScale;
            playerMovement.moveSpeed = initMoveSpeed;
            playerMovement.jumpForce = initJumpForce;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        
        sinkingTimer = 0f;
        trapCollider.isTrigger = false;
        playerRigidbody.gravityScale = initGravityScale;
        playerMovement.moveSpeed = initMoveSpeed;
        playerMovement.jumpForce = initJumpForce;
    }
}

    