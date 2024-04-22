using UnityEngine;

public class Arrow : MonoBehaviour
{
    public int damage = 1; // Damage dealt by the bullet
    public AudioClip destroySound; // Sound effect to play on destroy

    // Adjust this value to play the sound slightly earlier
    public float soundOffset = 0.1f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the bullet collided with an enemy (e.g., tagged as "Enemy")
        if (other.CompareTag("Enemy"))
        {
            // Deal damage to the enemy
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }

            // Play the specified destroy sound effect with a slight offset
            AudioSource.PlayClipAtPoint(destroySound, transform.position, soundOffset);

            // Destroy the bullet
            Destroy(gameObject);
        }
        else if (other.CompareTag("Ground"))
        {
            // Destroy the bullet when colliding with the ground
            Destroy(gameObject);
        }
    }
}