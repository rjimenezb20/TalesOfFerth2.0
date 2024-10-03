using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRangeSystem : MonoBehaviour
{
    public float detectionRadius;
    public LayerMask layerInteraction;

    private List<GameObject> enemiesOnRange;
    private Collider[] enemiesDetected = new Collider[20];
    private int enemiesNumber = 0;
    private Transform target;

    private void Awake()
    {
        enemiesOnRange = new List<GameObject>();
    }

    void Start()
    {
        StartCoroutine(CheckEnemiesOnRange());
        InvokeRepeating(nameof(CheckEnemies), 0f, 0.1f);
    }

    void CheckEnemies()
    {
        enemiesNumber = Physics.OverlapSphereNonAlloc(transform.position, detectionRadius, enemiesDetected, layerInteraction);

        enemiesOnRange.Clear(); 
        for (int i = 0; i < enemiesNumber; i++)
        {
            if (enemiesDetected[i] != null)
            {
                enemiesOnRange.Add(enemiesDetected[i].gameObject);
            }
        }
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
        HashSet<GameObject> currentEnemies = new HashSet<GameObject>();

        for (int i = 0; i < enemiesNumber; i++)
        {
            if (enemiesDetected[i] != null)
            {
                GameObject enemy = enemiesDetected[i].gameObject;

                if (!enemiesOnRange.Contains(enemy))
                    enemiesOnRange.Add(enemy);

                currentEnemies.Add(enemy);
            }
        }

        enemiesOnRange.RemoveAll(enemy => !currentEnemies.Contains(enemy) || CheckDistanceToEnemy(enemy.transform) > detectionRadius);
    }

    private Transform GetClosestEnemy()
    {
        float minDistance = detectionRadius;
        Transform closestEnemy = null;

        foreach (GameObject enemy in enemiesOnRange)
        {
            if (enemy != null)
            {
                float distance = CheckDistanceToEnemy(enemy.transform);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestEnemy = enemy.transform;
                }
            }
        }

        if (closestEnemy != null)
        {
            target = closestEnemy;
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
            return GetClosestEnemy();
        }
    }

    public void SetRange(int range)
    {
        detectionRadius = range;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}