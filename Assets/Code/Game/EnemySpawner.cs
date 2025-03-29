using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Put in Triggers
/// On Trigger Enter, Spawn enemies in random areas 
/// inside the system
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemiesToSpawn;
    [SerializeField] private bool isEnemySpawned;

    // Start is called before the first frame update
    void Start()
    {    
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /// Migrate to a helper namespace
    private Vector3 RandomPointInBounds(Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            -8.73f,
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isEnemySpawned)
        {
            // disable renable 
            isEnemySpawned = true;

            foreach (GameObject enemy in enemiesToSpawn)
            {
                Debug.Log(RandomPointInBounds(other.bounds));
                enemy.transform.position = Random.insideUnitSphere * 5;
                // reset y position to ground.
                enemy.transform.position = new Vector3(enemy.transform.position.x, transform.position.y, transform.position.z);
                Instantiate(enemy, enemy.transform.position, Quaternion.identity);
            }

        }
    }
}
