using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRangeSystem : MonoBehaviour
{
    public float detectionRadius;
    public string layerInteraction;

    private List<GameObject> enemiesOnRange;
    private Collider[] enemiesDetected = new Collider[20];
    private int enemiesNumber = 0;
    private Transform target;
    private int enemyLayer;

    private void Awake()
    {
        enemiesOnRange = new List<GameObject>();
        enemyLayer = LayerMask.GetMask(layerInteraction);
    }

    void Start()
    {
        StartCoroutine(CheckEnemiesOnRange());
    }
    
    void Update()
    {
        enemiesNumber = Physics.OverlapSphereNonAlloc(transform.position, detectionRadius, enemiesDetected, enemyLayer);
    }

    IEnumerator CheckEnemiesOnRange()
    {
        while (true)
        {
            yield return new WaitUntil(() => enemiesNumber != enemiesOnRange.Count);
            UpdateEnemyList();
        }
    }

    private void UpdateEnemyList()
    {
        if (enemiesNumber >= enemiesOnRange.Count)
        {
            for (int i = 0; i < enemiesDetected.Length; i++)
            {
                if (enemiesDetected[i] != null)
                {
                    GameObject enemy = enemiesDetected[i].gameObject;

                    if (!enemiesOnRange.Contains(enemy))
                        enemiesOnRange.Add(enemy);
                }
            }
        }
        else
        {
            for (int i = enemiesOnRange.Count - 1; i >= 0; i--)
            {
                if (enemiesOnRange[i] == null)
                {
                    enemiesOnRange.RemoveAt(i);
                }
                else if (CheckDistanceToEnemy(enemiesOnRange[i].transform) > detectionRadius)
                {
                    enemiesOnRange.Remove(enemiesOnRange[i]);
                }
            }
        }
    }

    private Transform GetClosestEnemy()
    {
        float minDistance = detectionRadius;

        foreach (GameObject enemy in enemiesOnRange)
        {
            if (enemy != null)
            {
                float distance = CheckDistanceToEnemy(enemy.transform);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    target = enemy.transform;
                }
            }
        }
        return target;
    }

    private float CheckDistanceToEnemy(Transform enemy)
    {
        float distance = Vector3.Distance(transform.position, enemy.transform.position);
        return distance;
    }

    public Transform SetTarget()
    {
        if (enemiesOnRange.Count == 0)
        {
            return null;
        }
        else
        {
            return GetClosestEnemy(); //EFICIENCIA?
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
