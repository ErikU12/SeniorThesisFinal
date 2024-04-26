using UnityEngine;

public class SniperEnemy : MonoBehaviour
{
    public float detectionRange = 10f; // Range within which the enemy detects the player
    public float shootingRange = 7f; // Maximum shooting range
    public GameObject projectilePrefab; // Prefab for the projectile to shoot
    public Transform shootPoint; // Point from where the projectile is shot
    public float shootingInterval = 2f; // Time interval between shots
    public float moveSpeed = 3f; // Movement speed of the enemy
    public float stoppingDistance = 5f; // Distance from the player to stop moving
    public float projectileSpeed = 10f; // Speed of the projectile
    public Animator animator; // Reference to the Animator component
    private static readonly int Shoot = Animator.StringToHash("Shoot");

    private Transform playerTransform; // Reference to the player's transform
    private bool isShooting = false; // Flag to track if the enemy is shooting
    private AudioSource audioSource; // Reference to the AudioSource component

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        audioSource = GetComponent<AudioSource>();

        if (playerTransform == null)
        {
            Debug.LogError("Player not found. Make sure the player has the 'Player' tag.");
        }
    }

    void Update()
    {
        // Calculate the distance to the player
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        // Determine the direction to the player
        Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;

        // Move the enemy away from the player if too close
        if (distanceToPlayer < stoppingDistance)
        {
            transform.Translate(-directionToPlayer * (moveSpeed * Time.deltaTime));
        }
        else
        {
            // Flip the sprite to face the player's direction
            if (directionToPlayer.x > 0)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z); // Face right
            }
            else
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z); // Face left
            }

            // If the player is within the detection range
            if (distanceToPlayer <= detectionRange)
            {
                // If the player is within the shooting range and not already shooting, shoot
                if (distanceToPlayer <= shootingRange && !isShooting)
                {
                    isShooting = true;
                    ShootAtPlayer(distanceToPlayer);
                }
                else
                {
                    // Move towards the player up to the stopping distance
                    if (distanceToPlayer > stoppingDistance)
                    {
                        transform.Translate(directionToPlayer * (moveSpeed * Time.deltaTime));
                    }
                }
            }
        }
    }

    void ShootAtPlayer(float distanceToPlayer)
    {
        // Trigger the shooting animation
        if (animator != null)
        {
            animator.SetTrigger(Shoot);
        }

        // Instantiate a projectile prefab at the shoot point
        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);

        // Set the projectile's velocity towards the player
        Vector2 direction = (playerTransform.position - shootPoint.position).normalized;
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * projectileSpeed;
        }

        // Play shoot sound
       // if (audioSource != null && projectile.GetComponent<Projectile>() != null && projectile.GetComponent<Projectile>().shootSound != null)
        {
           // audioSource.PlayOneShot(projectile.GetComponent<Projectile>().shootSound);
        }

        // Reset the shooting flag after the shooting interval
        Invoke("ResetShooting", shootingInterval);
    }

    void ResetShooting()
    {
        isShooting = false;
    }
}



