using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 1; // Maximum health of the zombie
    private int _currentHealth; // Current health of the zombie
    public Animator animator; // Changed from Animator to animator, follow C# conventions
    public AudioClip deathSound; // Sound effect for the enemy death
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

        // Destroy the zombie after animation finishes
        Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision is with an object tagged as "Bullet"
        if (collision.gameObject.CompareTag("Bullet"))
        {
            // Apply damage to the zombie
            TakeDamage(1); // Assuming each bullet deals 1 damage

            // Destroy the bullet
            Destroy(collision.gameObject);
        }
    }
}