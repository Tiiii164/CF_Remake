using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private MapManager mapManager;

    private MapData mapData;

    void Start()
    {
        int selectedDifficulty = PlayerPrefs.GetInt("SelectedDifficulty", 1); // Độ khó mặc định là 1

        mapData = mapManager.GetMapByDifficulty(selectedDifficulty);

        if (mapData != null)
        {
            SpawnCats();
            SpawnEnvironmentObjects();
        }
        else
        {
            Debug.LogError("Không tìm thấy MapData cho độ khó: " + selectedDifficulty);
        }
    }

    void SpawnCats()
    {
        foreach (var spawnAreaSO in mapData.spawnAreas)
        {
            for (int i = 0; i < spawnAreaSO.spawnPoints.Length; i++)
            {
                Transform spawnPoint = spawnAreaSO.spawnPoints[i];
                int catsToSpawn = spawnAreaSO.numberOfCatsPerPoint[i];

                for (int j = 0; j < catsToSpawn; j++)
                {
                    Vector3 randomPosition = spawnPoint.position + Random.insideUnitSphere * 20f;
                    randomPosition.y = spawnPoint.position.y;

                    int randomIndex = Random.Range(0, mapData.cats.Length);
                    CatSO catPrefab = mapData.cats[randomIndex];
                    Instantiate(catPrefab.catPrefab, randomPosition, Quaternion.identity);
                }
            }
        }
    }

    void SpawnEnvironmentObjects()
    {
        for (int i = 0; i < mapData.numberOfObjectsToSpawn; i++)
        {
            EnvironmentObjectSO randomObject = mapData.environmentObjects[Random.Range(0, mapData.environmentObjects.Length)];

            Vector3 randomPos = new Vector3(Random.Range(0, 1400f), 0, Random.Range(-115f, 115f));
            Instantiate(randomObject.prefab, randomPos, Quaternion.identity);
        }
    }
}
