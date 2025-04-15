using UnityEngine;

public class EnemyJumpBoost : MonoBehaviour
{
    [SerializeField] private float _jumpPower;
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.rigidbody.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
        }
    }
}
