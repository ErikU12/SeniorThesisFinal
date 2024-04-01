using UnityEngine;

public class FloorSpawner : MonoBehaviour
{
    public GameObject floorPrefab;
    public Transform spawnPoint;
    public GameObject objectToDestroy;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Spawn the floor at the specified spawn point
            Instantiate(floorPrefab, spawnPoint.position, Quaternion.identity);

            // Destroy the specified object
            if (objectToDestroy != null)
            {
                Destroy(objectToDestroy);
            }
        }
    }
}