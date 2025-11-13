using UnityEngine;
using System.Collections.Generic;

public class EnemyPool : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int initialSize = 20;

    Queue<GameObject> pool = new Queue<GameObject>();

    void Start()
    {
        for (int i = 0; i < initialSize; i++)
        {
            var go = Instantiate(enemyPrefab, transform);
            go.SetActive(false);
            pool.Enqueue(go);
        }
    }

    public GameObject Get(Transform parent = null)
    {
        GameObject go;
        if (pool.Count > 0)
        {
            go = pool.Dequeue();
        }
        else
        {
            go = Instantiate(enemyPrefab, transform);
        }

        go.SetActive(true);
        if (parent != null)
            go.transform.SetParent(parent, worldPositionStays: true);
        return go;
    }

    public void Return(GameObject go)
    {
        go.SetActive(false);
        go.transform.SetParent(transform);
        pool.Enqueue(go);
    }
}
