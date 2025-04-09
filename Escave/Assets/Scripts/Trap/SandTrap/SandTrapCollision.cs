using UnityEngine;

public class SandTrapCollision : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float sinkDelay = 3f;
    [SerializeField] private float sinkingSpeed = 0.5f;
    [SerializeField] private float slowMultiplier = 0.2f;
    [SerializeField] private float sinkingDurationBeforeDeath = 5f;

    private bool isPlayerInTrap = false;
    private bool isPlayerSinking = false;
    private float timer = 0f;
    private float sinkingTimer = 0f;

    private GameObject player;
    private Rigidbody2D playerRb;
    private PlayerMovement playerMovement;
    private PlayerDeath playerDeath;
    private float originalSpeed;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            player = collision.gameObject;
            playerRb = player.GetComponent<Rigidbody2D>();
            playerMovement = player.GetComponent<PlayerMovement>();
            playerDeath = player.GetComponent<PlayerDeath>();

            if (playerMovement != null)
            {
                originalSpeed = playerMovement.moveSpeed;
            }
  
            isPlayerInTrap = true;
            timer = 0f;
            sinkingTimer = 0f;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            ResetTrap();
        }
    }

    private void Update()
    {
        if (isPlayerInTrap && !isPlayerSinking)
        {
            timer += Time.deltaTime;

            if (timer >= sinkDelay)
            {
                StartSinking();
            }
        }

        if (isPlayerSinking && player != null)
        {
            SinkPlayer();

            sinkingTimer += Time.deltaTime;
            if (sinkingTimer >= sinkingDurationBeforeDeath)
            {
                KillPlayer();
            }
        }
    }

    private void StartSinking()
    {
        isPlayerSinking = true;

        if (playerMovement != null)
            playerMovement.moveSpeed *= slowMultiplier;

        if (playerRb != null)
        {
            playerRb.gravityScale = 0f;
            playerRb.linearVelocity = Vector2.zero;
            playerRb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
        }
    }

    private void SinkPlayer()
    {
        player.transform.position += Vector3.down * sinkingSpeed * Time.deltaTime;
    }

    private void KillPlayer()
    {
        if (playerDeath != null)
        {
            playerDeath.PlayerDie();
        }

        ResetTrap();
    }

    private void ResetTrap()
    {
        isPlayerInTrap = false;
        isPlayerSinking = false;
        timer = 0f;
        sinkingTimer = 0f;

        if (playerMovement != null)
            playerMovement.moveSpeed = originalSpeed;

        if (playerRb != null)
        {
            playerRb.gravityScale = 1f;
            playerRb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        player = null;
        playerRb = null;
        playerMovement = null;
        playerDeath = null;
    }
}
