using System.Collections.Generic;
using UnityEngine;

public class ChaptersFileData
{
    public List<bool> chaptersUnlocked;

    public ChaptersFileData()
    {
        chaptersUnlocked = new List<bool>();
        chaptersUnlocked.Add(true);
    }
}
