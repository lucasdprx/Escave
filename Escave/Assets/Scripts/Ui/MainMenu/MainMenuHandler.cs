using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    public CanvasGroup blackScreen;

    [Space(10)] 
    [Header("Times")] 
    public float lerpTime;
    
    public void QuitGame()
    {
        Application.Quit();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("PioletWallJump");
    }

    private void Start()
    {
        StartCoroutine(FadeOutAnim(blackScreen));
    }

    public void FadeIn(CanvasGroup _canvasGroup)
    {
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
        _canvasGroup.interactable = false;
        float _elapsedTime = 0;

        while (_elapsedTime < lerpTime)
        {
            _canvasGroup.alpha = Mathf.Lerp(1f, 0f, _elapsedTime / lerpTime);
            _elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        _canvasGroup.alpha = 0f;
        _canvasGroup.gameObject.SetActive(false);
        _canvasGroup.interactable = true;
    }

    private IEnumerator FadeInAnim(CanvasGroup _canvasGroup)
    {
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
    }
}
