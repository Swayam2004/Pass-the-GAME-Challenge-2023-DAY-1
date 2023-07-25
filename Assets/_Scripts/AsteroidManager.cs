using UnityEngine;

public class AsteroidManager : MonoBehaviour
{
    [SerializeField] private GameObject _asteroidPrefab;
    [SerializeField] private int _numberOfAsteroidToSpawn = 10;
    [SerializeField] private float _spawnRadius = 5f;

    private Transform _playerTransform;

    private void Start()
    {
        _playerTransform = PlayerController.Instance.transform;

        // Spawn junk objects around the player
        for (int i = 0; i < _numberOfAsteroidToSpawn; i++)
        {
            SpawnAsteroidAroundPlayer();
        }
    }

    private void Update()
    {
        // Check and destroy junk objects outside the spawn radius
        DestroyAsteroidOutsideRadius();
    }

    private void SpawnAsteroidAroundPlayer()
    {
        Vector2 randomOffset = Random.insideUnitCircle * _spawnRadius;
        Vector3 spawnPosition = _playerTransform.position + new Vector3(randomOffset.x, randomOffset.y, 0f);

        Instantiate(_asteroidPrefab, spawnPosition, Quaternion.identity);
    }

    private void DestroyAsteroidOutsideRadius()
    {
        Asteroid[] asteroidObjects = FindObjectsByType<Asteroid>(FindObjectsSortMode.None);

        foreach (Asteroid asteroid in asteroidObjects)
        {
            float distanceToPlayer = Vector3.Distance(asteroid.transform.position, _playerTransform.position);

            if (distanceToPlayer > _spawnRadius)
            {
                Destroy(asteroid.gameObject);
            }
        }

        if (asteroidObjects.Length < _numberOfAsteroidToSpawn / 2)
        {
            SpawnAsteroidAroundPlayer();
        }
    }
}
