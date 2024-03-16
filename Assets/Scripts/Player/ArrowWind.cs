using UnityEngine;

public class ArrowWind : MonoBehaviour
{
    public int damage = 1; // Damage dealt by the bullet
    public float floatDuration = 2f; // Duration for which the enemy floats in the air
    public float floatForce = 10f; // Force applied to the enemy to make it float
    private bool hasHitEnemy = false; // Flag to track if the arrow has hit an enemy

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