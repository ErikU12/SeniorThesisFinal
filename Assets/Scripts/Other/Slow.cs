using UnityEngine;

public class Slow : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public float spawnRadius = 2f;
    public float despawnTime = 5f; // Set the despawn time in the editor
    public LayerMask playerLayer;
    public Transform spawnPoint;
    public float moveSpeed = 5f; // Speed at which the prefab moves
    public float throwForce = 10f; // Force with which the prefab is thrown

    private GameObject spawnedPrefab;
    private bool isPrefabSpawned = false; // Track if a prefab is currently spawned
    private bool isThrown = false; // Track if the prefab has been thrown

    private void Update()
    {
        UpdatePrefabPosition();

        // Check if the X button is pressed and no prefab is spawned
        if (Input.GetKeyDown(KeyCode.X) && !isPrefabSpawned)
        {
            SpawnPrefab();
            Invoke("DespawnPrefab", despawnTime);
        }

        // Check if the X button is pressed and a prefab is spawned
        else if (Input.GetKeyDown(KeyCode.X) && isPrefabSpawned && !isThrown)
        {
            ThrowPrefab();
        }
    }

    private void UpdatePrefabPosition()
    {
        if (isPrefabSpawned && spawnedPrefab != null && !isThrown)
        {
            spawnedPrefab.transform.position = transform.position;
        }
    }

    private void SpawnPrefab()
    {
        // Calculate a random position around the player within the specified radius
        Vector2 randomPosition = Random.insideUnitCircle.normalized * spawnRadius;

        // Use the specified spawnPoint or default to the player's position
        Vector3 spawnPosition = (spawnPoint != null)
            ? spawnPoint.position
            : transform.position + new Vector3(randomPosition.x, randomPosition.y, 0f);

        // Spawn the prefab at the calculated position
        spawnedPrefab = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);

        // Set the sorting order of the spawned prefab to be higher than the player's sorting order
        Renderer playerRenderer = GetComponent<Renderer>();
        Renderer prefabRenderer = spawnedPrefab.GetComponent<Renderer>();

        if (playerRenderer != null && prefabRenderer != null)
        {
            prefabRenderer.sortingOrder = playerRenderer.sortingOrder + 1;
        }

        // Ignore collisions with the player's box collider for the specified duration
        Physics2D.IgnoreCollision(spawnedPrefab.GetComponent<Collider2D>(), GetComponent<Collider2D>(), true);

        isPrefabSpawned = true; // Set the flag to indicate a prefab is spawned
    }

    private void DespawnPrefab()
    {
        if (spawnedPrefab != null)
        {
            Destroy(spawnedPrefab);
            isPrefabSpawned = false; // Reset the flag when the prefab is despawned
            isThrown = false; // Reset the thrown state
        }
    }

    private void ThrowPrefab()
    {
        if (spawnedPrefab != null)
        {
            // Apply force to throw the prefab in a specified direction
            Rigidbody2D rb = spawnedPrefab.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = transform.right * throwForce; // Adjust direction if needed
            }
            isThrown = true; // Set the thrown state
        }
    }
}
