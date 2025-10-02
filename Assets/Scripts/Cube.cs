using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Cube : MonoBehaviour
{
    private Rigidbody _rb;
    private Collider _collider;
    private bool _isActivated;
    private float _activationTime;
    private float _lifeTime;

    public event System.Action<Cube> OnLifeTimeExpired;
    public event System.Action OnFirstCollision;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        _lifeTime = Random.Range(5f, 7f);
        Deactivate();
    }

    public void PrepareForSpawn(Vector3 position, Vector3 velocity)
    {
        GetComponent<ColorChanger>()?.ResetColor();

        var colorChanger = GetComponent<ColorChanger>();
        if (colorChanger != null)
        {
            colorChanger.ResetColor();
        }

        transform.position = position;
        _rb.velocity = velocity;
        _collider.enabled = true;
        gameObject.SetActive(true);
        _isActivated = false;
    }

    private void Activate()
    {
        _isActivated = true;
        _activationTime = Time.time;
    }

    public void Deactivate()
    {
        _rb.velocity = Vector3.zero;
        _collider.enabled = false;
        gameObject.SetActive(false);
        _isActivated = false;
    }

    private void Update()
    {
        if (_isActivated && Time.time - _activationTime >= _lifeTime)
        {
            Deactivate();
            OnLifeTimeExpired?.Invoke(this);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!_isActivated && !collision.collider.isTrigger)
        {
            Activate();
            OnFirstCollision?.Invoke();
        }
    }
}