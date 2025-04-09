using NUnit.Framework;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDeath : MonoBehaviour, IDataPersistence
{
    [Header("Checkpoints")]
    [SerializeField] private List<GameObject> checkpoints; // checkpoint list
    private GameObject currentCheckpoint; // active checkpoint

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip deathSound; // sound to play on death

    public int DeathCount => deathCounter;
    [SerializeField, ReadOnly] private int deathCounter = 0;

    public UnityEvent<int> OnDeath;
    public bool _isRestarting = false;
    
    public void LoadData(GameData _gameData)
    [SerializeField] private ParticleSystem _deathParticles;
    
    private void Start()
    {
        if (_gameData.playerPos != Vector2.zero)
        {
            transform.position = _gameData.playerPos;
            currentCheckpoint = checkpoints[0];
        }
        
        deathCounter = _gameData.deathCount;
        OnDeath.Invoke(deathCounter);
    }

    public void SaveData(ref GameData _gameData)
    {
        _gameData.deathCount = deathCounter;
        _gameData.playerPos = currentCheckpoint.transform.position;
    }
    
    public void LoadData(GameData _gameData)
    {
        if(_gameData.playerPos != Vector2.zero)
            transform.position = _gameData.playerPos;
        
        deathCounter = _gameData.deathCount;
    }

    public void SaveData(ref GameData _gameData)
    {
        _gameData.playerPos = currentCheckpoint.transform.position;
        _gameData.deathCount = deathCounter;
    }

    public void PlayerDie()
    {
        if (currentCheckpoint == null)
        {
            Debug.LogError("Aucun checkpoint actif n'est d�fini !");
            return;
        }

        if (!_isRestarting)
        {
            deathCounter++;
            OnDeath.Invoke(deathCounter);
            PlayDeathSound();
            Instantiate(_deathParticles, this.transform.position, Quaternion.identity);
        }
        transform.position = currentCheckpoint.transform.position;
    }

    private void PlayDeathSound()
    {
        if (audioSource != null && deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }
        else
        {
            Debug.LogWarning("AudioSource ou DeathSound non assign� !");
        }
    }

    public void SetCheckpoint(GameObject newCheckpoint)
    {
        if (newCheckpoint == null)
        {
            Debug.LogWarning("Checkpoint fourni est null !");
            return;
        }

        if (checkpoints.Contains(newCheckpoint))
        {
            currentCheckpoint = newCheckpoint;
        }
        else
        {
            Debug.LogWarning("Le checkpoint sp�cifi� n'est pas dans la liste des checkpoints !");
        }
    }
    
}
