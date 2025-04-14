using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIItemShow : MonoBehaviour
{
    [SerializeField] private GameObject _pickUpUiShowPiolet;
    [SerializeField] private TextMeshProUGUI _pioletTutorial;
    [SerializeField] private TextMeshProUGUI _hookTutorial;
    [SerializeField] private GameObject _pickUpUiShowHook;
    [SerializeField] private float _itemShowDelay;
    private PlayerInput _playerInput;

    public void ShowPiolet(PlayerInput action)
    {
        Time.timeScale = 0;
        _playerInput = action;
        _playerInput.currentActionMap.Disable();
        _pickUpUiShowPiolet.SetActive(true);
        string[] keyboardInput = action.currentActionMap.bindings[23].ToString().Split("/");
        string[] controllerInput = action.currentActionMap.bindings[25].ToString().Split("/");
        _pioletTutorial.SetText("You can use it by jumping against a wall, to stay attached you must hold the movement key towards the wall and press [" + keyboardInput[1] + "] or [" + controllerInput[1] + "].");
        
    }    
    public void ShowHook(PlayerInput action)
    {
        Time.timeScale = 0;
        _playerInput = action;
        _playerInput.currentActionMap.Disable();
        _pickUpUiShowHook.SetActive(true);
        string[] keyboardInput = action.currentActionMap.bindings[21].ToString().Split("/");
        string[] controllerInput = action.currentActionMap.bindings[22].ToString().Split("/");
        _hookTutorial.SetText("You can use it by pressing [" + keyboardInput[1] + "] or [" + controllerInput[1] + "].");
    }

    public void HideUI()
    {
        _playerInput.currentActionMap.Enable();
        Time.timeScale = 1;
        _pickUpUiShowPiolet.SetActive(false);
        _pickUpUiShowHook.SetActive(false);
    }
    
    
}
