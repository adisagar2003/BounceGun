using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class RestPlayerPosition : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject FPController;
    private void OnTriggerEnter(Collider other)
    {
       
       if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(1);
        }
    }
}
