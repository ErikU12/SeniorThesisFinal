using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 1; // Maximum health of the enemy
    private int _currentHealth; // Current health of the enemy
    public Animator _animator; // Reference to the Animator component

    void Start()
    {
        _currentHealth = maxHealth;
        // Find the parent GameObject which has the Animator component
        _animator = transform.parent.GetComponent<Animator>();

        if (_animator == null)
        {
            Debug.LogError("Animator component not found in the parent GameObject.");
        }
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            // Play death animation
            PlayDeathAnimation();

            // Disable enemy's collider to prevent further interaction
            GetComponent<Collider2D>().enabled = false;

            // Disable any scripts attached to the enemy to stop its behavior
            // For example, you could disable movement scripts, shooting scripts, etc.
            // GetComponent<YourScriptType>().enabled = false;

            // Destroy the enemy GameObject after a delay
            Destroy(gameObject, 1f); // Adjust the delay as needed
        }
    }

    private void PlayDeathAnimation()
    {
        // Trigger the "Die" parameter in the animator controller to play the death animation
        _animator.SetTrigger("EnemyDeath");
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
    }
}