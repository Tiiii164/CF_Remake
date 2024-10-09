using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject playAgainButton;
   



    private void Update()
    {
        if(Time.timeScale == 0)
        {
            playAgainButton.SetActive(true);
        }
    }
    public void StopGame()
    {
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void PlayAgain()
    {
        Time.timeScale = 1;
        Debug.Log(Time.timeScale);
        SceneManager.LoadScene("UIScene");
    }
}
