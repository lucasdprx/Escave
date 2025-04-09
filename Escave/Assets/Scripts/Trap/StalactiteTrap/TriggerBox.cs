using UnityEngine;

public class TriggerBox : MonoBehaviour
{
    [Header("references")]
    [SerializeField] private StalactitesCollision StalactitesCollision;
    [SerializeField] private GameObject prefabTrap;

    [Header("Attribute")]
    [SerializeField] private string animName;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !prefabTrap.activeSelf)
        {
            prefabTrap.SetActive(true);
            StalactitesCollision.PlayAnimationAndWait(animName);
        }
    }
}
