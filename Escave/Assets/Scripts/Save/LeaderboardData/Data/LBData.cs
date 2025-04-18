using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LBData
{
    //----------Data to save----------
    public List<float> timers;
    //---------------------------------
    
    public LBData()
    {
        timers = new List<float>();
    }
}
