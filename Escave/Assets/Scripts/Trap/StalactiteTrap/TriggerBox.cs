using UnityEngine;

public class TriggerBox : MonoBehaviour
{
    [Header("references")]
    [SerializeField] private StalactitesCollision _stalactitesCollision;

    private bool _isPlayerInside;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        _stalactitesCollision.SetIsPlayerInside(true);

        if (!_stalactitesCollision.GetIsStarted())
        {
            _stalactitesCollision.StartTrap();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _stalactitesCollision.SetIsPlayerInside(false);
        }
    }
}
