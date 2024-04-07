using UnityEngine;

public class TxtBxPrompt2 : MonoBehaviour
{
    public GameObject spritePrefab; // The sprite prefab to spawn
    public float spawnDistance = 5f; // The distance at which the sprite will spawn
    public Transform spawnLocation; // The location where the sprite will spawn

    private GameObject spawnedSprite; // Reference to the spawned sprite

    void Update()
    {
        // Check if the "N" key is pressed
        if (Input.GetKeyDown(KeyCode.V))
        {
            // Check if the player is within the specified distance
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null && Vector3.Distance(player.transform.position, transform.position) <= spawnDistance)
            {
                // Spawn the sprite if it hasn't been spawned already
                if (spawnedSprite == null)
                {
                    SpawnSprite();
                }
            }
        }

        // Check if the player is not within the specified distance and a sprite has been spawned
        GameObject playerInRange = GameObject.FindGameObjectWithTag("Player");
        if (spawnedSprite != null && (playerInRange == null || Vector3.Distance(playerInRange.transform.position, transform.position) > spawnDistance))
        {
            DespawnSprite();
        }
    }

    void SpawnSprite()
    {
        // Spawn the sprite at the specified location
        if (spritePrefab != null && spawnLocation != null)
        {
            spawnedSprite = Instantiate(spritePrefab, spawnLocation.position, Quaternion.identity);
        }
    }

    void DespawnSprite()
    {
        // Destroy the spawned sprite
        if (spawnedSprite != null)
        {
            Destroy(spawnedSprite);
            spawnedSprite = null; // Reset spawnedSprite reference
        }
    }
}