using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CollectibleHeart", menuName = "Scriptable Objects/Collectibles/Heart")]
public class CollectibleData : ScriptableObject
{
    [Header("References")]
    [SerializeField] private Sprite icon;
    [SerializeField] private GameObject prefab;

    [Header("Attributes")]
    [SerializeField] private string collectibleName;
    [SerializeField] private int valueCollect;
    private bool hasBeenCollected = false;

    [Header("Effects")]
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private AudioSource pickupSource;
    [SerializeField] private ParticleSystem pickupEffect;


    public string CollectibleName => collectibleName;
    public Sprite Icon => icon;
    public GameObject Prefab => prefab;
    public AudioClip PickupSound => pickupSound;
    public ParticleSystem PickupEffect => pickupEffect;
    public bool HasBeenCollected => hasBeenCollected;
    public int ValueCollect => valueCollect;


    public void PlayPickupSound(AudioSource source)
    {
        if (source != null && pickupSound != null)
        {
            source.PlayOneShot(pickupSound);
        }
        else
        {
            Debug.LogWarning("pickupSound non assigné !");
        }
    }

    public void PickUpAugmentation()
    {
        valueCollect++;
    }

    public ParticleSystem PlayPickupEffect(Vector3 position)
    {
        if (pickupEffect != null)
        {
            ParticleSystem effect = Instantiate(pickupEffect, position, Quaternion.identity);
            effect.Play();
            Destroy(effect.gameObject, effect.main.duration);
            return effect;
        }
        else
        {
            Debug.LogWarning("pickupEffect non assigné !");
            return null;
        }
    }

    public void ResetDataCollectibles()
    {
        valueCollect = 0;
        hasBeenCollected = false;
    }
}
