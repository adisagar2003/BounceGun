using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : BasePowerup
{
    public delegate void AddPowerUp(int amount);
    public static event AddPowerUp OnAddAmmo;

    [SerializeField] private int ammo = 10;

    protected override void PowerUpFunctionality()
    {
        OnAddAmmo?.Invoke(ammo);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
