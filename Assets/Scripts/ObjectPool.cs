using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    private Stack<T> _pool = new Stack<T>();
    private GameObject _prefab;
    private int _poolSize;
    private Transform _parent;

    public ObjectPool(GameObject prefab, int poolSize, Transform parent)
    {
        _prefab = prefab;
        _poolSize = poolSize;
        _parent = parent;

        for (int i = 0; i < _poolSize; i++)
        {
            T obj = UnityEngine.Object.Instantiate(_prefab, _parent).GetComponent<T>();
            obj.gameObject.SetActive(false);
            _pool.Push(obj);
        }
    }

    public T GetObject()
    {
        if (_pool.Count > 0)
        {
            T obj = _pool.Pop();
            obj.gameObject.SetActive(true);
            return obj;
        }
        return null;
    }

    public void ReturnObject(T obj)
    {
        if (obj != null && obj.gameObject.activeSelf)
        {
            obj.gameObject.SetActive(false);
            _pool.Push(obj);
        }
    }
}