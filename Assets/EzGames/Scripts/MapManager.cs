using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] private MapData[] allMaps;  

    public MapData GetMapByDifficulty(int difficultyLevel)
    {
        foreach (MapData map in allMaps)
        {
            if (map.difficultyLevel == difficultyLevel)
            {
                return map;  
            }
        }
        return null; 
    }
}
