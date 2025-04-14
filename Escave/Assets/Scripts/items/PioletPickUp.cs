using System;
using UnityEngine;

public class PioletPickUp : MonoBehaviour
{
    [SerializeField] private PlayerWallJump _playerWallJump;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            EnablingPiolet();
            DataPersistenceManager.instance.gameData.isPioletEnabled = true;
        }
    }
    
    public void EnablingPiolet()
    {
        _playerWallJump._isGrabUnlock = true;
        gameObject.SetActive(false);
    }
}
