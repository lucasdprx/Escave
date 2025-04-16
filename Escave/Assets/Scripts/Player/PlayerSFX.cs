using System;
using System.Collections;
using UnityEngine;

public class PlayerSFX : MonoBehaviour
{
    [SerializeField] private float _stepTime;
    private bool _isWalking;
    private void Start()
    {
        PlayerMovement.OnStepSound += PlayWalkSFX;
    }

    public static void PlaySFX(AudioType type)
    {
        AudioManager.Instance.PlaySound(type);
    }

    #region Start/Stop Routines
    private void PlayWalkSFX()
    {
        if (_isWalking) return;
        StartCoroutine(WalkRoutine());
    }
    #endregion

    #region Routines
    private IEnumerator WalkRoutine()
    {
         _isWalking = true;
         AudioManager.Instance.PlaySound(AudioType.step);
         yield return new WaitForSeconds(_stepTime);
        _isWalking = false;
    }
    #endregion
}
