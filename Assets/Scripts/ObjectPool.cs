using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : Component
{
    private Queue<GameObject> _pool = new Queue<GameObject>();
    private GameObject _prefab;
    private int _poolSize;
    private Transform _parent;

    public ObjectPool(GameObject prefab, int size, Transform parent)
    {
        _prefab = prefab;
        _poolSize = size;
        _parent = parent;
        InitializePool();
    }

    private void InitializePool()
    {
        for (int i = 0; i < _poolSize; i++)
        {
            GameObject obj = UnityEngine.Object.Instantiate(_prefab, _parent);
            obj.transform.localScale = _prefab.transform.localScale;
            obj.SetActive(false);
            _pool.Enqueue(obj);
        }
    }

    public GameObject GetObject()
    {
        if (_pool.Count > 0)
        {
            GameObject obj = _pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        return null;
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        _pool.Enqueue(obj);

        var cube = obj.GetComponent<Cube>();
        cube.SetColorChanged(false);
        cube.SetLifeTime(0);
        cube.SetLifeTime(0);
        cube.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
    public List<GameObject> GetAllObjects()
    {
        return new List<GameObject>(_pool);
    }
}
