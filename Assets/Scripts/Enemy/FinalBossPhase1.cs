using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FinalBossPhase1 : MonoBehaviour
{
    [System.Serializable]
    public struct WarpFirePointPair
    {
        public Transform warpPoint;
        public Transform[] firePoints; // Array of fire points associated with the warp point
    }

    public WarpFirePointPair[] warpFirePointPairs;
    public GameObject fireballPrefab;
    public AudioClip fireballSound; // Sound effect for the fireball
    public float teleportInterval = 5f;
    public float fireballDelay = 1f;
    public float detectionRange = 10f; // Range within which the player is detected
    public float normalMovementSpeed = 1f; // Normal movement speed of the boss
    public float fastMovementSpeed = 2f; // Faster movement speed of the boss when low health

    private List<WarpFirePointPair> shuffledWarpFirePointPairs = new List<WarpFirePointPair>();
    private int currentIndex = 0;
    private int teleportCount = 0;
    private float lastTeleportTime;
    private Transform playerTransform; // Reference to the player's transform
    private BossHealth bossHealth; // Reference to the BossHealth component

    private bool isLowHealth = false; // Flag to indicate low health mode
    private bool isTeleporting = false; // Flag to indicate if boss is currently teleporting

    void Start()
    {
        // Initialize last teleport time
        lastTeleportTime = Time.time;

        // Shuffle the warp-fire point pairs initially
        ShuffleWarpFirePointPairs();

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
        if (!isTeleporting && bossHealth.currentHealth > 0 && playerTransform != null && Vector3.Distance(transform.position, playerTransform.position) <= detectionRange &&
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
        // Set teleporting flag to true
        isTeleporting = true;

        // Start the coroutine
        StartCoroutine(TeleportAfterAnimation());
    }

    IEnumerator TeleportAfterAnimation()
    {
        // Wait for a brief moment before teleporting
        yield return new WaitForSeconds(1.0f); // Adjust delay as needed

        // Teleport to the next point in the shuffled list
        Transform nextWarpPoint = shuffledWarpFirePointPairs[currentIndex].warpPoint;
        transform.position = nextWarpPoint.position;

        // Spawn fireballs from the associated fire points
        Transform[] firePoints = shuffledWarpFirePointPairs[currentIndex].firePoints;
        foreach (Transform firePoint in firePoints)
        {
            SpawnFireballs(firePoint);
        }

        // Increment index for the next teleportation
        currentIndex = (currentIndex + 1) % shuffledWarpFirePointPairs.Count;

        // Increment teleport count
        teleportCount++;

        // Check if three teleports have occurred
        if (teleportCount >= 3)
        {
            // Reset teleport count
            teleportCount = 0;

            // Shuffle the warp-fire point pairs
            ShuffleWarpFirePointPairs();
        }

        // Set teleporting flag to false after teleporting
        isTeleporting = false;
    }

    void SpawnFireballs(Transform firePoint)
    {
        // Play sound effect for the fireball
        if (fireballSound != null)
        {
            AudioSource.PlayClipAtPoint(fireballSound, transform.position);
        }

        // Spawn fireballs from the specified fire point
        GameObject fireball = Instantiate(fireballPrefab, firePoint.position, Quaternion.identity);
        if (isLowHealth)
        {
            // Double the fireball's speed if in low health mode
            fireball.GetComponent<Rigidbody2D>().velocity *= 2f;
        }
    }

    // Helper method to shuffle the warp-fire point pairs
    void ShuffleWarpFirePointPairs()
    {
        shuffledWarpFirePointPairs.Clear();
        shuffledWarpFirePointPairs.AddRange(warpFirePointPairs);
        ShuffleList(shuffledWarpFirePointPairs);
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


