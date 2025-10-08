using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private Cube _cubePrefab;
    [SerializeField] private Transform _platform;
    [SerializeField] private int _poolSize = 20;
    [SerializeField] private float _spawnRate = 2f;
    [SerializeField] private float _cubeSpeed = 5f;
    [SerializeField] private Vector2 _heightRange = new Vector2(1f, 3f);

    private ObjectPool<Cube> _pool;
    private BoxCollider _platformCollider;
    private float _nextSpawnTime;
    private Cube _currentCube;

    private void Awake()
    {
        if (!_platform.TryGetComponent(out BoxCollider platformCollider))
        {
            Debug.LogError("Платформа не имеет BoxCollider!");
            enabled = false;
            return;
        }
        _platformCollider = platformCollider;

        _pool = new ObjectPool<Cube>(_cubePrefab, _poolSize, _platform);
    }

    private void Update()
    {
        if (Time.time >= _nextSpawnTime)
        {
            SpawnCube();
            _nextSpawnTime = Time.time + 1f / _spawnRate;
        }
    }

    private void SpawnCube()
    {
        Cube cube = _pool.GetObject();
        if (cube == null) return;

        cube.OnExpired += OnCubeDestroyed;

        Vector3 spawnPosition = CalculateSpawnPosition();
        cube.transform.SetPositionAndRotation(spawnPosition, Quaternion.identity);
        cube.PrepareForSpawn(_cubeSpeed);
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

    private void OnCubeDestroyed(Cube cube)
    {
        if (cube != null)
        {
            cube.OnExpired -= OnCubeDestroyed;
            _pool.ReturnObject(cube);
        }
    }
}