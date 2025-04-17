using UnityEngine;
using UnityEngine.UI;

public class BackBtnScript : MonoBehaviour
{
    public void OnLeftInput(GameObject _leftBtn)
    {
        _leftBtn.GetComponent<Button>().Select();
        Debug.Log(_leftBtn.name);
    }
}
