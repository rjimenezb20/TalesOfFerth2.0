using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavesController : MonoBehaviour
{
    [Header("Enemy types")]
    public Enemy UndeadWarrior;
    public Enemy UndeadArcher;

    [Header("WavesConfig")]
    public List<Wave> waves = new List<Wave>();
    public float timeBetweenSpawn;

    /*[Header("WayPoints")]
    public List<Vector3> waypoints1 = new List<Vector3>();
    public List<Vector3> waypoints2 = new List<Vector3>();
    public List<Vector3> waypoints3 = new List<Vector3>();*/

    private Coroutine coroutine;
    private float timer;
    private int totalEnemies;
    private int enemiesSpawned;
    private int warriorCount;
    private int bruteCount;
    private int aux;

    public void WaveManagement(int day)
    {
        if (waves.Count > 0)
            if (day == waves[0].waveDayStartTime) {
                totalEnemies = waves[0].warriorCount + waves[0].bruteCount;
                coroutine = StartCoroutine(SpawnTimer());
            }
    }

    public IEnumerator SpawnTimer()
    {
        while (true)
        {
            timer += Time.deltaTime % 60;
            if (timer >= timeBetweenSpawn)
            {
                foreach (var spawner in waves[0].spawners)
                {
                    if (enemiesSpawned < totalEnemies)
                        if (aux < waves[0].spawners.Count - Mathf.Ceil(waves[0].bruteCount / totalEnemies * 10))
                        {  
                            if (warriorCount < waves[0].warriorCount)
                            {
                                spawner.SpawnEnemy("warrior");
                                warriorCount += 1;
                                enemiesSpawned += 1;
                                aux += 1;
                            } 
                            else if (bruteCount < waves[0].bruteCount)
                            {
                                spawner.SpawnEnemy("brute");
                                bruteCount += 1;
                                enemiesSpawned += 1;
                                aux += 1;
                            }
                        } 
                        else
                        {
                            if (aux >= waves[0].spawners.Count)
                                aux = 0;

                            if (bruteCount < waves[0].bruteCount)
                            {
                                spawner.SpawnEnemy("brute");
                                bruteCount += 1;
                                enemiesSpawned += 1;
                                aux += 1;
                            }
                        }
                }
                timer = 0;

                if (enemiesSpawned >= totalEnemies)
                {
                    waves.RemoveAt(0);
                    warriorCount = 0;
                    bruteCount = 0;
                    enemiesSpawned = 0;
                    aux = 0;
                    StopCoroutine(coroutine);
                }
            }
            yield return null;
        }
    }

    /*public Vector3 GetWayPoint(int ruteNumber, int wayPoint)
    {
        switch (ruteNumber)
        {
            case 1:
                if (wayPoint > waypoints1.Count)
                    return waypoints1[wayPoint];
                else
                    return waypoints1[waypoints1.Count];
            case 2:
                if (wayPoint > waypoints2.Count)
                    return waypoints2[wayPoint];
                else
                    return waypoints2[waypoints2.Count];
            case 3:
                if (wayPoint > waypoints3.Count)
                    return waypoints3[wayPoint];
                else
                    return waypoints3[waypoints3.Count];
        }

        return Vector3.zero;
    }*/
}