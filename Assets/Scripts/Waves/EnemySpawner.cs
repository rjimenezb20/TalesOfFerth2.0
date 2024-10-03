using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    private ObjectPool ObjectPool;

    private void Start()
    {
        ObjectPool = FindAnyObjectByType<ObjectPool>();
    }

    public void SpawnEnemy(string enemyType)
    {
        GameObject enemy = null;

        enemy = ObjectPool.GetPooledObject(enemyType);
        enemy.transform.position = transform.position;
        enemy.GetComponent<NavMeshAgent>().enabled = true;
        enemy.SetActive(true);
    }

    public void SpawnEnemyWithTarget(string enemyType, Transform target)
    {
        GameObject enemy = null;
        
        enemy = ObjectPool.GetPooledObject(enemyType);
        enemy.transform.position = transform.position;
        enemy.GetComponent<NavMeshAgent>().enabled = true;
        enemy.SetActive(true);
        enemy.GetComponent<Enemy>().SetTarget(target);
    }
}