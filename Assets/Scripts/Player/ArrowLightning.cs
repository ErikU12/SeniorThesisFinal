using UnityEngine;

public class ArrowLightning : MonoBehaviour
{
    public int damage = 1; // Damage dealt by the bullet
    public float knockbackForce = 300f; // Increased knockback force
    public AudioClip collisionSound; // Sound effect for collision
    private bool hasDealtDamage = false; // Flag to track if damage has been dealt
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
        if (other.CompareTag("Enemy") && !hasDealtDamage)
        {
            // Deal damage to the enemy
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
                hasDealtDamage = true; // Set the flag to true to prevent further damage

                // Apply knockback force to the enemy
                Rigidbody2D enemyRb = other.GetComponent<Rigidbody2D>();
                if (enemyRb != null)
                {
                    // Calculate knockback direction (opposite of arrow's movement)
                    Vector2 knockbackDirection = (other.transform.position - transform.position).normalized;
                    // Apply force to the enemy
                    enemyRb.AddForce(knockbackDirection * knockbackForce);
                }

                // Play collision sound effect
                if (collisionSound != null)
                {
                    audioSource.PlayOneShot(collisionSound);
                }
            }
        }
    }
}