using UnityEngine;

public class CollectibleData
{
    public bool HasBeenCollected { get; private set; }

    public void PickUp()
    {
        HasBeenCollected = true;
    }

    public void SetData(bool hasBeenCollected)
    {
        HasBeenCollected = hasBeenCollected;
    }
}