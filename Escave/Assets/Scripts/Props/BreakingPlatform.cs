using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakingPlatform : MonoBehaviour
{
    [SerializeField] private float breakingTime;
    [SerializeField] private float recreationTime;
    
    [SerializeField] private ParticleSystem onTouchParticles;
    [SerializeField] private ParticleSystem breakingParticles;
    
    private BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer;

    private bool _hasTouched;
    private Vector3 _initialPosition;
    private float _timer;
    private Transform _plateformTransform;
    public event Action OnBroken;

    private AudioManager _audioManager;

   [SerializeField] private GameObject _breakingPlatformPositionBlock;
   [SerializeField] private float _distanceToShake = 0.05f;
   [SerializeField] private float _shakeDuration = 0.5f; // Durée du shake en secondes

    private Queue<ParticleSystem> particlePool = new Queue<ParticleSystem>();

    private void Start()
    {
        _plateformTransform = transform;
        _initialPosition = _plateformTransform.position;
        Vector3 _scaleWanted = new Vector3(_plateformTransform.localScale.x, 1f, 1f);
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        onTouchParticles.transform.localScale = _scaleWanted;

        _audioManager = AudioManager.Instance;
    }

    private void OnCollisionEnter2D(Collision2D _collisionGameObject)
    {
        if (_collisionGameObject.gameObject.layer != LayerMask.NameToLayer("Player"))  return;
        
        if (!_hasTouched)
        {
            Instantiate(onTouchParticles, transform.position, Quaternion.identity);
                
            if(_collisionGameObject.transform.position.y > transform.position.y)
                _hasTouched = true;
        }

        if (_hasTouched)
        {
            StartCoroutine(Shake(_shakeDuration));
        }
    }

    private void Update()
    {
        if (!_hasTouched) return;
        
        _timer += Time.deltaTime;
        if (boxCollider.enabled)
        {
            if (_timer >= breakingTime)
            {
                BreakPlatform();
            }
        }
        else
        {
            if (_timer >= recreationTime)
            {
                ResetPlatform();
            }
        }
    }

    private void BreakPlatform()
    {
        StopAllCoroutines();
        Vector3 position = _plateformTransform.position;
        Instantiate(_breakingPlatformPositionBlock, position, Quaternion.identity);
        Instantiate(breakingParticles, position, Quaternion.identity);
        _timer = 0;
        boxCollider.enabled = false;
        spriteRenderer.enabled = false;
        _audioManager.PlaySound(AudioType.platformBreak);

        OnBroken?.Invoke();
    }

    private void ResetPlatform()
    {
        Vector3 position = _initialPosition;
        Instantiate(breakingParticles, position, Quaternion.identity);
        _timer = 0;
        boxCollider.enabled = true;
        spriteRenderer.enabled = true;
        _hasTouched = false;
        position = _initialPosition;
        _plateformTransform.position = position;
        _audioManager.PlaySound(AudioType.platformRespawn);
    }
    
    private IEnumerator Shake(float duration)
    {
        Vector3 originalPosition = _initialPosition;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float offset = _distanceToShake;
            _plateformTransform.position = originalPosition + new Vector3(offset, 0, 0);
            yield return new WaitForSeconds(0.01f);
            _plateformTransform.position = originalPosition - new Vector3(offset, 0, 0);
            yield return new WaitForSeconds(0.01f);

            elapsedTime += 0.02f; // Temps total �coul�
        }

        _plateformTransform.position = originalPosition; // R�initialiser la position
    }

    private void InitializeParticlePool(int poolSize)
    {
        for (int i = 0; i < poolSize; i++)
        {
            ParticleSystem particle = Instantiate(breakingParticles);
            particle.gameObject.SetActive(false);
            particlePool.Enqueue(particle);
        }
    }

    private ParticleSystem GetParticleFromPool()
    {
        if (particlePool.Count > 0)
        {
            ParticleSystem particle = particlePool.Dequeue();
            particle.gameObject.SetActive(true);
            return particle;
        }
        return Instantiate(breakingParticles); // Si le pool est vide
    }

    private void ReturnParticleToPool(ParticleSystem particle)
    {
        particle.gameObject.SetActive(false);
        particlePool.Enqueue(particle);
    }

}