using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject GameOverPanel;

    private void OnEnable()
    {
        PlayerHealth.OnDeathEvent += ShowGameOverPanel;
    }

    private void ShowGameOverPanel()
    {
        GameOverPanel.SetActive(true);
    }
}
