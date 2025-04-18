public class CollectibleData
{
    public bool HasBeenCollected;

    public void PickUp()
    {
        HasBeenCollected = true;
    }
}