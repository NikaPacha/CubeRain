using System;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(Rigidbody))]

public class Cube : MonoBehaviour
{
    [SerializeField] private float _minLifeTime = 2f;
    [SerializeField] private float _maxLifeTime = 7f;
    private Renderer _renderer;
    private Rigidbody _rigidbody;
    private bool _colorChanged = false;
    private float _lifeTime;
    private float _startTime;

    private ObjectPool<Cube> _pool;

    public event Action<Cube> OnCubeClicked;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!_colorChanged && collision.gameObject.CompareTag("Platform"))
        {
            ChangeColor();
            _lifeTime = Random.Range(_minLifeTime, _maxLifeTime);
            _startTime = Time.time;
        }
    }

    private void Update()
    {
        if (_colorChanged && Time.time - _startTime >= _lifeTime)
        {
            _pool.ReturnObject(gameObject);
        }
    }

    private void ChangeColor()
    {
        _renderer.material.color = new Color(
            Random.Range(0f, 1f),
            Random.Range(0f, 1f),
            Random.Range(0f, 1f)
        );
        _colorChanged = true;
    }

    public void SetPool(ObjectPool<Cube> pool)
    {
        _pool = pool;
    }
    public void SetColorChanged(bool value)
    {
        _colorChanged = value;
    }
    public void SetLifeTime(float value)
    {
        _lifeTime = value;
    }

    public void SetStartTime(float value)
    {
        _startTime = value;
    }
}
