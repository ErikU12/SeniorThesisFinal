using UnityEngine;

public class SkellyHealth : MonoBehaviour
{
    public int maxHealth = 1;
    public Animator animator; // Reference to the Animator component
    private static readonly int ShieldDeath = Animator.StringToHash("ShieldDeath");
    private int _currentHealth;

    private void Start()
    {
        _currentHealth = maxHealth;
    }

    // Method to handle enemy taking damage
    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    // Method to handle enemy death
    private void Die()
    {
        // Play death animation
        if (animator != null)
        {
            animator.SetTrigger(ShieldDeath);

            // Delay destruction of the object until after the animation finishes
            float animationLength = animator.GetCurrentAnimatorStateInfo(0).length;
            Destroy(gameObject, animationLength);
        }
        else
        {
            // Destroy the enemy GameObject immediately if animator is null
            Destroy(gameObject);
        }
    }

    // Handle collisions with bullets and other objects
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet") && !other.CompareTag("ShieldHitbox"))
        {
            // Get the bullet damage from the Bullet script (assuming it has one)
            Arrow arrow = other.GetComponent<Arrow>();
            if (arrow != null)
            {
                TakeDamage(1); // Adjust damage as needed
            }

            // Destroy the bullet on collision
            Destroy(other.gameObject);
        }
    }

    // Handle collisions with player melee attacks
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerMelee") && !collision.gameObject.CompareTag("ShieldHitbox"))
        {
            // Apply damage to the enemy
            TakeDamage(2); // Adjust damage as needed

            // Calculate knockback direction away from the player
            Vector2 knockbackDirection = (transform.position - collision.transform.position).normalized;

            // Apply knockback force
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                float knockbackForce = 10f; // Adjust as needed
                rb.velocity = knockbackDirection * knockbackForce;
            }
        }
    }
}

