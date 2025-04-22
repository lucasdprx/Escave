using System.Collections.Generic;
using UnityEngine;

public class KeybindsLoader : MonoBehaviour
{
    [SerializeField] private List<RebindSaveLoad> rebinds;

    private void Start()
    {
        foreach (RebindSaveLoad rebind in rebinds)
        {
            rebind.Load();
        }
    }
}
