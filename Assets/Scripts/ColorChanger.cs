using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    private Renderer _renderer;
    private Color _originalColor;
    private bool _hasChangedColor;

    public void HandleFirstCollision()
    {
        if (_hasChangedColor) return;

        _renderer.material.color = Random.ColorHSV();
        _hasChangedColor = true;
    }

    public void ResetColor()
    {
        _renderer.material.color = _originalColor;
        _hasChangedColor = false;
    }

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        if (_renderer == null)
        {
            Debug.LogError("Renderer component not found!");
            enabled = false;
            return;
        }

        _originalColor = _renderer.material.color;
    }
}