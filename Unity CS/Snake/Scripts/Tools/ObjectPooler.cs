using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    private static ObjectPooler _instance;
    public static ObjectPooler Instance => _instance;

    [SerializeField] private GameObject _pooledObjectPrefab = null;
    [SerializeField] private int _poolAmount = 50;
    [SerializeField] private bool _willGrow = false;

    private List<GameObject> _pooledObjects = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        if(Instance) 
            Destroy(gameObject);

        _instance = this;
        PreInstantiatePool();
    }

    private void PreInstantiatePool()
    {
        int lIndex = 0;
        GameObject lObj;
        for (lIndex = 0; lIndex < _poolAmount; ++lIndex)
        {
            lObj = Instantiate(_pooledObjectPrefab);
            lObj.SetActive(false);
            _pooledObjects.Add(lObj);
        }
    }

    public GameObject GetPooledObject()
    {
        foreach (GameObject pooledObj in _pooledObjects)
        {
            if (!pooledObj.activeInHierarchy)
                return pooledObj;
        }
        
        if (_willGrow)
        {
            GameObject lObj = Instantiate(_pooledObjectPrefab);
            _pooledObjects.Add(lObj);
            return lObj;
        }

        return null;
    }
}
