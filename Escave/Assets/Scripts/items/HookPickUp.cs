using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class HookPickUp : MonoBehaviour
{
    [SerializeField] private GrapplingHook _grapplingHook;
    [SerializeField] private UIItemShow _itemShow;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            EnablingHook();
            DataPersistenceManager.instance.gameData.isHookEnabled = true;
            _itemShow.Active();
        }
    }

    public void EnablingHook()
    {
        _grapplingHook._isHookUnlock = true;
        gameObject.SetActive(false); 
    }
}
