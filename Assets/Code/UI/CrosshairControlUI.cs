using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Changes crosshair according to the objects..
/// </summary>
public class CrosshairControlUI : MonoBehaviour
{
    private Image crosshairImage;

    private void Start()
    {
        crosshairImage = GetComponent<Image>();    
    }

    private void OnEnable()
    {
        PlayerGun.OnEnemyDetection += TurnCrosshairRed;
        PlayerGun.OnNoEnemyDetection += TurnCrosshairNormal;
    }

    private void OnDisable()
    {
        PlayerGun.OnEnemyDetection -= TurnCrosshairRed;
        PlayerGun.OnNoEnemyDetection -= TurnCrosshairNormal;

    }

    private void TurnCrosshairRed()
    {
        if (!crosshairImage)
        {
            Debug.LogError("No crosshair Image");
            return;
        }
        crosshairImage.color = Color.red;
    }

    private void TurnCrosshairNormal()
    {
        if (!crosshairImage)
        {
            Debug.LogError("No crosshair Image");
            return;
        }
        crosshairImage.color = Color.white;
       
    }
}
