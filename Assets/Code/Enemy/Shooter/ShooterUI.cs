using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShooterUI : MonoBehaviour
{
    [SerializeField] private Slider shooterHealthBarSlider;
    [SerializeField] private GameObject shooterHealthBar;
    [SerializeField] private Camera mainCamera;
    private Shooter shooter;

    // Start is called before the first frame update
    void Start()
    {
        shooter = GetComponent<Shooter>();
        mainCamera = FindFirstObjectByType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        shooterHealthBarSlider.value = shooter.GetHealth();
        UIToFaceCamera();
    }

    private void UIToFaceCamera()
    {
        shooterHealthBar.transform.LookAt(mainCamera.transform);
    }
}
