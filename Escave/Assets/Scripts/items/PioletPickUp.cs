using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PioletPickUp : MonoBehaviour
{
    [SerializeField] private PlayerWallJump _playerWallJump;
    [SerializeField] private UIItemShow _itemShow;
    private PlayerInput _playerAction;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _playerAction = other.GetComponent<PlayerInput>();
            EnablingPiolet();
            DataPersistenceManager.instance.gameData.isPioletEnabled = true;
            _itemShow.Active();
        }
    }
    
    public void EnablingPiolet()
    {
        _playerWallJump._isGrabUnlock = true;
        gameObject.SetActive(false);
    }
}
