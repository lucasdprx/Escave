using UnityEngine;

public class PlayerStep : MonoBehaviour
{
    [SerializeField] private float _stepTime;
    private bool _isWalking;
    private float _timerStep;
    private void Start()
    {
        PlayerMovement.OnStepSound += PlayWalkSFX;
    }
    
    private void PlayWalkSFX()
    {
        if (_isWalking) return;
        
        _isWalking = true;
        AudioManager.Instance.PlaySound(AudioType.step);
    }

    private void Update()
    {
        if (!_isWalking) return;
        
        _timerStep += Time.deltaTime;
        if (_timerStep >= _stepTime)
        {
            _isWalking = false;
            _timerStep = 0;
        }
    }
}
