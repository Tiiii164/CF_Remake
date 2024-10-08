using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultySelector : MonoBehaviour
{
    public void OnDifficultySelected(int difficultyLevel)
    {
        PlayerPrefs.SetInt("SelectedDifficulty", difficultyLevel);
        SceneManager.LoadScene("RescueCats");
    }
}
