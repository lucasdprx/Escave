using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CreditsCreatorDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _role;
    
    [SerializeField] private EventSystem _eventSystem;
    private Button _button;

    [SerializeField] private string _link;
    private bool isOnPic;

    private void Start()
    {
        _button = GetComponent<Button>();
    }
    
    public void OnPointerEnter(PointerEventData _eventData)
    {
        isOnPic = true;
    }

    public void OnPointerExit(PointerEventData _eventData)
    {
        isOnPic = false;
    }

    void Update()
    {
        if (_eventSystem.currentSelectedGameObject == _button.gameObject || isOnPic)
        {
            _name.gameObject.SetActive(true);
            _role.gameObject.SetActive(true);
        } else if (!isOnPic && _eventSystem.currentSelectedGameObject != _button.gameObject)
        {
            _name.gameObject.SetActive(false);
            _role.gameObject.SetActive(false);
        }
    }

    public void OnClick()
    {
        Application.OpenURL(_link);
    }
}
