using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    //----------Data to save----------
    public int deathCount;

    public List<Vector2> playerPos;
    public List<Vector3> cameraPos;

    public List<bool> collectibles;
    public int collectiblesCollected;

    public bool isHookEnabled;
    public bool isPioletEnabled;

    public List<bool> chaptersFinished;

    public List<float> timers;
    //---------------------------------
    
    public GameData()
    {
        deathCount = 0;
        
        timers = new List<float>();
        
        playerPos = new List<Vector2>();
        playerPos.Add(Vector2.zero);
        cameraPos = new List<Vector3>();
        cameraPos.Add(new Vector3(0, 0, -10));
        
        collectibles = new List<bool>();
        
        isHookEnabled = false;
        isPioletEnabled = false;
        
        chaptersFinished = new List<bool>(2);
    }
}
