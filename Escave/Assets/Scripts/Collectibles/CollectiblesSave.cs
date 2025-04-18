using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollectiblesSave : MonoBehaviour, IDataPersistence
{
    [SerializeField] private List<CollectObjet> collectibles;
    [SerializeField] private TextMeshProUGUI _fraiseText;

    private List<bool> _temp;
    
    private int valueCollected;

    private void Awake()
    {
        CollectObjet.OnCollectibleCollected += OnCollectibleCollected;
    }

    public void LoadData(GameData _gameData)
    {
        Debug.Log("Loading Collectibles");
        
        valueCollected = _gameData.collectiblesCollected;
        UpdateText();

        if (_gameData.collectibles.Count == 0)
        {
            foreach (CollectObjet collectObjet in collectibles)
            {
                _gameData.collectibles.Add(false);
            }
        }
        
        for (int i = 0; i < _gameData.collectibles.Count; i++)
        {
            if (collectibles.Count <= i) continue;
            
            print("Initialize");
            collectibles[i].Initialize(_gameData.collectibles[i]);
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

    public void Save()
    {
        _temp = new List<bool>();
        
        for (int i = 0; i < collectibles.Count; i++)
        {
            if(collectibles[i].collectibleData != null)
                _temp.Add(collectibles[i].collectibleData.HasBeenCollected);
        }
    }

    public void SaveData(ref GameData _gameData)
    {
        _gameData.collectiblesCollected = valueCollected;
        _gameData.collectibles = _temp;
    }
}
