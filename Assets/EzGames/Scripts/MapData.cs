using UnityEngine;

[CreateAssetMenu(fileName = "MapData", menuName = "ScriptableObjects/MapData", order = 1)]
public class MapData : ScriptableObject
{
    public string mapName;
    public int difficultyLevel;
    public EnvironmentObjectSO[] environmentObjects;  
    public int numberOfObjectsToSpawn;  
    public SpawnAreaSO[] spawnAreas;    
    public CatSO[] cats;
}
