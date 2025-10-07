using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Collider _collider;
    [SerializeField] private ColorChanger _colorChanger;

    public static event System.Action<Cube> CubeDestroyed;

    private bool _isActivated;
    private float _activationTime;
    private float _lifeTime;
    private float _minLifeTime = 5f;
    private float _maxLifeTime = 7f;

    private void Awake()
    {
        InitializeComponents();
    }

    private void Update()
    {
        if (_isActivated && Time.time - _activationTime > _lifeTime)
        {
            Deactivate();
            CubeDestroyed?.Invoke(this);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Activate();
        _colorChanger?.HandleFirstCollision();
    }

    private void OnDestroy()
    {
        CubeDestroyed = null;
    }

    public void PrepareForSpawn(float speed)
    {
        _isActivated = false;
        _rb.velocity = Vector3.down * speed;
        _colorChanger?.ResetColor();
        _lifeTime = UnityEngine.Random.Range(_minLifeTime, _maxLifeTime);
        _activationTime = Time.time;
    }

    public void Activate()
    {
        if (_isActivated) return;
        _isActivated = true;
        _activationTime = Time.time;
    }

    public void Deactivate()
    {
        if (!_isActivated) return;
        _isActivated = false;
        _rb.velocity = Vector3.zero;
    }

    private void InitializeComponents()
    {
        _rb = _rb != null ? _rb : GetComponent<Rigidbody>();
        _collider = _collider != null ? _collider : GetComponent<Collider>();
        _colorChanger = _colorChanger != null ? _colorChanger : GetComponent<ColorChanger>();
    }
}