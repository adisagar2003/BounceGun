using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject GameOverPanel;
    [SerializeField] private bool isPaused = false;
    private void Awake()
    {
        
        if (GameOverPanel == null)
        {
        }
        else
        {
            GameOverPanel.SetActive(false);
        }

    }

    private void OnEnable()
    {
        PlayerHealth.OnDeathEvent += ShowGameOverPanel;
    }

    private void ShowGameOverPanel()
    {
        if (!GameOverPanel) return;
        GameOverPanel?.SetActive(true);
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    #region Main Menu
    public void StartGame()
    {
        SceneManager.LoadScene("PlayerMovementTesting");
    }

    public void EndGame()
    {
        Application.Quit();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isPaused)
            {
                Time.timeScale = 1;
                isPaused = false;   
            }

           else
            {
                isPaused = true;
                Time.timeScale = 0;
            }
         
        }


    }
    #endregion
}
