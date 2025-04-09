using TMPro;
using UnityEngine;

[System.Serializable]
public class GameData
{
    //----------Data to save----------
    public int deathCount;
    public float inGameTimer;

    public Vector2 playerPos;
    //---------------------------------
    
    public GameData()
    {
        deathCount = 0;
        inGameTimer = 0f;
        
        playerPos = Vector2.zero;
    }
}
