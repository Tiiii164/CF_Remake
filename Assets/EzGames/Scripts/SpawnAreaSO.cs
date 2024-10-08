using UnityEngine;

[CreateAssetMenu(fileName = "SpawnArea", menuName = "ScriptableObjects/SpawnArea", order = 1)]
public class SpawnAreaSO : ScriptableObject
{
    public Transform[] spawnPoints;  // Các điểm spawn dùng Transform
    public int[] numberOfCatsPerPoint; // Số lượng mèo sẽ spawn tại mỗi điểm

    // Đảm bảo số lượng mèo tương ứng với số điểm spawn
    public int GetTotalCatsToSpawn()
    {
        int totalCats = 0;
        foreach (int num in numberOfCatsPerPoint)
        {
            totalCats += num;
        }
        return totalCats;
    }
}
