using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [SerializeField] private Transform _platform;
    [SerializeField] private Cube _cubePrefab;
    [SerializeField] private int _poolSize = 20;
    [SerializeField] private float _minHeight = 42f;
    [SerializeField] private float _maxHeight = 48f;
    [SerializeField] private float _spawnRate = 1f;
    [SerializeField] private float _cubeSpeed = 15f;
    private float _half = 0.5f;

    private ObjectPool<Cube> _objectPool;
    private BoxCollider _platformCollider;
    private Vector3 _platformSize;
    private Vector3 _platformPosition;
    private float _nextSpawnTime;

    private void Awake()
    {
        _platformCollider = _platform.GetComponent<BoxCollider>();
        _platformPosition = _platform.position;
        _platformSize = _platformCollider.size;
        _objectPool = new ObjectPool<Cube>(_cubePrefab.gameObject, _poolSize, transform);

        foreach (GameObject cube in _objectPool.GetAllObjects())
        {
            cube.GetComponent<Cube>().SetPool(_objectPool);
        }
    }

    private void Update()
    {
        if (Time.time >= _nextSpawnTime)
        {
            SpawnCube();
            _nextSpawnTime = Time.time + _spawnRate;
        }
    }

    private void SpawnCube()
    {
        GameObject cube = _objectPool.GetObject();       

        float randomX = Random.Range(
            _platformPosition.x - _platformSize.x * _half,
            _platformPosition.x + _platformSize.x * _half
        );

        float randomZ = Random.Range(
            _platformPosition.z,
            _platformPosition.z
        );

        float randomY = Random.Range(_minHeight, _maxHeight);

        Vector3 spawnPosition = new Vector3(randomX, randomY, randomZ);

        cube.transform.position = spawnPosition;
        cube.GetComponent<Rigidbody>().velocity = Vector3.down * _cubeSpeed;
    }
}
