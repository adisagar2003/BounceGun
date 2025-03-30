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
        DontDestroyOnLoad(this.gameObject);
        
        if (GameOverPanel == null)
        {
            Debug.LogError("Game over Panel is not assigned");
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
        GameOverPanel.SetActive(true);
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
                Debug.Log("Pause Game");
                isPaused = true;
                Time.timeScale = 0;
            }
         
        }


    }
    #endregion
}
