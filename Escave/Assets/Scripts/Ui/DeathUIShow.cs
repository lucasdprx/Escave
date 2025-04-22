using TMPro;
using UnityEngine;

public class DeathUIShow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _deathText;
    [SerializeField] private PlayerDeath _playerDeath;

    private void Start()
    {
        _playerDeath.OnDeath.AddListener(ShowDeathCounter);
        ShowDeathCounter(DataPersistenceManager.instance.gameData.deathCount);
    }

    private void ShowDeathCounter(int deathCount)
    {
        _deathText.text = deathCount.ToString();
    }
}
