using System;
using System.Collections;
using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Collider _collider;
    [SerializeField] private ColorChanger _colorChanger;

    public event Action<Cube> OnExpired;

    private bool _isActivated;
    private Coroutine _lifeTimerCoroutine;
    private float _minLifeTime = 2f;
    private float _maxLifeTime = 7f;

    private void OnCollisionEnter(Collision collision)
    {
        if (!_isActivated && collision.gameObject.TryGetComponent<Platform>(out _))
        {
            Activate();
            _colorChanger.HandleFirstCollision();
        }
    }

    public void PrepareForSpawn(float speed)
    {
        _isActivated = false;

        if (_rigidbody != null)
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
            _rigidbody.rotation = Quaternion.identity;
            _rigidbody.ResetInertiaTensor();
        }

        transform.rotation = Quaternion.identity;
        _colorChanger?.ResetColor();
        _rigidbody.velocity = Vector3.down * speed;

        if (_lifeTimerCoroutine != null)
        {
            StopCoroutine(_lifeTimerCoroutine);
            _lifeTimerCoroutine = null;
        }
    }

    public void Activate()
    {
        _isActivated = true;
        float lifetime = UnityEngine.Random.Range(_minLifeTime, _maxLifeTime);
        _lifeTimerCoroutine = StartCoroutine(LifetimeRoutine(lifetime));
        Debug.LogError($"время пошло осталось {lifetime} секунд.");
    }

    private IEnumerator LifetimeRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        OnExpired?.Invoke(this);
        Deactivate();
    }

    private void Deactivate()
    {
        _isActivated = false;
        _rigidbody.velocity = Vector3.zero;
    }

    private void OnDestroy() => OnExpired = null;
}
