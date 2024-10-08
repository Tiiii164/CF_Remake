using UnityEngine;
using UnityEngine.UI;

public class DifficultySelector1 : MonoBehaviour
{
    public void OnEasyButtonClicked()
    {
        PlayerPrefs.SetInt("SelectedDifficulty", 1); // Độ khó 1 cho Easy
        Debug.Log("Selected Difficulty: Easy");
    }

    public void OnMediumButtonClicked()
    {
        PlayerPrefs.SetInt("SelectedDifficulty", 2); // Độ khó 2 cho Medium
        Debug.Log("Selected Difficulty: Medium");
    }

    public void OnHardButtonClicked()
    {
        PlayerPrefs.SetInt("SelectedDifficulty", 3); // Độ khó 3 cho Hard
        Debug.Log("Selected Difficulty: Hard");
    }
}
