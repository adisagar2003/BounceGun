using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : BaseThrowable, IInteractable
{

    private void Awake()
    {
        Debug.Log("Knife Awakened");
    }

    public void Interact()
    {
        Debug.Log("Future interactions");
    }
    
}
