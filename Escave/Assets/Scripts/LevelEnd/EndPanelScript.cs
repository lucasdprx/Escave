using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndPanelScript : MonoBehaviour
{
    #region Time
    [SerializeField] private TextMeshProUGUI timerText;
    private int _secondsPassInLevel;
    private int _minutesPassInLevel;
    private int _hoursPassInLevel;
    private int _millisecondsPassInLevel;
    #endregion
    
    #region Inspector
    
    [SerializeField] private CanvasGroup backgroundImage;
    
    [SerializeField] private TextMeshProUGUI deathCountText;
    [SerializeField] private TextMeshProUGUI heartText;
    [SerializeField] private TextMeshProUGUI titleText;
    
    [Space(10)]
    [SerializeField] private string title;
    
    [Space(10)]
    [SerializeField] private float timerAnimTime;
    [SerializeField] private float heartAnimTime;
    [SerializeField] private float deathAnimTime;
    [SerializeField] private float titleAnimTime;
    [SerializeField] private float buttonAnimTime;
    [SerializeField] private float backgroundAnimTime;
    
    [SerializeField] private TimerUIManager timerUIManager;
    
    [Space(10)]
    [SerializeField] private Image nextLevelBtn;
    [SerializeField] private Image mainMenuBtn;
    
    #endregion

    private string timerString;
    private string heartString;
    private string deathString;

    private char[] timerStringArray;
    private char[] heartStringArray;
    private char[] deathStringArray;
    private char[] titleStringArray;

    public void InitializeEndPanelScript(ref GameData _gameData)
    {
        backgroundImage.interactable = true;
        backgroundImage.blocksRaycasts = true;
        
        deathCountText.text = "";
        heartText.text = "";
        timerText.text = "";
        titleText.text = "";
        nextLevelBtn.color = new Color32(255, 255, 255, 0);
        mainMenuBtn.color = new Color32(255, 255, 255, 0);
        backgroundImage.alpha = 0;
        
        timerString = LoadTimerText(timerUIManager._timePassInLevel);
        heartString = LoadDeathText(_gameData.deathCount);
        deathString = LoadHeartText(_gameData.collectibles);
        
        timerStringArray = timerString.ToCharArray();
        heartStringArray = heartString.ToCharArray();
        deathStringArray = deathString.ToCharArray();
        titleStringArray = title.ToCharArray();
        
        StartCoroutine(ShowPanel());
        StartCoroutine(ShowTitle());
        StartCoroutine(ShowTimer());
        StartCoroutine(ShowDeath());
        StartCoroutine(ShowHearts());
        StartCoroutine(ShowButtonOne());
        StartCoroutine(ShowButtonTwo());
        
        GetComponent<CanvasSelectionButton>().SelectThing();
    }

    public void LoadScene(string _scene)
    {
        SceneManager.LoadScene(_scene);
    }

    private string LoadTimerText(float _timePassInLevel)
    {
        _millisecondsPassInLevel = (int)(_timePassInLevel % (int)_timePassInLevel * 100);
        if(_millisecondsPassInLevel < 0) _millisecondsPassInLevel = 0;
        _secondsPassInLevel = (int)_timePassInLevel % 60;
        _minutesPassInLevel = (int)_timePassInLevel / 60;
        _minutesPassInLevel %= 60;
        _hoursPassInLevel = (int)_timePassInLevel / 3600;
            
        return "Time: " + _hoursPassInLevel + "h" + _minutesPassInLevel + "m" + _secondsPassInLevel + "s" + _millisecondsPassInLevel;
    }
    private string LoadDeathText(int _deathCount)
    {
        return "Death count: " + _deathCount;
    }
    private string LoadHeartText(List<bool> _hearts)
    {
        int heartCount = 0;
        int maxHeartCount = _hearts.Count;
        foreach (bool _heart in _hearts){
            if (_heart)
                heartCount++;
        }
        
        return "Heart: " + heartCount + "/" + maxHeartCount;
    }
    
    private IEnumerator ShowPanel()
    {
        float _elapsedTime = 0f;
        
        while (_elapsedTime < backgroundAnimTime)
        {
            backgroundImage.alpha = Mathf.Lerp(backgroundImage.alpha, 1f, _elapsedTime / backgroundAnimTime);
            _elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        backgroundImage.alpha = 1;
    }

    private IEnumerator ShowTitle()
    {
        float titleInterval = titleAnimTime / titleStringArray.Length;
        
        float _elapsedTime = 0f;
        int _index = 0;
        
        while (_index < titleStringArray.Length)
        {
            if (_elapsedTime >= titleInterval)
            {
                titleText.text += titleStringArray[_index];
                Debug.Log(titleStringArray[_index]);
                _index++;
                _elapsedTime -= titleInterval;
            }
            _elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    
    private IEnumerator ShowTimer()
    {
        float _elapsedTime = 0f;
        int _index = 0;
        
        float timerInterval = timerAnimTime / timerStringArray.Length;
        
        while (_index < timerStringArray.Length)
        {
            if (_elapsedTime >= timerInterval)
            {
                timerText.text += timerStringArray[_index];
                _index++;
                _elapsedTime -= timerInterval;
            }
            _elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    
    private IEnumerator ShowDeath()
    {
        float _elapsedTime = 0f;
        int _index = 0;
        
        float deathInterval = deathAnimTime / deathStringArray.Length;
        
        while (_index < deathStringArray.Length)
        {
            if (_elapsedTime >= deathInterval)
            {
                deathCountText.text += deathStringArray[_index];
                _index++;
                _elapsedTime -= deathInterval;
            }
            _elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    
    private IEnumerator ShowHearts()
    {
        float _elapsedTime = 0f;
        int _index = 0;
        
        float heartInterval = heartAnimTime / heartStringArray.Length;
        
        while (_index < heartStringArray.Length)
        {
            if (_elapsedTime >= heartInterval)
            {
                heartText.text += heartStringArray[_index];
                _index++;
                _elapsedTime -= heartInterval;
            }
            _elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    
    private IEnumerator ShowButtonOne()
    {
        float _elapsedTime = 0f;
        
        while (_elapsedTime < buttonAnimTime)
        {
            float nextBtnAlpha = Mathf.Lerp(nextLevelBtn.color.a, 1f, _elapsedTime / buttonAnimTime);
            Color nextBtnColor = new Color(255, 255, 255, nextBtnAlpha);
            nextLevelBtn.color = nextBtnColor;
            _elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    
    private IEnumerator ShowButtonTwo()
    {
        float _elapsedTime = 0f;
        
        while (_elapsedTime < buttonAnimTime)
        {
            float mainBtnAlpha = Mathf.Lerp(mainMenuBtn.color.a, 1f, _elapsedTime / buttonAnimTime);
            Color mainBtnColor = new Color(255, 255, 255, mainBtnAlpha);
            mainMenuBtn.color = mainBtnColor;
            _elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
