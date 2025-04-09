using NUnit.Framework;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    [Header("Checkpoints")]
    [SerializeField] private List<GameObject> checkpoints; // checkpoint list
    private GameObject currentCheckpoint; // active checkpoint

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip deathSound; // sound to play on death

    public int DeathCount => deathCounter;
    [SerializeField, ReadOnly] private int deathCounter = 0;

    private void Start()
    {
        // initialisation of the current checkpoint to the first one in the list
        if (checkpoints != null && checkpoints.Count > 0)
        {
            currentCheckpoint = checkpoints[0];
        }
        else
        {
            Debug.LogError("Aucun checkpoint n'est assigné !");
        }
    }

    public void PlayerDie()
    {
        if (currentCheckpoint == null)
        {
            Debug.LogError("Aucun checkpoint actif n'est défini !");
            return;
        }

        transform.position = currentCheckpoint.transform.position;
        deathCounter++;

        PlayDeathSound();
    }

    private void PlayDeathSound()
    {
        if (audioSource != null && deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }
        else
        {
            Debug.LogWarning("AudioSource ou DeathSound non assigné !");
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
            Debug.LogWarning("Le checkpoint spécifié n'est pas dans la liste des checkpoints !");
        }
    }
}
