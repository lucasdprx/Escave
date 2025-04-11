using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.UI;
using UnityEngine.UI;

public class DropdownHandler : MonoBehaviour
{
    [SerializeField] private GameObject template;
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private ScrollRect scrollRect;
    
    [SerializeField] private List<Selectable> selectables;
     
    [SerializeField] private OptionsMenuHandler optionsMenuHandler;

    private Vector2 _nextPos;
    
    [SerializeField] private PlayerInput playerInput;

    private GameObject selectedElement;
    
    void OnEnable()
    {
        foreach (var _action in playerInput.actionEvents)
        {
            if (_action.actionName.Contains("Move"))
            {
                _action.AddListener(InputAction);
            }
        }
        
        if (gameObject.name == "Template") return;
        
        scrollRect = GetComponent<ScrollRect>();
        scrollRect.content.GetComponentsInChildren(selectables);
        
        string _actualRes = Screen.currentResolution.width + "x" + Screen.currentResolution.height;

        foreach (Selectable selectable in selectables)
        {
            if(selectable.name.Contains(_actualRes))
                EventSystem.current.SetSelectedGameObject(selectable.gameObject, null);
        }
        StopAllCoroutines();
        StartCoroutine(Scroll());
    }

    private void Start()
    {
        if (scrollRect)
        {
            scrollRect.content.GetComponentsInChildren(selectables);
        }
        StopAllCoroutines();
        StartCoroutine(Scroll());
    }
    
    public void InputAction(InputAction.CallbackContext _ctx)
    {
        Vector2 _delta = _ctx.ReadValue<Vector2>();
        if (_delta.y < 0.5f && _delta.y > -0.5f) return;

        if (selectables.Count > 0)
        {
            StopAllCoroutines();
            StartCoroutine(Scroll());
        }
    }

    private void Update()
    {
        scrollRect.normalizedPosition = Vector2.Lerp(scrollRect.normalizedPosition, _nextPos, Time.deltaTime * 10f);
    }

    private IEnumerator Scroll()
    {
        int selectedIndex = -1;
        
        while (selectedElement == EventSystem.current.currentSelectedGameObject) yield return null;
        
        selectedElement = EventSystem.current.currentSelectedGameObject;

        if (selectedElement.name != "Item")
        {
            string[] _res = selectedElement.name.Split(':');

            if (_res.Length != 1)
            {
                string _resString = _res[1].Trim();
        
                if (selectedElement)
                {
                    for (int i = 0; i < optionsMenuHandler.resolutions.Count; i++)
                    {
                        if (string.Equals(optionsMenuHandler.resolutions[i], _resString))
                        {
                            selectedIndex = 10 - i;
                        }
                    }
                }

                if (selectedIndex > -1)
                {
                    _nextPos = new Vector2(0, (float)selectedIndex / (selectables.Count-1));
                    scrollRect.verticalNormalizedPosition = _nextPos.y;
                }
            }
        }
    }
}
