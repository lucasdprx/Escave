using UnityEngine;

public class HookPickUp : MonoBehaviour
{
    [SerializeField] private GrapplingHook _grapplingHook;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            EnablingHook();
            DataPersistenceManager.instance.gameData.isHookEnabled = true;
        }
    }

    public void EnablingHook()
    {
        _grapplingHook._isHookUnlock = true;
        gameObject.SetActive(false); 
    }
}
