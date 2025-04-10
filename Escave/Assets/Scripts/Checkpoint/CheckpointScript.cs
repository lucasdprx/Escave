using UnityEngine;

public class CheckpointScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _other.GetComponent<PlayerDeath>().SetCheckpoint(gameObject);
        }
    }
}
