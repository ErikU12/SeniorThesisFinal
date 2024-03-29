using UnityEngine;

public class Arrow : MonoBehaviour
{
    public int damage = 1; // Damage dealt by the bullet

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the bullet collided with an enemy (e.g., tagged as "Enemy")
        if (other.CompareTag("Enemy"))
        {
            // Deal damage to the enemy and destroy the bullet
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
        else if (other.CompareTag("Ground"))
        {
            // Destroy the bullet when colliding with the ground
            Destroy(gameObject);
        }
    }
}

