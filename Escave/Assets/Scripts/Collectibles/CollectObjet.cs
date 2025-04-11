using System.Collections;
using UnityEngine;

public class CollectObjet : MonoBehaviour
{
    [SerializeField] CollectibleData collectibleData;
    private SpriteRenderer spriteRenderer;
    private AudioManager _audioManager;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = collectibleData.Icon;
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
