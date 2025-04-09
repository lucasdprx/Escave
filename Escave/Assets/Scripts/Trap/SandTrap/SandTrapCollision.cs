using UnityEngine;

public class SandTrapCollision : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float sinkDelay = 3f;
    [SerializeField] private float sinkingGravityScale = 0.2f;
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
    private float originalGravityScale;

    private Collider2D trapCollider;

    private void Awake()
    {
        trapCollider = GetComponent<Collider2D>();
        if (trapCollider == null)
            Debug.LogError("Le piège de sable n'a pas de Collider2D !");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            player = collision.gameObject;
            playerRb = player.GetComponent<Rigidbody2D>();
            playerMovement = player.GetComponent<PlayerMovement>();
            playerDeath = player.GetComponent<PlayerDeath>();

            if (playerMovement != null)
                originalSpeed = playerMovement.moveSpeed;

            if (playerRb != null)
                originalGravityScale = playerRb.gravityScale;

            isPlayerInTrap = true;
            timer = 0f;
            sinkingTimer = 0f;

            Debug.Log("Le joueur est entré dans le piège de sable.");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (!isPlayerSinking) // NE reset que si on n’est pas en train de s’enfoncer
            {
                Debug.Log("Le joueur est sorti du piège de sable.");
                ResetTrap();
            }
            else
            {
                Debug.Log("Le joueur quitte le collider pendant qu’il s’enfonce (ignoré).");
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

        if (isPlayerSinking && player != null)
        {
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
            playerRb.gravityScale = sinkingGravityScale;

        if (trapCollider != null)
            trapCollider.enabled = false;

        Debug.Log("Le joueur commence à s'enfoncer dans le sable.");
    }

    private void KillPlayer()
    {
        if (playerDeath != null)
        {
            Debug.Log("Le joueur est mort après être resté trop longtemps dans le piège.");
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
            playerRb.gravityScale = originalGravityScale;

        if (trapCollider != null)
            trapCollider.enabled = true;

        player = null;
        playerRb = null;
        playerMovement = null;
        playerDeath = null;

        Debug.Log("Réinitialisation du piège de sable.");
    }
}
