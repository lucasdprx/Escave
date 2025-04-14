using System;
using UnityEngine;

public class PioletPickUp : MonoBehaviour
{
    [SerializeField] private PlayerWallJump _playerWallJump;
    [SerializeField] private GameObject _pickUpUiShow;
    [SerializeField] private UIItemShow _itemShow;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            EnablingPiolet();
            DataPersistenceManager.instance.gameData.isPioletEnabled = true;
            _itemShow.ShowPiolet();
        }
    }
    
    public void EnablingPiolet()
    {
        _playerWallJump._isGrabUnlock = true;
        gameObject.SetActive(false);
    }
}
