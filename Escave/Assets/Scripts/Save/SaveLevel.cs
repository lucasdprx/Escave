using System.Collections.Generic;
using UnityEngine;
public class SaveLevel : MonoBehaviour
{
    [SerializeField] private List<GameObject> _levels;

    private void Start()
    {
        GameObject nearestLevel = GetNearestLevel(DataPersistenceManager.instance.gameData.cameraPos);
        nearestLevel.SetActive(true);
    }
    private GameObject GetNearestLevel(Vector3 pos)
    {
        GameObject nearestLevel = _levels[0];
        float minDistance = Vector3.Distance(pos, nearestLevel.transform.position);

        foreach (GameObject level in _levels)
        {
            float distance = Vector3.Distance(pos, level.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestLevel = level;
            }
        }

        return nearestLevel;
    }
}
