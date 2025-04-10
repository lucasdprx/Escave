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

    public int DeathCount => deathCounter;
    [SerializeField, ReadOnly] private int deathCounter = 0;

    public UnityEvent<int> OnDeath;
    public bool _isRestarting = false;
    
    [SerializeField] private ParticleSystem _deathParticles;

    private PlayerSFX _playerSFX;

    private void Start()
    {
        _playerSFX = GetComponent<PlayerSFX>();
    }

    public void SaveData(ref GameData _gameData)
    {
        _gameData.deathCount = deathCounter;
        _gameData.playerPos = currentCheckpoint.transform.position;
    }
    
    public void LoadData(GameData _gameData)
    {
        if (_gameData.playerPos != Vector2.zero)
        {
            transform.position = _gameData.playerPos;
        }
        else
        {
            currentCheckpoint = checkpoints[0];
        }
        deathCounter = _gameData.deathCount;
    }

    public void PlayerDie()
    {
        _playerSFX.PlayDeathSound();

        if (currentCheckpoint == null)
        {
            Debug.LogError("Aucun checkpoint actif n'est d�fini !");
            return;
        }

        if (!_isRestarting)
        {
            deathCounter++;
            OnDeath.Invoke(deathCounter);
            Instantiate(_deathParticles, this.transform.position, Quaternion.identity);
        }
        transform.position = currentCheckpoint.transform.position;
        _playerSFX.PlayRespawnSound();
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
            _playerSFX.PlayCheckpointReachSound();
        }
        else
        {
            Debug.LogWarning("Le checkpoint sp�cifi� n'est pas dans la liste des checkpoints !");
        }
    }
    
}
