using UnityEngine;

public class TriggerBox : MonoBehaviour
{
    [Header("references")]
    [SerializeField] private StalactitesCollision StalactitesCollision;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !StalactitesCollision.isStarted)
        {
            StalactitesCollision.StartTrap();
        }
    }
}
