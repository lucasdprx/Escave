using System.Collections;
using UnityEngine;

public class CollectObjet : MonoBehaviour
{
    public CollectibleData collectibleData;
    private SpriteRenderer spriteRenderer;
    private AudioManager _audioManager;
    
    public CollectiblesSave collectiblesSave;
    
    [SerializeField] private Sprite icon;
    [SerializeField] private ParticleSystem pickupEffect;

    private void Start()
    {
        collectibleData = new CollectibleData();
        
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = icon;
        _audioManager = AudioManager.Instance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collectibleData.HasBeenCollected)
        {
            return;
        }

        Debug.Log("Collision with " + collision.gameObject.name);

        if (collision.CompareTag("Player"))
        {
            _audioManager.PlaySound(AudioType.collectibleGet);
            collectiblesSave.OnCollectibleCollected();
            collectibleData.PickUp();
            ParticleSystem effect = PlayPickupEffect(transform.position);
            StartCoroutine(DestroyCollectible(effect));
        }
    }

    private IEnumerator DestroyCollectible(ParticleSystem effect)
    {
        if (effect != null)
        {
            while (effect.isPlaying)
            {
                yield return null;
            }
        }
        Destroy(gameObject);
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
            Debug.LogWarning("pickupEffect non assign� !");
            return null;
        }
    }
}
