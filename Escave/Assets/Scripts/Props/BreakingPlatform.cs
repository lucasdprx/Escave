using System;
using System.Collections;
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
    public event Action OnBroken;

   [SerializeField] private GameObject _breakingPlatformPositionBlock;
   [SerializeField] private float _distanceToShake = 0.05f;

    private void Start()
    {
        _initialPosition = transform.position;
        Vector3 _scaleWanted = new Vector3(transform.localScale.x, 1f, 1f);
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        onTouchParticles.transform.localScale = _scaleWanted;
    }

    private void OnCollisionEnter2D(Collision2D _other)
    {
        if (_other.gameObject.layer != LayerMask.NameToLayer("Player"))  return;
        
        if (!_hasTouched)
        {
            Instantiate(onTouchParticles, transform.position, Quaternion.identity);
                
            if(_other.transform.position.y > transform.position.y)
                _hasTouched = true;
        }

        if (_hasTouched)
        {
            StartCoroutine(Shake());
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
                BreakPlateform();
            }
        }
        else
        {
            if (_timer >= recreationTime)
            {
                ResetPlateform();
            }
        }
    }

    private void BreakPlateform()
    {
        StopAllCoroutines();
        Instantiate(_breakingPlatformPositionBlock, transform.position, Quaternion.identity);
        Instantiate(breakingParticles, transform.position, Quaternion.identity);
        _timer = 0;
        boxCollider.enabled = false;
        spriteRenderer.enabled = false;

        OnBroken?.Invoke();
    }

    private void ResetPlateform()
    {
        transform.position = _initialPosition;
        Instantiate(breakingParticles, transform.position, Quaternion.identity);
        _timer = 0;
        boxCollider.enabled = true;
        spriteRenderer.enabled = true;
        _hasTouched = false;
        transform.position = _initialPosition;
    }
    
    private IEnumerator Shake() 
    {
        Vector3 originalPosition = _initialPosition;

        while (true)
        {
            float offset = _distanceToShake;
            transform.position = originalPosition + new Vector3(offset, 0, 0);
            yield return new WaitForSeconds(0.01f);
            transform.position = originalPosition - new Vector3(offset, 0, 0);
            yield return new WaitForSeconds(0.01f);
        }
    }

}