using System.Collections;
using UnityEngine;

public class UIItemShow : MonoBehaviour
{
    [SerializeField] private GameObject _pickUpUiShowPiolet;
    [SerializeField] private GameObject _pickUpUiShowHook;
    [SerializeField] private float _itemShowDelay;

    public void ShowPiolet()
    {
        _pickUpUiShowPiolet.SetActive(true);
        StartCoroutine(DisableUI(_pickUpUiShowPiolet));
    }    
    public void ShowHook()
    {
        _pickUpUiShowHook.SetActive(true);
        StartCoroutine(DisableUI(_pickUpUiShowHook));
    }

    private IEnumerator DisableUI(GameObject obj)
    {
        yield return new WaitForSeconds(_itemShowDelay);
        obj.SetActive(false);
        StopCoroutine(DisableUI(obj));
    }
}
