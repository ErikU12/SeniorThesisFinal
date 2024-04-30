using UnityEngine;

public class FloorSpawner : MonoBehaviour
{
    public GameObject floorPrefab;
    public Transform spawnPoint;
    public float despawnDistance = 10f; // Distance at which the floor despawns

    private GameObject spawnedFloor; // Reference to the spawned floor object
    private GameObject player; // Reference to the player object

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Spawn the floor at the specified spawn point
            spawnedFloor = Instantiate(floorPrefab, spawnPoint.position, Quaternion.identity);
        }
    }

    void Update()
    {
        // Check if the spawned floor exists and the player exists
        if (spawnedFloor != null && player != null)
        {
            // Calculate the distance between the player and the spawned floor
            float distance = Vector2.Distance(spawnedFloor.transform.position, player.transform.position);

            // If the distance exceeds the despawn distance, despawn the floor
            if (distance > despawnDistance)
            {
                DespawnFloor();
            }
        }
    }

    void DespawnFloor()
    {
        // Destroy the spawned floor
        Destroy(spawnedFloor);
    }
}