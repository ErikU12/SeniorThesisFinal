using UnityEngine;

public class TxtBxPrompt : MonoBehaviour
{
    public GameObject spritePrefab; // The sprite prefab to spawn
    public string playerTag = "Player"; // The tag of the player
    public float spawnDistance = 5f; // The distance at which the sprite will spawn
    public Transform spawnLocation; // The location where the sprite will spawn

    private GameObject spawnedSprite; // Reference to the spawned sprite

    void Update()
    {
        // Check if the player is within the specified distance
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);
        if (player != null && Vector3.Distance(player.transform.position, transform.position) <= spawnDistance)
        {
            // Spawn the sprite if it hasn't been spawned already
            if (spawnedSprite == null)
            {
                SpawnSprite();
            }
        }
        else
        {
            // Destroy the sprite if the player is not within the specified distance
            if (spawnedSprite != null)
            {
                Destroy(spawnedSprite);
            }
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
}