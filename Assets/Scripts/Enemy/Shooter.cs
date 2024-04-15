using UnityEngine;

public class Shooter : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 5.0f;
    public float detectionRange = 5.0f; // Range within which the enemy detects the player.
    public float shootInterval = 2.0f;  // Time between shots

    private Transform playerTransform;
    private float lastShotTime;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        lastShotTime = Time.time;
    }

    private void Update()
    {
        if (playerTransform == null)
        {
            return; // Player not found
        }

        // Calculate the distance between the enemy and the player.
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        // Check if the player is within detection range.
        if (distanceToPlayer <= detectionRange)
        {
            // Check if it's time to shoot again.
            if (Time.time - lastShotTime >= shootInterval)
            {
                Shoot();
                lastShotTime = Time.time;
            }
        }
    }

    private void Shoot()
    {
        if (playerTransform == null)
        {
            return; // Player not found
        }

        Vector3 direction = (playerTransform.position - transform.position).normalized;

        // Create a new projectile
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        // Set the projectile's velocity
        rb.velocity = direction * projectileSpeed;
    }
}