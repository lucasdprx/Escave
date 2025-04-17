using UnityEngine;

public class DestroyBreakingPlatformPositionBlock : MonoBehaviour
{
    [SerializeField] private float _destroyBlockPlatform;
    private float _actualTimer;
    
    private void Update()
    {
        _actualTimer += Time.deltaTime;
        if (_actualTimer >= _destroyBlockPlatform)
        {
            _actualTimer = 0;
            Destroy(gameObject);
        }
    }
}
