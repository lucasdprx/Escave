using UnityEngine;
using UnityEngine.UI;

public class CanvasSelectionButton : MonoBehaviour
{
    [SerializeField] private Button _buttonToSelect;

    private void OnEnable()
    {
        _buttonToSelect.Select();
    }
}
