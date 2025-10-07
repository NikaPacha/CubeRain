using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    private Stack<T> _pool = new Stack<T>();
    private T _prefab;
    private int _poolSize;
    private Transform _parent;

    public ObjectPool(T prefab, int poolSize, Transform parent)
    {
        if (prefab == null)
            throw new System.ArgumentNullException(nameof(prefab), "Префаб не может быть null");

        _prefab = prefab;
        _poolSize = poolSize;
        _parent = parent;

        for (int i = 0; i < _poolSize; i++)
        {
            T obj = InstantiatePrefab();
            obj.gameObject.SetActive(false);
            _pool.Push(obj);
        }
    }

    private T InstantiatePrefab()
    {
        GameObject gameObject = UnityEngine.Object.Instantiate(_prefab.gameObject, _parent);
        T component = gameObject.GetComponent<T>();
        
        if (component == null)
            throw new System.InvalidOperationException($"Префаб должен содержать компонент типа {typeof(T)}");
        
        return component;
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