using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Put in Triggers
/// On Trigger Enter, Spawn enemies in random areas
/// Need to assign transforms manually for spawn points and list of enemies
/// inside the system
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemiesToSpawn;
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private bool isEnemySpawned;

    private int spawnPointIndex = 0;
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
                // make the enemy spawn
                Instantiate(enemy, spawnPoints[spawnPointIndex++].position, Quaternion.identity);
            }

        }
    }
}
