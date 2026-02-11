using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private Cube _cubePrefab;
    [SerializeField] private float _spawnInterval = 0.1f;
    [SerializeField] private float _spawnHeight = 20f;
    [SerializeField] private float _spawnRadius = 5;
    [SerializeField] private int _poolMaxSize = 30;
    [SerializeField] private int _poolCapacity = 30;
    [SerializeField] private int _minimalCubeLifetime = 2;
    [SerializeField] private int _maximalCubeLifeTime = 5;
    
    private ObjectPool<Cube> _cubesPool;

    private void Awake()
    {
        _cubesPool = new ObjectPool<Cube>(
            createFunc: () => Instantiate(_cubePrefab),
            actionOnGet: (cube) => ActionOnGet(cube),
            actionOnRelease: cube => ActionOnRelease(cube),
            actionOnDestroy: cube => Destroy(cube.gameObject),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
    }

    private void Start()
    {
        Debug.Log(_cubesPool.CountAll);
        InvokeRepeating(nameof(GetCubeFromPool), 0f, _spawnInterval);
    }

    private void GetCubeFromPool()
    {
        if (_cubesPool.CountActive < _poolMaxSize)
        {
            _cubesPool.Get();
        }
    }

    private void ActionOnGet(Cube cube)
    {
        cube.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        cube.transform.position = GetStartPosition();
        cube.gameObject.SetActive(true);
        
        cube.OnTouched += StartCubeLifetime;
    }

    private void ActionOnRelease(Cube cube)
    {
        cube.gameObject.SetActive(false);
        cube.SetDefaultState();
        
        cube.OnTouched -= StartCubeLifetime;
    }

    private Vector3 GetStartPosition()
    {
        float coordinateX = Random.Range(-(_spawnRadius), _spawnRadius);
        float coordinateZ = Random.Range(-(_spawnRadius), _spawnRadius);
        
        return new Vector3(coordinateX, _spawnHeight, coordinateZ);
    }

    private void StartCubeLifetime(Cube cube)
    {
        StartCoroutine(CubeLifetimeStopwatch(cube));
    }
    
    private IEnumerator CubeLifetimeStopwatch(Cube cube)
    {
        int time = Random.Range(_minimalCubeLifetime, _maximalCubeLifeTime);
        
        yield return new WaitForSeconds(time);

        _cubesPool.Release(cube);
    }
}
