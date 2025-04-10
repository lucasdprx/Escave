using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CreditsCreatorDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _role;

    [SerializeField] private string _link;
    
    public void OnPointerEnter(PointerEventData _eventData)
    {
        _name.gameObject.SetActive(true);
        _role.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData _eventData)
    {
        _name.gameObject.SetActive(false);
        _role.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData _eventData)
    {
        Application.OpenURL(_link);
    }
}
