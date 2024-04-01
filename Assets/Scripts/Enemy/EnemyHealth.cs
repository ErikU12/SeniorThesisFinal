using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 1; // Maximum health of the enemy
    private int currentHealth; // Current health of the enemy
    public Animator animator; // Animator reference
    private static readonly int EnemyDeath = Animator.StringToHash("EnemyDeath");

    // Reference to the Skelly script
    private Skelly skelly;

    private void Start()
    {
        currentHealth = maxHealth;
        skelly = GetComponent<Skelly>(); // Get the Skelly component attached to the same GameObject
    }

    public void TakeDamage(int damage)
    {
        // Check if the enemy is within the shield hitbox
        if (skelly != null && skelly.IsShieldHit())
        {
            // Shield hit, no damage taken
            return;
        }

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die(); // Call the Die method when health reaches zero
        }
    }

    private void Die()
    {
        // Play death animation
        animator.SetTrigger(EnemyDeath);

        // Destroy the enemy after animation finishes
        Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Handle collision with damaging objects (e.g., bullets)
        if (collision.collider.CompareTag("Bullet"))
        {
            TakeDamage(1); // Assuming each collision deals 1 damage
        }
    }
}