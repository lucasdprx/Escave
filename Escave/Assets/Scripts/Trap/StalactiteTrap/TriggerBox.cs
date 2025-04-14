using UnityEngine;

public class TriggerBox : MonoBehaviour
{
    [Header("references")]
    [SerializeField] private StalactitesCollision _stalactitesCollision;

    private bool _isPlayerInside = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        _isPlayerInside = true;

        if (!_stalactitesCollision.isStarted)
        {
            _stalactitesCollision.StartTrap();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _isPlayerInside = false;
        }
    }

    public bool IsPlayerStillInside()
    {
        return _isPlayerInside;
    }
}
