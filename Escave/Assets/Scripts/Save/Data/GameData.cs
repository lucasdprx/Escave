using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class GameData
{
    //----------Data to save----------
    public int deathCount;
    public float inGameTimer;

    public Vector2 playerPos;
    public Vector3 cameraPos;

    public List<bool> collectibles;
    public int collectiblesCollected;

    public bool isHookEnabled;
    public bool isPioletEnabled;
    //---------------------------------
    
    public GameData()
    {
        deathCount = 0;
        inGameTimer = 0f;
        
        playerPos = Vector2.zero;
        cameraPos = Vector3.zero;
        cameraPos.z = -10;
        
        collectibles = new List<bool>();
        
        isHookEnabled = false;
        isPioletEnabled = false;
    }
}
