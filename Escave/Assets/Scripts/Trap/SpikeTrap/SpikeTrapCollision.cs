using UnityEngine;

public class TrapCollision : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        
        collision.gameObject.GetComponent<PlayerDeath>().PlayerDie();
    }

}
