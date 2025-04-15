using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Video;

public class UIItemShow : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] InputActionReference _action;

    [Header("Messages")]
    [SerializeField] private TextMeshProUGUI _textMesh;
    [TextArea]
    [SerializeField] private string message;
    
    [Header("Videos")]
    [SerializeField] private VideoPlayer _videoPlayer1;
    [SerializeField] private VideoPlayer _videoPlayer2;

    public void Active()
    {
        Time.timeScale = 0;
        _playerInput.currentActionMap.Disable();
        ShowWindow();
    }

    public void HideWindow()
    {
        _playerInput.currentActionMap.Enable();
        Time.timeScale = 1;
        
        gameObject.SetActive(false);
    }

    private void ShowWindow()
    {
        gameObject.SetActive(true);
        _textMesh.text = message.Replace(
            "[firstBind]", _action.action.bindings[0].ToString().Split("/")[1]).Replace(
            "[secondBind]", _action.action.bindings[1].ToString().Split("/")[1]);
        
        _videoPlayer1.Play();
        _videoPlayer2.Play();
    }
}
