using UnityEngine;
using UnityEngine.UI;

public class CanvasSelectionButton : Select
{
    [SerializeField] private Button _buttonToSelect;

    private void OnEnable()
    {
        _buttonToSelect.Select();
    }

    public override void SelectThing()
    {
        _buttonToSelect.Select();
    }

    public void ButtonSelect()
    {
        _buttonToSelect.Select();
    }
}
