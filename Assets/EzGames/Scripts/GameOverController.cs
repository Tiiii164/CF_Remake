using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverController : MonoBehaviour
{
    [SerializeField] GameObject gameOverPanel;

    public void StopGame()
    {
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true); 
    }
}
