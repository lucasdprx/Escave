using TMPro;
using UnityEngine;

[System.Serializable]
public class GameData
{
    //----------Data to save----------
    public int deathCount;
    public float inGameTimer;

    public Transform checkpoint;
    //---------------------------------
    
    public GameData()
    {
        deathCount = 0;
        inGameTimer = 0f;
        
        checkpoint = null;
    }
}
