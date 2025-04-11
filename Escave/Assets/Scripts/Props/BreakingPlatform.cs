using System;
using Unity.VisualScripting;
using UnityEngine;

public class BreakingPlatform : MonoBehaviour
{
    [SerializeField] private float breakingTime;
    [SerializeField] private float recreationTime;
    
    [SerializeField] private ParticleSystem onTouchParticles;
    [SerializeField] private ParticleSystem breakingParticles;
    
    private BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer;

    private bool _hasTouched = false;
    private float _timer;

    private void Start()
    {
        Vector3 _scaleWanted = new Vector3(transform.localScale.x, 1f, 1f);
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        onTouchParticles.transform.localScale = _scaleWanted;
    }

    private void OnCollisionEnter2D(Collision2D _other)
    {
        if (_other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (!_hasTouched)
            {
                Instantiate(onTouchParticles, transform.position, Quaternion.identity);
                
                if(_other.transform.position.y > transform.position.y)
                    _hasTouched = true;
            }
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
                Instantiate(breakingParticles, transform.position, Quaternion.identity);
                _timer = 0;
                boxCollider.enabled = false;
                spriteRenderer.enabled = false;
            }
        }
        else
        {
            if (_timer >= recreationTime)
            {
                Instantiate(breakingParticles, transform.position, Quaternion.identity);
                _timer = 0;
                boxCollider.enabled = true;
                spriteRenderer.enabled = true;
                _hasTouched = false;
            }
        }
    }
}