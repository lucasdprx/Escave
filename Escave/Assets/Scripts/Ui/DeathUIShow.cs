using System;
using TMPro;
using UnityEngine;

public class DeathUIShow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _deathText;
    [SerializeField] private PlayerDeath _playerDeath;

    private void Start()
    {
        _playerDeath.OnDeath.AddListener(ShowDeathCounter);
    }

    private void ShowDeathCounter(int deathCount)
    {
        Debug.Log(deathCount);
        _deathText.text = "Number of death: " + deathCount;
    }
}
