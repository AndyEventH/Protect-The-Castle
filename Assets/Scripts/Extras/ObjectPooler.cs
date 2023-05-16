using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int poolSize = 10;
    private List<GameObject> pool;
    private GameObject poolContainer;

    private void Awake()
    {
        pool = new List<GameObject>();
        poolContainer = new GameObject($"Pool - {prefab.name}");
        CreatePooler();
    }
    private void CreatePooler()
    {
        for (int i = 0; i < poolSize; i++)
        {
            pool.Add(CreateInstance());
        }
    }
    private GameObject CreateInstance()
    {
        GameObject newInstance = Instantiate(prefab);
        newInstance.transform.SetParent(poolContainer.transform);
        newInstance.SetActive(false);
        return newInstance;
    }
    public GameObject GetInstanceFromPool()
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                return pool[i];
            }
        }
        return CreateInstance();
    }
    public static void ReturnToPool(GameObject instance)
    {
        instance.SetActive(false);
    }
    public static IEnumerator ReturnToPoolWithDelay(GameObject instance, float delay)
    {
        yield return new WaitForSeconds(delay);
        instance.SetActive(false);
    }
}
