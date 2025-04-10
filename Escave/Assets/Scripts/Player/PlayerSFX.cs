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

    #region Simple SFX (no routines)
    
    public void PlayDeathSound()           { _audioManager.PlaySound(AudioType.death);           }
    public void PlayRespawnSound()         { _audioManager.PlaySound(AudioType.respawn);         }
    public void PlayCheckpointReachSound() { _audioManager.PlaySound(AudioType.checkpointReach); }
    public void PlayWallClingSFX()
    {
        //!\\ UNIMPLEMENTED
        //_audioManager.PlaySound(AudioType.wallCling);
    }

    public void PlayWallJumpSFX()
    {
        //!\\ UNIMPLEMENTED
        //_audioManager.PlaySound(AudioType.wallJump);
    }

    public void PlayJumpLandSFX() { _audioManager.PlaySound(AudioType.jumpLand); }
    #endregion

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
