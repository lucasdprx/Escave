using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputSetter : MonoBehaviour
{
    [SerializeField] private InputActionAsset _inputActionAsset;
    [SerializeField] private PlayerInput _playerInput;
    private bool _isSet = false;

    private void LateUpdate()
    {
        // if (_isSet == false)
        // {
        //     _playerInput.actions = _inputActionAsset;
        //     _isSet = true;
        // }
    }

    public void SetPlayerInput()
    {
        if (_isSet == false)
        {
            _playerInput.actions = _inputActionAsset;
            _isSet = true;
        }
    }
}
