using UnityEngine;

[RequireComponent(typeof(Cube), typeof(Renderer))]
public class ColorChanger : MonoBehaviour
{
    [SerializeField] private Color _defaultColor = Color.white;

    private Material _material;
    private Cube _cube;
    private bool _colorChanged;

    private void Awake()
    {
        _cube = GetComponent<Cube>();
        if (_cube == null)
        {
            Debug.LogError("Cube component missing!", this);
            return;
        }

        Renderer renderer = GetComponent<Renderer>();
        if (renderer == null)
        {
            Debug.LogError("Renderer component missing!", this);
            return;
        }

        _material = renderer.material;
        ResetColor();

        _cube.OnFirstCollision += HandleFirstCollision;
    }

    private void HandleFirstCollision()
    {
        if (!_colorChanged)
        {
            _material.color = new Color(
                Random.value,
                Random.value,
                Random.value
            );
            _colorChanged = true;
        }
    }

    public void ResetColor()
    {
        if (_material != null)
        {
            _material.color = _defaultColor;
            _colorChanged = false;
        }
    }

    private void OnDestroy()
    {
        if (_cube != null)
        {
            _cube.OnFirstCollision -= HandleFirstCollision;
        }
    }
}
