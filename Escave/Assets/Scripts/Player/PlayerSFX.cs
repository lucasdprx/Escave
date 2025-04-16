using System.Collections;
using UnityEngine;

public class PlayerSFX : MonoBehaviour
{
    [SerializeField] private float _stepTime;
    private AudioManager _audioManager;
    private bool _isWalking;

    private void Start()
    {
        _audioManager = AudioManager.Instance;
    }

    public void PlaySFX(AudioType type) { _audioManager.PlaySound(type); } //For simple SFX without coroutines

    #region Start/Stop Routines
    public void PlayWalkSFX()
    {
        if (_isWalking) return;
        StartCoroutine(WalkRoutine());
    }
    #endregion

    #region Routines
    private IEnumerator WalkRoutine()
    {
        _isWalking = true;
         _audioManager.PlaySound(AudioType.step);
         yield return new WaitForSeconds(_stepTime);
        _isWalking = false;
    }
    #endregion
}
