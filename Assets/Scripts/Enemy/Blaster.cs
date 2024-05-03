using UnityEngine;

public class BlasterEnemy : MonoBehaviour
{
    public float detectionRange = 5f; // Range within which the enemy detects the player
    public float chargingSpeed = 6f; // Speed at which the enemy charges towards the player
    public GameObject bulletPrefab; // Prefab for the bullet to shoot
    public Transform shootPoint; // Point from where the bullet is shot
    public float bulletLifetime = 1f; // Lifetime of the bullet
    public float shootingInterval = 1f; // Time interval between shots
    public float stoppingDistance = 1f; // Distance from the player to stop moving
    public Animator animator; // Reference to the Animator component
    public float projectileSpeed = 10f; // Speed of the projectile
    public AudioClip shootSound; // Sound effect for shooting

    private static readonly int Shoot = Animator.StringToHash("Shoot");
    private static readonly int ShooterEnemy2Run = Animator.StringToHash("ShooterEnemy2Run");

    private Transform playerTransform; // Reference to the player's transform
    private bool isShooting = false; // Flag to track if the enemy is shooting
    private AudioSource audioSource; // Reference to the AudioSource component

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        if (playerTransform == null)
        {
            Debug.LogError("Player not found. Make sure the player has the 'Player' tag.");
        }

        // Add AudioSource component to the BlasterEnemy object if it doesn't exist
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        // Calculate the distance to the player
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        // Check if the player is within the detection range
        if (distanceToPlayer <= detectionRange)
        {
            // Determine the direction to the player
            Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;

            // Flip the sprite if necessary
            if (directionToPlayer.x > 0) // Player is on the right side
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else // Player is on the left side
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }

            // If the player is within the shooting range and not already shooting, shoot
            if (distanceToPlayer <= stoppingDistance && !isShooting)
            {
                isShooting = true;
                ShootAtPlayer();
            }
            else
            {
                // Move towards the player up to the stopping distance
                if (distanceToPlayer > stoppingDistance)
                {
                    // Move towards the player
                    transform.Translate(directionToPlayer * (chargingSpeed * Time.deltaTime));

                    // Trigger the running animation
                    if (animator != null)
                    {
                        animator.SetBool(ShooterEnemy2Run, true);
                    }
                }
                else
                {
                    // Stop running animation
                    if (animator != null)
                    {
                        animator.SetBool(ShooterEnemy2Run, false);
                    }
                }
            }
        }
    }

    void ShootAtPlayer()
    {
        // Trigger the shooting animation
        if (animator != null)
        {
            animator.SetTrigger(Shoot);
        }

        // Instantiate a bullet prefab at the shoot point
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);

        // Set the projectile's velocity towards the player
        Vector2 direction = (playerTransform.position - shootPoint.position).normalized;
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * projectileSpeed;
        }

        // Play shoot sound if assigned
        if (audioSource != null && shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }

        // Destroy the bullet after its lifetime
        Destroy(bullet, bulletLifetime);

        // Reset the shooting flag after the shooting interval
        Invoke("ResetShooting", shootingInterval);
    }

    void ResetShooting()
    {
        isShooting = false;
    }
}




