using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Shows UI for Ammo
/// Dependencies: 
///     -- Player Gun: For ammo data;
/// </summary>
public class AmmoUI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private TextMeshProUGUI ReloadUI;
    [SerializeField] private PlayerGun playerGun;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ReloadUI.text = $"{playerGun.currentAmmoOnClip} / {playerGun.ammoNotInClip}";
    }
}
