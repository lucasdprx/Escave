using UnityEngine;

public class TrapCollision : MonoBehaviour
{
    public Transform respawnPoint;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        
        collision.transform.position = respawnPoint.position;
    }

}
