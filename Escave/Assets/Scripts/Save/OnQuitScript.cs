using System;
using UnityEngine;

public class OnQuitScript : MonoBehaviour
{
    [SerializeField] private CollectiblesSave collectiblesSave;

    private void OnApplicationQuit()
    {
        OnQuit();
    }

    public void OnQuit()
    {
        collectiblesSave.LoadData(DataPersistenceManager.instance.gameData);
        DataPersistenceManager.instance.SaveGame();
    }
}
