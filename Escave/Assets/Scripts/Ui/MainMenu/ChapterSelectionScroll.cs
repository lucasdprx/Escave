using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ChapterSelectionScroll : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private List<Selectable> selectables;
    [SerializeField] private GameObject chapterSelection;
    [SerializeField] private Button _backBtn;
    
    [SerializeField] private EventSystem _eventSystem;
    
    private Vector2 _nextPos;
    private ScrollRect scrollRect;
    private GameObject selectedElement;
    
    public bool _canMove;

    private void Awake()
    {
        selectedElement = selectables[0].gameObject;
        _nextPos = new Vector2(0, 1);
        scrollRect = GetComponent<ScrollRect>();
        foreach (var _action in playerInput.actionEvents)
        {
            if (_action.actionName.Contains("Move"))
            {
                _action.AddListener(InputAction);
            }
        }
    }

    public void SetCanMove(bool canMove)
    {
        _canMove = canMove;
    }
    
    public void InputAction(InputAction.CallbackContext _ctx)
    {
        if (!chapterSelection.activeSelf) return;
        if (!_canMove) return;
        
        Vector2 _delta = _ctx.ReadValue<Vector2>();
        
        if (_delta.x < -0.5f)
        {
            BackBtnScript _backBtnScript = _backBtn.GetComponent<BackBtnScript>();
            _backBtnScript.OnLeftInput(selectedElement);
        }

        if (!_eventSystem.enabled) return;
        if (_eventSystem.currentSelectedGameObject == _backBtn.gameObject) return;
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
        if (EventSystem.current.currentSelectedGameObject != _backBtn.gameObject)
        {
            selectedElement = EventSystem.current.currentSelectedGameObject;
            
            for (int i = 0; i < selectables.Count; i++)
            {
                if (selectables[i].gameObject == selectedElement)
                {
                    selectedIndex = (selectables.Count-1) - i;
                }
            }
        
            if (selectedIndex > -1)
            {
                _nextPos = new Vector2(0, (float)selectedIndex / (selectables.Count-1));
            }
        }
    }
}
