using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    //----------Data to save----------
    public int deathCount;

    public Vector2 playerPos;
    public Vector3 cameraPos;

    public List<bool> collectibles;
    public int collectiblesCollected;

    public bool isHookEnabled;
    public bool isPioletEnabled;

    public bool chapterUnlocked;
    public bool buttonStateSet;

    public float timer;
    //---------------------------------
    
    public GameData()
    {
        deathCount = 0;

        timer = 0;
        
        playerPos = Vector2.zero;
        cameraPos = Vector3.zero;
        
        collectibles = new List<bool>();
        
        isHookEnabled = false;
        isPioletEnabled = false;
        
        chapterUnlocked = false;
        buttonStateSet = false;
    }
}
