using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    [Header("Enemy types")]
    [SerializeField] private GameObject warriorPrefab;
    [SerializeField] private GameObject brutePrefab;

    [Header("Pool size")]
    [SerializeField] private int warriorPoolSize = 100;
    [SerializeField] private int brutePoolSize = 100;

    private List<GameObject> warriorPool = new List<GameObject>();
    private List<GameObject> kingPool = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
        InitializePool(warriorPool, warriorPoolSize, warriorPrefab);
        InitializePool(kingPool, brutePoolSize, brutePrefab);
    }

    private void InitializePool(List<GameObject> pool, int poolSize, GameObject unitPrefab)
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(unitPrefab);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    public GameObject GetPooledObject(string poolName)
    {
        List<GameObject> pool = null;
        GameObject prefabToUse = null;

        if (poolName == "warrior")
        {
            pool = warriorPool;
            prefabToUse = warriorPrefab;
        }
        else if (poolName == "brute")
        {
            pool = kingPool;
            prefabToUse = brutePrefab;
        }

        foreach (var obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                return obj;
            }
        }

        if (pool == null || prefabToUse == null)
            return null;

        GameObject newObj = Instantiate(prefabToUse);
        newObj.SetActive(false);
        pool.Add(newObj);
        return newObj;
    }

    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
    }
}    