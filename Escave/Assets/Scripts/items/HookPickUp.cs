using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class HookPickUp : MonoBehaviour
{
    [SerializeField] private GrapplingHook _grapplingHook;
    [SerializeField] private UIItemShow _itemShow;
    private PlayerInput _playerAction;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _playerAction = other.GetComponent<PlayerInput>();
            EnablingHook();
            DataPersistenceManager.instance.gameData.isHookEnabled = true;
            _itemShow.ShowHook(_playerAction);
        }
    }

    public void EnablingHook()
    {
        _grapplingHook._isHookUnlock = true;
        gameObject.SetActive(false); 
    }
}
