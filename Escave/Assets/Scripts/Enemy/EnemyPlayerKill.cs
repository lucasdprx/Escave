using UnityEngine;

public class EnemyPlayerKill : MonoBehaviour
{
    private PlayerDeath _playerDeath;
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Player"))  return;
        
        _playerDeath = other.gameObject.GetComponent<PlayerDeath>();
        _playerDeath.PlayerDie();
        other.rigidbody.linearVelocity = Vector3.zero;
    }
}
