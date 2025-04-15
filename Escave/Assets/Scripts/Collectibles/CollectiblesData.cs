using UnityEngine;

public class CollectibleData
{
    [Header("Attributes")]
    private bool hasBeenCollected = false;

    [Header("Effects")]
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private AudioSource pickupSource;
    
    public AudioClip PickupSound => pickupSound;
    public bool HasBeenCollected => hasBeenCollected;


    public void PlayPickupSound(AudioSource source)
    {
        if (source != null && pickupSound != null)
        {
            source.PlayOneShot(pickupSound);
        }
        else
        {
            Debug.LogWarning("pickupSound non assign� !");
        }
    }

    public void PickUp()
    {
        Debug.Log(hasBeenCollected);
        hasBeenCollected = true;
    }

    public void SetData(bool _hasBeenCollected)
    {
        hasBeenCollected = _hasBeenCollected;
    }
}