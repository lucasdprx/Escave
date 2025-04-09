using System.Collections;
using UnityEngine;

public class StalactitesCollision : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject prefabSpawnPoint;
    [SerializeField] private GameObject prefabTrap;
    [SerializeField] private Animator stalactitesRespawnAnimation;
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private string animName;

    private const string PlayerTag = "Player";
    private const string GroundTag = "Ground";
    private PlayerDeath playerDeathScript;
    private float animationLength;

    private void Awake()
    {
        if (prefabSpawnPoint == null || prefabTrap == null || stalactitesRespawnAnimation == null || rb == null)
        {
            Debug.LogError("Une ou plusieurs références ne sont pas assignées dans l'inspecteur !");
        }
        SetGravity(0);
    }



    private void Start()
    {
        // Récupérer la bonne longueur de l'animation (via clip, pas state info)
        AnimationClip[] clips = stalactitesRespawnAnimation.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name == animName)
            {
                animationLength = clip.length;
                break;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.transform.tag)
        {
            case PlayerTag:
                Debug.Log("dead");
                playerDeathScript = collision.gameObject.GetComponent<PlayerDeath>();
                playerDeathScript.PlayerDie();
                break;

            case GroundTag:
                transform.position = prefabSpawnPoint.transform.position;
                rb.simulated = false;
                SetGravity(0);
                gameObject.SetActive(false);
                break;

            default:
                Debug.Log($"Collision avec un objet non géré : {collision.transform.tag}");
                break;
        }
    }

    public void PlayAnimationAndWait(string animName)
    {
        SetGravity(0);

        // Réaffiche le piège
        transform.localScale = Vector3.one;
        rb.simulated = true;

        StartCoroutine(WaitAnimationRespawn(animName));
    }

    public IEnumerator WaitAnimationRespawn(string animName)
    {
        int hash = Animator.StringToHash(animName);

        if (stalactitesRespawnAnimation.HasState(0, hash))
        {
            // Rewind forcé
            stalactitesRespawnAnimation.Play(animName, 0, 0);
        }
        else
        {
            Debug.LogError($"L'animation '{animName}' n'existe pas dans l'Animator.");
            yield break;
        }

        yield return new WaitForSeconds(animationLength);
        SetGravity(1f);
    }

    private void SetGravity(float gravityScale)
    {
        rb.gravityScale = gravityScale;
    }
}
