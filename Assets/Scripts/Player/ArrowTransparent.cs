using UnityEngine;

public class ArrowTransparent : MonoBehaviour
{
    public int damage = 1; // Damage dealt by the arrow
    public int playerHealthRestoreAmount = 1; // Amount of health restored to the player when the arrow hits an enemy

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the arrow collided with an enemy
        if (other.CompareTag("Enemy"))
        {
            // Deal damage to the enemy
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }

            // Restore health to the player
            PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.IncreaseHealth(playerHealthRestoreAmount);
            }

            // Destroy the arrow
            Destroy(gameObject);
        }
    }
}