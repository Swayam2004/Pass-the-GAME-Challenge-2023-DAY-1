using UnityEngine;

public class JunkManager : MonoBehaviour
{
    [SerializeField] private GameObject junkPrefab;
    [SerializeField] private int numberOfJunkToSpawn = 10;
    [SerializeField] private float spawnRadius = 5f;

    private Transform playerTransform;

    private void Start()
    {
        playerTransform = PlayerController.Instance.transform;

        // Spawn junk objects around the player
        for (int i = 0; i < numberOfJunkToSpawn; i++)
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
        Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPosition = playerTransform.position + new Vector3(randomOffset.x, randomOffset.y, 0f);

        Instantiate(junkPrefab, spawnPosition, Quaternion.identity);
    }

    private void DestroyJunkOutsideRadius()
    {
        Junk[] junkObjects = FindObjectsByType<Junk>(FindObjectsSortMode.None);

        foreach (Junk junk in junkObjects)
        {
            float distanceToPlayer = Vector3.Distance(junk.transform.position, playerTransform.position);

            if (distanceToPlayer > spawnRadius)
            {
                Destroy(junk.gameObject);
            }
        }

        if (junkObjects.Length < numberOfJunkToSpawn / 2)
        {
            SpawnJunkAroundPlayer();
        }
    }
}
