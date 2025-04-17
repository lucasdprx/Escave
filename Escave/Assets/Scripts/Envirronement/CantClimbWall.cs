using UnityEngine;

public class CantClimbWall : MonoBehaviour
{
    [SerializeField] private float reboundForce = 2f; // rebund force applied to the player

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
        PlayerMovement movement = collision.gameObject.GetComponent<PlayerMovement>();
        if (playerRb == null || movement == null) return;


        Vector2 collisionDirection = (collision.transform.position - transform.position).normalized; // Direction du joueur par rapport au mur


        if (collisionDirection.y > 0.5f) // On annule le rebond si le joueur vient du haut
        {
            Debug.Log("Rebound cancelled: player came from above.");
            return;
        }

        Vector2 reboundDirection; // détermine si le joueur viens de la gauche ou de la droite
        if (collisionDirection.x > 0)
        {
            
            reboundDirection = new Vector2(1f, 0.5f).normalized; // Joueur vient de la droite / rebond à droite
        }
        else
        {
            reboundDirection = new Vector2(-1f, 0.5f).normalized; // Joueur vient de la droite / rebond à droite
        }

        movement.ApplyKnockback(reboundDirection, reboundForce);
        Debug.Log("Rebound applied: " + reboundDirection);
    }
}
