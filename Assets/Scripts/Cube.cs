using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]

public class Cube : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;
    
    private Color _defaultColor;
    private bool _isTouched;
    private string _aimTag = "Platform";
    
    public event Action<Cube> OnTouched;

    public void SetDefaultState()
    {
        _isTouched = false;
        _renderer.material.color = _defaultColor;
    }
    
    private void Awake()
    {
        _defaultColor = _renderer.material.color;
        _isTouched = false;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(_aimTag) && _isTouched == false)
        {
            _isTouched = true;
            
            OnTouched?.Invoke(this);
        }
    }
}
