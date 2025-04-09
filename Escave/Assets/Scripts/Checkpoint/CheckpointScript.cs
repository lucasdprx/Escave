using UnityEngine;

public class CheckpointScript : MonoBehaviour
{
    [SerializeField] private LayerMask playerMask;
    
    void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.gameObject.layer == playerMask)
        {
            _other.GetComponent<PlayerDeath>().SetCheckpoint(gameObject);
        }
    }
}
