using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingSelection : Select
{
    [SerializeField] private Slider slider;

    private void OnEnable()
    {
        slider.Select();
    }

    public override void SelectThing()
    {
        slider.Select();
    }

    public void SliderSelect()
    {
        slider.Select();
    }
}
