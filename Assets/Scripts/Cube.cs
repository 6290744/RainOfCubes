using UnityEngine;
using System;
using System.Collections;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]

public class Cube : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;
    [SerializeField] private int _minimalCubeLifetime = 2;
    [SerializeField] private int _maximalCubeLifeTime = 5;
    
    private Color _defaultColor;
    private bool _isPlatformTouched;
    
    public event Action<Cube> PlatformTouched;
    public event Action<Cube> LifetimeEnded;

    public void SetDefaultState()
    {
        _isPlatformTouched = false;
        _renderer.material.color = _defaultColor;
    }
    
    private void Awake()
    {
        _isPlatformTouched = false;
        _defaultColor = _renderer.material.color;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (_isPlatformTouched == false && collision.gameObject.TryGetComponent<Platform>(out _))
        {
            _isPlatformTouched = true;
            
            StartCoroutine(LifetimeStopwatch());
            
            PlatformTouched?.Invoke(this);
        }
    }
    
    private IEnumerator LifetimeStopwatch()
    {
        int time = Random.Range(_minimalCubeLifetime, _maximalCubeLifeTime);
        
        yield return new WaitForSeconds(time);
        
        LifetimeEnded?.Invoke(this);
    }
}
