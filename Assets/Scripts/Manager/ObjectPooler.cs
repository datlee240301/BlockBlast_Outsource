using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string Tag;
        public GameObject Prefab;
        public int Size;
    }

    public static ObjectPooler Instance;
    [HideInInspector] public Transform Root;
    public List<Pool> Pools;
    private Dictionary<string, Queue<GameObject>> _poolDictionary;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Root = this.transform;
        _poolDictionary = new Dictionary<string, Queue<GameObject>>();
        foreach (Pool item in Pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < item.Size; i++)
            {
                GameObject obj = Instantiate(item.Prefab, this.transform);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            _poolDictionary.Add(item.Tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position)
    {
        GameObject objectToSpawn = _poolDictionary[tag].Dequeue();
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        _poolDictionary[tag].Enqueue(objectToSpawn);
        return objectToSpawn;
    }

    public void MoveToPool()
    {
        foreach (var pool in _poolDictionary)
        {
            foreach (GameObject obj in pool.Value)
            {
                obj.SetActive(false);
            }
        }
    }
}