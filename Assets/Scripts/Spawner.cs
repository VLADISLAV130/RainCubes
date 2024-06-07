using UnityEngine;
using UnityEngine.Pool;
using System.Collections;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Cube _cube;
    
    private float _repeatRate = 1f;
    private int _poolCapacity = 5;
    private int _poolMaxSize = 20;
    private float _hieght = 10f;
    private float _minPosition = -8f;
    private float _maxPosition = 8f;    
    private ObjectPool<Cube> _pool;

    private void Awake()
    {
        InitializePool();
    }

    private void Start()
    {
        StartCoroutine(SpawnCubeCoroutine());
    }

    public void ReleaseCube(Cube cube)
    {
        _pool.Release(cube);
    }

    private void InitializePool()
    {
        _pool = new ObjectPool<Cube>(
            CreateCubeInstance,
            actionOnGet: (cube) => PrepareCube(cube),
            actionOnRelease: (cube) => cube.gameObject.SetActive(false),
            actionOnDestroy: (cube) => Destroy(cube),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
    }

    private Cube CreateCubeInstance()
    {
        Cube cube = Instantiate(_cube);
        cube.OnReleased += ReleaseCube;
        return cube;
    }

    private void PrepareCube(Cube cube)
    {       
        cube.transform.position = GetRandomStartPosition();
        cube.SetInitialVelocity(Vector3.down);
        cube.gameObject.SetActive(true);
    }

    private Vector3 GetRandomStartPosition()
    {
        return new Vector3(Random.Range(_minPosition, _maxPosition), _hieght, Random.Range(_minPosition, _maxPosition));        
    }       

    private IEnumerator SpawnCubeCoroutine()
    {
        while (true)
        {
            _pool.Get();
            yield return new WaitForSeconds(_repeatRate);
        }
    }
}
