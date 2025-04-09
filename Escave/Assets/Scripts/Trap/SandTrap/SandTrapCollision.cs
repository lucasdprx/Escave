using UnityEngine;

public class SandTrapCollision : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float sinkDelay = 3f;
    [SerializeField] private float sinkingSpeed = 0.5f;
    [SerializeField] private float slowFactor = 0.3f;

    private bool isPlayerInTrap = false;
    private bool isPlayerSinking = false;
    private float timer = 0f;

    private GameObject player;
    private PlayerMovement playerMovement;
    private Rigidbody2D playerRb;
    private float originalSpeed;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            EnterTrap(collision.gameObject);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Empêche la sortie tant que le joueur est enfoncé
            if (!isPlayerSinking)
            {
                ExitTrap();
            }
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

        if (isPlayerSinking)
        {
            SinkPlayer();
        }
    }

    // --- Fonctions déléguées ---

    private void EnterTrap(GameObject playerObj)
    {
        player = playerObj;
        playerRb = player.GetComponent<Rigidbody2D>();
        playerMovement = player.GetComponent<PlayerMovement>();

        if (playerMovement != null)
        {
            originalSpeed = playerMovement.moveSpeed;
        }

        isPlayerInTrap = true;
        timer = 0f;

        Debug.Log("Player entered sand trap");
    }

    private void ExitTrap()
    {
        if (playerMovement != null)
        {
            playerMovement.moveSpeed = originalSpeed;
        }

        isPlayerInTrap = false;
        isPlayerSinking = false;
        timer = 0f;

        Debug.Log("Player exited sand trap");
    }

    private void StartSinking()
    {
        isPlayerSinking = true;

        if (playerMovement != null)
        {
            playerMovement.moveSpeed *= slowFactor;
        }

        Debug.Log("Player starts sinking!");
    }

    private void SinkPlayer()
    {
        if (playerRb != null)
        {
            Vector2 newPosition = playerRb.position + Vector2.down * sinkingSpeed * Time.deltaTime;
            playerRb.MovePosition(newPosition);
        }
    }
}