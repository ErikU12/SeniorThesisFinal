using System.Collections;
using UnityEngine;

public class ArrowFire : MonoBehaviour
{
    public int damagePerTick = 1; // Damage per tick of the DoT effect
    public float knockbackForce = 10f; // Force applied to the enemy per tick
    public float tickInterval = 1f; // Time interval between each tick
    public float totalDuration = 3f; // Total duration of the DoT effect
    public Color damageColor = Color.red; // Color to apply when damaged
    private float timer; // Timer to keep track of time

    private void Update()
    {
        // Increment the timer
        timer += Time.deltaTime;

        // Check if the total duration of the DoT effect has passed
        if (timer >= totalDuration)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the bullet collided with an enemy (e.g., tagged as "Enemy")
        if (other.CompareTag("Enemy"))
        {
            // Start applying damage over time effect
            ApplyDoTEffect(other.gameObject);
        }
        else if (other.CompareTag("Ground"))
        {
            // Destroy the bullet when colliding with the ground
            Destroy(gameObject);
        }
    }

    private void ApplyDoTEffect(GameObject target)
    {
        // Start the coroutine to apply damage over time
        StartCoroutine(DoTDamage(target));
    }

    private IEnumerator DoTDamage(GameObject target)
    {
        // Reference to the SpriteRenderer component of the enemy
        SpriteRenderer enemyRenderer = target.GetComponent<SpriteRenderer>();

        // Continue applying damage over time until the total duration is reached
        while (timer < totalDuration)
        {
            // Apply damage to the enemy
            EnemyHealth enemyHealth = target.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damagePerTick);
            }

            // Change the color of the enemy
            if (enemyRenderer != null)
            {
                enemyRenderer.color = damageColor;
            }

            // Wait for a short duration to emphasize the color change
            yield return new WaitForSeconds(0.1f);

            // Reset the color of the enemy
            if (enemyRenderer != null)
            {
                enemyRenderer.color = Color.white; // Reset to default color
            }

            // Apply knockback force to the enemy
            Rigidbody2D enemyRigidbody = target.GetComponent<Rigidbody2D>();
            if (enemyRigidbody != null)
            {
                // Calculate the direction away from the arrow
                Vector2 knockbackDirection = (target.transform.position - transform.position).normalized;

                // Apply the force
                enemyRigidbody.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            }

            // Wait for the specified interval before applying the next tick
            yield return new WaitForSeconds(tickInterval);
        }

        // Destroy the bullet after the DoT effect ends
        Destroy(gameObject);
    }
}
