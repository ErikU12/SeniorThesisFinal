using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 1; // Damage dealt by the bullet

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the bullet collided with an enemy (e.g., tagged as "Enemy")
        if (other.CompareTag("Enemy"))
        {
            // Deal damage to the enemy and destroy the bullet
            ZombieHealth enemyHealth = other.GetComponent<ZombieHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}

