using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Boss : MonoBehaviour
{
    public Transform[] teleportPoints;
    public Transform[] firePoints;
    public GameObject fireballPrefab;
    public AudioClip fireballSound; // Sound effect for the fireball
    public float teleportInterval = 5f;
    public float fireballDelay = 1f;
    public float detectionRange = 10f; // Range within which the player is detected
    public float normalMovementSpeed = 1f; // Normal movement speed of the boss
    public float fastMovementSpeed = 2f; // Faster movement speed of the boss when low health

    public Animator teleportAnimator; // Reference to the teleport animation animator

    private List<Transform> shuffledTeleportPoints = new List<Transform>();
    private int currentIndex = 0;
    private int teleportCount = 0;
    private float lastTeleportTime;
    private Transform playerTransform; // Reference to the player's transform
    private BossHealth bossHealth; // Reference to the BossHealth component

    private bool isLowHealth = false; // Flag to indicate low health mode
    private bool isTeleporting = false; // Flag to indicate if boss is currently teleporting
    private static readonly int RedEyeTeleport = Animator.StringToHash("RedEyeTeleport");

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

        // Manage teleport animation
        if (isTeleporting)
        {
            // Play teleport animation
            if (teleportAnimator != null)
            {
                teleportAnimator.SetTrigger(RedEyeTeleport);
            }
        }
        else
        {
            // Turn off teleport animation
            if (teleportAnimator != null)
            {
                teleportAnimator.ResetTrigger(RedEyeTeleport);
            }
        }
    }

    void TeleportToNextPoint()
    {
        // Set teleporting flag to true
        isTeleporting = true;

        // Teleport to the next point in the shuffled list after animation
        StartCoroutine(TeleportAfterAnimation());
    }

    IEnumerator TeleportAfterAnimation()
    {
        // Wait for teleport animation to finish
        yield return new WaitForSeconds(teleportAnimator.GetCurrentAnimatorStateInfo(0).length);

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

        // Set teleporting flag to false after teleporting
        isTeleporting = false;
    }

    void SpawnFireballs()
    {
        // Play sound effect for the fireball
        if (fireballSound != null)
        {
            AudioSource.PlayClipAtPoint(fireballSound, transform.position);
        }

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



