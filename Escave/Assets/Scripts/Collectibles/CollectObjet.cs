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
        if (collectibleData.HasBeenCollected || !collision.CompareTag("Player"))
        {
            return;
        }
        
        _audioManager.PlaySound(AudioType.collectibleGet);
        collectiblesSave.OnCollectibleCollected();
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
