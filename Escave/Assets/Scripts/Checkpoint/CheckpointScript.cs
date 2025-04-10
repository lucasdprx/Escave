using UnityEngine;

public class CheckpointScript : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D _other)
    {
        print(_other);
        if (_other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            print("enter");
            _other.GetComponent<PlayerDeath>().SetCheckpoint(gameObject);
        }
    }
}
