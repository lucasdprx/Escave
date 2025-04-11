using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingSelection : MonoBehaviour
{
    [SerializeField] private Slider slider;

    private void OnEnable()
    {
        slider.Select();
    }

    public void SliderSelect()
    {
        slider.Select();
    }
}
