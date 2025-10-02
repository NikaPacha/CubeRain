using UnityEngine;
using System.Collections.Generic;

public class PlatformSpawner : MonoBehaviour
{
    [SerializeField] private Cube _cubePrefab;
    [SerializeField] private Transform _platform;
    [SerializeField] private int _poolSize = 20;
    [SerializeField] private float _spawnRate = 1f;
    [SerializeField] private float _cubeSpeed = 10f;
    [SerializeField] private Vector2 _heightRange = new Vector2(5f, 10f);

    private Queue<Cube> _pool = new Queue<Cube>();
    private BoxCollider _platformCollider;
    private float _nextSpawnTime;

    private void Awake()
    {
        _platformCollider = _platform.GetComponent<BoxCollider>();
        InitializePool();
    }

    private void InitializePool()
    {
        for (int i = 0; i < _poolSize; i++)
        {
            Cube cube = Instantiate(_cubePrefab, transform);
            cube.OnLifeTimeExpired += ReturnCube;
            cube.Deactivate();
            _pool.Enqueue(cube);
        }
    }

    private void Update()
    {
        if (Time.time >= _nextSpawnTime && _pool.Count > 0)
        {
            SpawnCube();
            _nextSpawnTime = Time.time + _spawnRate;
        }
    }

    private void SpawnCube()
    {
        Cube cube = _pool.Dequeue();
        Vector3 spawnPos = CalculateSpawnPosition();
        cube.PrepareForSpawn(spawnPos, Vector3.down * _cubeSpeed);
    }

    private Vector3 CalculateSpawnPosition()
    {
        return new Vector3(
            Random.Range(_platform.position.x - _platformCollider.size.x / 2,
                        _platform.position.x + _platformCollider.size.x / 2),
            Random.Range(_heightRange.x, _heightRange.y),
            Random.Range(_platform.position.z - _platformCollider.size.z / 2,
                        _platform.position.z + _platformCollider.size.z / 2)
        );
    }

    private void ReturnCube(Cube cube)
    {
        cube.Deactivate();
        _pool.Enqueue(cube);
    }
}