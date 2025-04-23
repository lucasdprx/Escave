using UnityEngine;

public class CantClimbWall : MonoBehaviour
{
    [SerializeField] private float reboundForce = 2f; // Force de rebond appliquée au joueur
    [SerializeField] private Vector2 upwardRebound = new Vector2(0, 0.5f); // Composante verticale du rebond

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Vérifie si l'objet en collision est le joueur
        if (!collision.gameObject.CompareTag("Player")) return;

        // Récupère les composants nécessaires
        Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
        PlayerMovement movement = collision.gameObject.GetComponent<PlayerMovement>();
        if (playerRb == null || movement == null)
        {
            Debug.LogWarning("Player Rigidbody2D ou PlayerMovement manquant !");
            return;
        }

        // Calcule la direction du joueur par rapport au mur
        Vector2 playerToWallDirection = (collision.transform.position - transform.position).normalized;

        // Annule le rebond si le joueur vient du haut
        if (playerToWallDirection.y > 0.65f)
        {
            Debug.Log("Rebond annulé : le joueur vient du haut.");
            return;
        }

        // Détermine la direction du rebond (gauche ou droite)
        Vector2 reboundDirection = new Vector2(Mathf.Sign(playerToWallDirection.x), upwardRebound.y).normalized;

        // Applique le rebond au joueur
        movement.ApplyKnockback(reboundDirection, reboundForce);
        Debug.Log($"Rebond appliqué : direction {reboundDirection}, force {reboundForce}");
    }
}
