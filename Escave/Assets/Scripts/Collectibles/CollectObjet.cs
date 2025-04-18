using System;
using System.Collections;
using UnityEngine;

public class CollectObjet : MonoBehaviour
{
    public CollectibleData collectibleData;
    private SpriteRenderer spriteRenderer;
    private AudioManager _audioManager;
    
    [SerializeField] private Sprite icon;
    [SerializeField] private ParticleSystem pickupEffect;
    
    public static event Action OnCollectibleCollected;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = icon;
        _audioManager = AudioManager.Instance;
    }

    public void Initialize(bool _isCollected)
    {
        collectibleData = new CollectibleData();
        collectibleData.HasBeenCollected = _isCollected;

        gameObject.SetActive(!collectibleData.HasBeenCollected);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collectibleData.HasBeenCollected || !collision.CompareTag("Player"))
        {
            return;
        }
        
        OnCollectibleCollected?.Invoke();
        _audioManager.PlaySound(AudioType.collectibleGet);
        collectibleData.PickUp();
        ParticleSystem effect = PlayPickupEffect(transform.position);
        if (effect)
        {
            StartCoroutine(DestroyCollectible(effect));
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private IEnumerator DestroyCollectible(ParticleSystem effect)
    {
        yield return new WaitForSeconds(effect.totalTime);
        gameObject.SetActive(false);
    }

    private ParticleSystem PlayPickupEffect(Vector3 position)
    {
        if (pickupEffect != null)
        {
            ParticleSystem effect = Instantiate(pickupEffect, position, Quaternion.identity);
            effect.Play();
            Destroy(effect.gameObject, effect.main.duration);
            return effect;
        }
        
        Debug.LogWarning("pickupEffect non assign� !");
        return null;
    }
}
