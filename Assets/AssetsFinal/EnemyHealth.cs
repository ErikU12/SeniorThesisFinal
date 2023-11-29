using UnityEngine;

public class ZombieHealth : MonoBehaviour
{
    public int maxHealth = 10; // Maximum health of the zombie
    private int _currentHealth; // Current health of the zombie

    private void Start()
    {
        _currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            // Zombie is dead, destroy it
            Destroy(gameObject);
        }
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