using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour
{
    public CanvasGroup blackScreen;
    private AudioManager _audioManager;
    [SerializeField] private EventSystem eventSystem;

    [Space(10)] 
    [Header("Times")] 
    public float lerpTime;

    private Button _buttonToSelect;
    private CanvasGroup _canvasGroup;
    [SerializeField] private CanvasGroup _OptionsCanvas;
    [SerializeField] private Button playButton;
    
    public void QuitGame()
    {
        Application.Quit();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
        _audioManager.PlaySound(AudioType.levelStart);
    }

    public void NewGame()
    {
        DataPersistenceManager.instance.NewOptions();
        DataPersistenceManager.instance.SaveGame();
        SceneManager.LoadScene("Level1");
    }

    public void PlayUIClickSound()
    {
        _audioManager.PlaySound(AudioType.uiButton);
    }

    public void PlayUIReturnSound()
    {
        _audioManager.PlaySound(AudioType.uiReturn);
    }

    public void SelectButton(Button _button)
    {
        _buttonToSelect = _button;
    }

    public void SelectCanvasGroup(CanvasGroup _canvasGroup)
    {
        this._canvasGroup = _canvasGroup;
    }

    private void Start()
    {
        StartCoroutine(FadeOutAnim(blackScreen));
        _audioManager = AudioManager.Instance;
    }

    public void FadeIn(CanvasGroup _canvasGroup)
    {
        this._canvasGroup = _canvasGroup;
        StopAllCoroutines();
        StartCoroutine(FadeInAnim(_canvasGroup));
    }

    public void FadeOut(CanvasGroup _canvasGroup)
    {
        StopAllCoroutines();
        StartCoroutine(FadeOutAnim(_canvasGroup));
    }

    private IEnumerator FadeOutAnim(CanvasGroup _canvasGroup)
    {
        eventSystem.enabled = false;
        _canvasGroup.interactable = false;
        float _elapsedTime = 0;

        while (_elapsedTime < lerpTime)
        {
            _canvasGroup.alpha = Mathf.Lerp(1f, 0f, _elapsedTime / lerpTime);
            _elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        eventSystem.enabled = true;
        _canvasGroup.alpha = 0f;
        _canvasGroup.gameObject.SetActive(false);
        _canvasGroup.interactable = true;
        if (_buttonToSelect != null)
            _buttonToSelect.Select();
    }

    private IEnumerator FadeInAnim(CanvasGroup _canvasGroup)
    {
        eventSystem.enabled = false;
        _canvasGroup.gameObject.SetActive(true);
        _canvasGroup.interactable = false;
        float _elapsedTime = 0;

        while (_elapsedTime < lerpTime)
        {
            _canvasGroup.alpha = Mathf.Lerp(0f, 1f, _elapsedTime / lerpTime);
            _elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        _canvasGroup.alpha = 1f;
        _canvasGroup.interactable = true;
        
        eventSystem.enabled = true;
        _canvasGroup.gameObject.GetComponent<Select>().SelectThing();
    }

    public void BackCanvas(InputAction.CallbackContext _ctx)
    {
        if (_ctx.started && _canvasGroup != null)
        {
            FadeOut(_canvasGroup);
            if (_isInKeybinds)
            {
                _canvasGroup = _OptionsCanvas;
                _buttonToSelect = playButton;
            }
            else
                _canvasGroup = null;

        }
    }

    private bool _isInKeybinds;
    public void SetInKeybindsBool()
    {
        _isInKeybinds = true;
    }
}
