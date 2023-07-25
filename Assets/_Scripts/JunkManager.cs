using UnityEngine;
using UnityEngine.Pool;

public class JunkManager : MonoBehaviour
{
    public static JunkManager Instance { get; private set; }

    [SerializeField] private GameObject _junkPrefab;
    [SerializeField] private int _numberOfJunkToSpawn = 10;
    [SerializeField] private float _spawnRadius = 5f;

    private Transform _playerTransform;
    private ObjectPool<Junk> _junkPool;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There are more than one PlayerController");
            Destroy(gameObject);
            return;
        }
        Instance = this;

        _junkPool = new ObjectPool<Junk>(() =>
        {
            Vector2 randomOffset = Random.insideUnitCircle * _spawnRadius;
            Vector3 spawnPosition = _playerTransform.position + new Vector3(randomOffset.x, randomOffset.y, 0f);

            return Instantiate(_junkPrefab, spawnPosition, Quaternion.identity).GetComponent<Junk>();
        }, junk =>
        {
            Vector2 randomOffset = Random.insideUnitCircle * _spawnRadius;
            Vector3 spawnPosition = _playerTransform.position + new Vector3(randomOffset.x, randomOffset.y, 0f);

            junk.gameObject.SetActive(true);
            junk.transform.position = spawnPosition;
        }, junk =>
        {
            junk.gameObject.SetActive(false);
        }, junk =>
        {
            Destroy(junk.gameObject);
        }, false, 60, 100);
    }

    private void Start()
    {
        _playerTransform = PlayerController.Instance.transform;

        // Spawn junk objects around the player
        for (int i = 0; i < _numberOfJunkToSpawn; i++)
        {
            SpawnJunkAroundPlayer();
        }
    }

    private void Update()
    {
        // Check and destroy junk objects outside the spawn radius
        DestroyJunkOutsideRadius();
    }

    private void SpawnJunkAroundPlayer()
    {
        Junk junk = _junkPool.Get();
    }

    private void DestroyJunkOutsideRadius()
    {
        Junk[] junkObjects = FindObjectsByType<Junk>(FindObjectsSortMode.None);

        foreach (Junk junk in junkObjects)
        {
            float distanceToPlayer = Vector3.Distance(junk.transform.position, _playerTransform.position);

            if (distanceToPlayer > _spawnRadius)
            {
                _junkPool.Release(junk);
            }
        }

        if (junkObjects.Length < _numberOfJunkToSpawn / 2)
        {
            SpawnJunkAroundPlayer();
        }
    }

    public void DestroyJunk(Junk junk)
    {
        _junkPool.Release(junk);
    }
}
