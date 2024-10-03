using System.Collections.Generic;
using UnityEngine;

public class ShelterController : MonoBehaviour
{
    private int enemiesSpawned;
    private float timer;

    public float timeBetweenSpawn;
    public List<EnemySpawner> spawners;

    private void Update()
    {
        timer += Time.deltaTime % 60;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Unit" || other.gameObject.tag == "Building")
            if (timer >= timeBetweenSpawn)
            {
                foreach (var spawner in spawners)
                {
                    if (enemiesSpawned < spawners.Count)
                    {
                        spawner.SpawnEnemyWithTarget("warrior", other.gameObject.transform);
                        enemiesSpawned++;
                    }
                }

                enemiesSpawned = 0;
                timer = 0;
            }
    }
}