using UnityEngine;

public class FinalBossHealth : MonoBehaviour
{
    public int maxHealth = 10; // Maximum health of the boss
    public Animator animator; // Reference to the Animator component
    public GameObject deathEffect; // Particle effect for boss death

    private int currentHealth; // Current health of the boss
    private bool isDead = false; // Flag to indicate if the boss is dead
    private static readonly int HoodedFigureDeath = Animator.StringToHash("HoodedFigureDeath");

    void Start()
    {
        // Initialize current health to max health
        currentHealth = maxHealth;
    }

    // Method to take damage
    public void TakeDamage(int damage)
    {
        // Check if the boss is already dead
        if (isDead)
            return;

        // Reduce health
        currentHealth -= damage;

        // Play hurt animation (if you have one)
        // Example: animator.SetTrigger("Hurt");

        // Check if health reached zero
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Method to handle boss death
    void Die()
    {
        // Set dead flag to true
        isDead = true;

        // Play death animation
        if (animator != null)
        {
            animator.SetTrigger(HoodedFigureDeath);
        }

        // Spawn death effect
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }

        // Disable boss GameObject or other death-related actions
        gameObject.SetActive(false);
    }
}