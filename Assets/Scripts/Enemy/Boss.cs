using UnityEngine;
using System.Collections.Generic;

public class Boss : MonoBehaviour
{
    public Transform[] teleportPoints;
    public Transform[] firePoints;
    public GameObject fireballPrefab;
    public float teleportInterval = 5f;
    public float fireballDelay = 1f;
    public float detectionRange = 10f; // Range within which the player is detected
    public float normalMovementSpeed = 1f; // Normal movement speed of the boss
    public float fastMovementSpeed = 2f; // Faster movement speed of the boss when low health

    private List<Transform> shuffledTeleportPoints = new List<Transform>();
    private int currentIndex = 0;
    private int teleportCount = 0;
    private float lastTeleportTime;
    private Transform playerTransform; // Reference to the player's transform
    private BossHealth bossHealth; // Reference to the BossHealth component

    private bool isLowHealth = false; // Flag to indicate low health mode

    void Start()
    {
        // Initialize last teleport time
        lastTeleportTime = Time.time;

        // Shuffle the teleport points initially
        ShuffleTeleportPoints();

        // Find the player's transform
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        // Get the BossHealth component
        bossHealth = GetComponent<BossHealth>();

        // Start teleporting if player is in range and boss health is not zero
        if (playerTransform != null && Vector3.Distance(transform.position, playerTransform.position) <= detectionRange && bossHealth.currentHealth > 0)
        {
            TeleportToNextPoint();
        }
    }

    void Update()
    {
        // Check if player is in range, boss health is not zero, and it's time to teleport again
        if (bossHealth.currentHealth > 0 && playerTransform != null && Vector3.Distance(transform.position, playerTransform.position) <= detectionRange &&
            Time.time - lastTeleportTime >= (isLowHealth ? teleportInterval / 2 : teleportInterval))
        {
            // Reset last teleport time
            lastTeleportTime = Time.time;

            // Teleport to the next point
            TeleportToNextPoint();
        }

        // Check for low health
        if (bossHealth.currentHealth <= 5 && !isLowHealth)
        {
            // Activate low health mode
            isLowHealth = true;
            // Increase movement speed when low health
            GetComponent<Rigidbody2D>().velocity *= fastMovementSpeed / normalMovementSpeed;
        }
    }

    void TeleportToNextPoint()
    {
        // Teleport to the next point in the shuffled list
        transform.position = shuffledTeleportPoints[currentIndex].position;

        // Increment index for the next teleportation
        currentIndex = (currentIndex + 1) % shuffledTeleportPoints.Count;

        // Increment teleport count
        teleportCount++;

        // Check if three teleports have occurred
        if (teleportCount >= 3)
        {
            // Reset teleport count
            teleportCount = 0;

            // Shuffle the teleport points
            ShuffleTeleportPoints();

            // Invoke method to spawn fireballs after a delay
            Invoke("SpawnFireballs", fireballDelay);
        }
    }

    void SpawnFireballs()
    {
        // Spawn fireballs from each fire point
        foreach (Transform firePoint in firePoints)
        {
            GameObject fireball = Instantiate(fireballPrefab, firePoint.position, Quaternion.identity);
            if (isLowHealth)
            {
                // Double the fireball's speed if in low health mode
                fireball.GetComponent<Rigidbody2D>().velocity *= 2f;
            }
        }
    }

    // Helper method to shuffle the teleport points
    void ShuffleTeleportPoints()
    {
        shuffledTeleportPoints.Clear();
        shuffledTeleportPoints.AddRange(teleportPoints);
        ShuffleList(shuffledTeleportPoints);
    }

    // Helper method to shuffle a list
    void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    // Method to activate low health mode
    public void ActivateLowHealthMode()
    {
        isLowHealth = true;
    }

    // Method to deactivate low health mode
    public void DeactivateLowHealthMode()
    {
        isLowHealth = false;
    }
}
