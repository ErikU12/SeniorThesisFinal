using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 1; // Maximum health of the enemy
    private int _currentHealth; // Current health of the enemy
    public Animator animator; // Reference to the Animator component
    public AudioClip deathSound; // Sound effect for the enemy death
    public float knockbackForce = 10f; // Force of knockback
    private static readonly int EnemyDeath = Animator.StringToHash("EnemyDeath");

    private void Start()
    {
        _currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            Die(); // Call the Die method when health reaches zero
        }
    }

    private void Die()
    {
        // Play death animation
        animator.SetTrigger(EnemyDeath);

        // Play death sound effect
        if (deathSound != null)
        {
            AudioSource.PlayClipAtPoint(deathSound, transform.position);
        }

        // Destroy the enemy after animation finishes
        Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision is with an object tagged as "Bullet"
        if (collision.gameObject.CompareTag("Bullet"))
        {
            // Apply damage to the enemy
            TakeDamage(1); // Assuming each bullet deals 1 damage

            // Destroy the bullet
            Destroy(collision.gameObject);
        }
        // Check if the collision is with the player tagged as "PlayerMelee"
        else if (collision.gameObject.CompareTag("PlayerMelee"))
        {
            // Deal 2 damage to the enemy
            TakeDamage(2);

            // Calculate knockback direction from the enemy to the player
            Vector2 knockbackDirection = (transform.position - collision.transform.position).normalized;

            // Apply knockback force
            Rigidbody2D enemyRB = GetComponent<Rigidbody2D>();
            if (enemyRB != null)
            {
                enemyRB.velocity = knockbackDirection * knockbackForce;
            }
        }
    }
}
