using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private Cube _cubePrefab;
    [SerializeField] private float _spawnInterval = 0.2f;
    [SerializeField] private float _spawnHeight = 20f;
    [SerializeField] private float _spawnRadius = 5;
    [SerializeField] private int _poolMaxSize = 30;
    [SerializeField] private int _poolCapacity = 30;
    
    private bool _isSpawning;
    
    private ObjectPool<Cube> _cubesPool;

    private void Awake()
    {
        _cubesPool = new ObjectPool<Cube>(
            createFunc: () => Instantiate(_cubePrefab),
            actionOnGet: (cube) => OnGetFromPool(cube),
            actionOnRelease: cube => OnReleaseToPool(cube),
            actionOnDestroy: cube => Destroy(cube.gameObject),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
    }

    private void Start()
    {
        _isSpawning = true;
        StartCoroutine(SpawnLoop());
    }
    
    private void OnGetFromPool(Cube cube)
    {
        cube.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        cube.transform.position = GetStartPosition();
        cube.gameObject.SetActive(true);
        
        cube.LifetimeEnded += _cubesPool.Release;
    }

    private void OnReleaseToPool(Cube cube)
    {
        cube.gameObject.SetActive(false);
        cube.SetDefaultState();
        
        cube.LifetimeEnded -= _cubesPool.Release;
    }

    private Vector3 GetStartPosition()
    {
        float coordinateX = Random.Range(-(_spawnRadius), _spawnRadius);
        float coordinateZ = Random.Range(-(_spawnRadius), _spawnRadius);
        
        return new Vector3(coordinateX, _spawnHeight, coordinateZ);
    }
    
    private IEnumerator SpawnLoop()
    {
        while (_isSpawning)
        {
            if (_cubesPool.CountActive < _poolMaxSize)
            {
                _cubesPool.Get();
            }
        
            yield return new WaitForSeconds(_spawnInterval);
        }
    }
}
