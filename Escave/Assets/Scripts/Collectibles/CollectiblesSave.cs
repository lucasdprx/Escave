using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollectiblesSave : MonoBehaviour, IDataPersistence
{
    [SerializeField] private List<CollectObjet> collectibles;
    [SerializeField] private TextMeshProUGUI _fraiseText;
    
    private int valueCollected;
    
    public void LoadData(GameData _gameData)
    {
        valueCollected = _gameData.collectiblesCollected;
        UpdateText();
        
        if (_gameData.collectibles.Count == 0) return;
        
        for (int i = 0; i < _gameData.collectibles.Count; i++)
        {
            if (collectibles[i].collectibleData != null)
                collectibles[i].collectibleData.SetData(_gameData.collectibles[i]);
        }

        foreach (CollectObjet _collectObjet in collectibles)
        {
            if (_collectObjet.collectibleData == null) continue;
            
            if (_collectObjet.collectibleData.HasBeenCollected)
            {
                _collectObjet.gameObject.SetActive(false);
            }
            else if(!_collectObjet.collectibleData.HasBeenCollected)
            {
                _collectObjet.gameObject.SetActive(true);
            }
        }
    }

    public void OnCollectibleCollected()
    {
        valueCollected++;
        UpdateText();
    }

    private void UpdateText()
    {
        _fraiseText.text = valueCollected.ToString();
    }

    public void SaveData(ref GameData _gameData)
    {
        _gameData.collectiblesCollected = valueCollected;
        _gameData.collectibles = new List<bool>();

        for (int i = 0; i < collectibles.Count; i++)
        {
            _gameData.collectibles.Add(collectibles[i].collectibleData.HasBeenCollected);
        }
    }
}
