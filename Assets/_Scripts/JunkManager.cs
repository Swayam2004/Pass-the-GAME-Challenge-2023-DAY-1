using UnityEngine;

public class JunkManager : MonoBehaviour
{
    [SerializeField] private GameObject _junkPrefab;
    [SerializeField] private int _numberOfJunkToSpawn = 10;
    [SerializeField] private float _spawnRadius = 5f;

    private Transform _playerTransform;

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
        Vector2 randomOffset = Random.insideUnitCircle * _spawnRadius;
        Vector3 spawnPosition = _playerTransform.position + new Vector3(randomOffset.x, randomOffset.y, 0f);

        Instantiate(_junkPrefab, spawnPosition, Quaternion.identity);
    }

    private void DestroyJunkOutsideRadius()
    {
        Junk[] junkObjects = FindObjectsByType<Junk>(FindObjectsSortMode.None);

        foreach (Junk junk in junkObjects)
        {
            float distanceToPlayer = Vector3.Distance(junk.transform.position, _playerTransform.position);

            if (distanceToPlayer > _spawnRadius)
            {
                Destroy(junk.gameObject);
            }
        }

        if (junkObjects.Length < _numberOfJunkToSpawn / 2)
        {
            SpawnJunkAroundPlayer();
        }
    }
}
