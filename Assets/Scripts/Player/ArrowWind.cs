using UnityEngine;

public class ArrowWind : MonoBehaviour
{
    public int damage = 1; // Damage dealt by the bullet
    public float floatDuration = 2f; // Duration for which the enemy floats in the air
    public float floatForce = 10f; // Force applied to the enemy to make it float
    public AudioClip hitSound; // Sound effect for hitting the enemy
    private bool hasHitEnemy = false; // Flag to track if the arrow has hit an enemy
    private AudioSource audioSource; // Reference to the AudioSource component

    private void Start()
    {
        // Get reference to AudioSource component
        audioSource = GetComponent<AudioSource>();

        // Add AudioSource component if not already present
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the bullet collided with an enemy (e.g., tagged as "Enemy")
        if (other.CompareTag("Enemy") && !hasHitEnemy)
        {
            // Deal damage to the enemy
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }

            // Apply force to make the enemy float
            Rigidbody2D enemyRb = other.GetComponent<Rigidbody2D>();
            if (enemyRb != null)
            {
                enemyRb.velocity = new Vector2(enemyRb.velocity.x, floatForce);
                hasHitEnemy = true; // Set the flag to true to prevent further damage
                // Reset the floating after a delay
                Invoke(nameof(ResetFloating), floatDuration);

                // Play hit sound effect
                if (hitSound != null)
                {
                    audioSource.PlayOneShot(hitSound);
                }
            }
        }
        else if (other.CompareTag("Ground"))
        {
            // Destroy the bullet when colliding with the ground
            Destroy(gameObject);
        }
    }

    private void ResetFloating()
    {
        hasHitEnemy = false;
    }
}
