using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject GameOverPanel;

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
}
