using UnityEngine;
using UnityEngine.SceneManagement;

public class ChapterSelectionManager : MonoBehaviour
{
    private AudioManager _audioManager;
    
    public void LoadChapters(string _levelName)
    {
        SceneManager.LoadScene(_levelName);
        _audioManager.PlaySound(AudioType.levelStart);
    }

    public void Start()
    {
        _audioManager = AudioManager.Instance;
    }
}
