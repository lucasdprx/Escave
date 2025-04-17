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
    [SerializeField, ReadOnly] private int deathCounter;

    public UnityEvent<int> OnDeath;
    public UnityEvent OnDeath2;
    public bool _isRestarting;
    
    [SerializeField] private ParticleSystem _deathParticles;
    [SerializeField] private GrapplingHook _grapplingHook;
    [SerializeField] private CollectiblesSave _collectiblesSave;
    
    private void Start()
    {
        PauseMenuManager.OnRestartGame += PlayerDie;
    }

    public void SaveData(ref GameData _gameData)
    {
        _gameData.deathCount = deathCounter;
        if (currentCheckpoint)
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
        if (deathCounter > 0)
        {
            OnDeath.Invoke(deathCounter);
        }
    }

    public void PlayerDie()
    {
        AudioManager.Instance.PlaySound(AudioType.death);

        if (currentCheckpoint == null)
        {
            return;
        }

        if (!_isRestarting)
        {
            deathCounter++;
            OnDeath.Invoke(deathCounter);
            Instantiate(_deathParticles, this.transform.position, Quaternion.identity);
        }
        _collectiblesSave.LoadData(DataPersistenceManager.instance.gameData);
        _grapplingHook.DestroyProjectile();
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        transform.position = currentCheckpoint.transform.position;
        AudioManager.Instance.PlaySound(AudioType.respawn);
        
        OnDeath2.Invoke();
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
            _collectiblesSave.SaveData(ref DataPersistenceManager.instance.gameData);
            currentCheckpoint = newCheckpoint;
            AudioManager.Instance.PlaySound(AudioType.checkpointReach);
        }
        else
        {
            Debug.LogWarning("Le checkpoint sp�cifi� n'est pas dans la liste des checkpoints !");
        }
    }
}
