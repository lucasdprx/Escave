using UnityEngine;

public class TrapCollision : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        if (collision.gameObject.TryGetComponent<PlayerDeath>(out var playerDeath))
        {
            // Appelle la méthode pour tuer le joueur
            playerDeath.PlayerDie();
        }
        else
        {
            Debug.LogWarning("Le joueur n'a pas de composant PlayerDeath attaché !");
        }
    }
}
