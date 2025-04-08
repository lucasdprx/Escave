using UnityEngine;

public class TrapCollision : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //appeler la fonction de la mort
        Debug.Log("hi");
    }

}
