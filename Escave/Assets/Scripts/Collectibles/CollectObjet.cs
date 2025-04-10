using System.Collections;
using UnityEngine;

public class CollectObjet : MonoBehaviour
{
    [SerializeField] CollectibleData collectibleData;
    [SerializeField] private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = collectibleData.Icon;
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
            collectibleData.PlayPickupSound(audioSource);
            collectibleData.PickUpAugmentation();
            ParticleSystem effect = collectibleData.PlayPickupEffect(transform.position);
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
}
