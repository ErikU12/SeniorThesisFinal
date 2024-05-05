using UnityEngine;

public class FlameShield : MonoBehaviour
{
    public int baseDamage = 10; // Base damage for each FlameSkull collision
    private int totalDamage = 0; // Total damage accumulated from FlameSkull collisions

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("FlameSkull"))
        {
            Debug.Log("FlameSkull detected by FlameShield.");

            // Change color of the shield to red
            Renderer shieldRenderer = GetComponent<Renderer>();
            if (shieldRenderer != null)
            {
                shieldRenderer.material.color = Color.red;
            }

            // Change the tag of this GameObject to "FlameShield"
            gameObject.tag = "FlameShield";

            // Destroy the FlameSkull
            Destroy(other.gameObject);

            // Increase damage based on the number of FlameSkulls destroyed
            totalDamage += baseDamage;

            // Check if the collided object is an enemy (boss)
            if (gameObject.CompareTag("Enemy"))
            {
                Debug.Log("Enemy damaged by FlameShield.");

                // Get the BossHealth component of the enemy
                BossHealth bossHealth = gameObject.GetComponent<BossHealth>();
                if (bossHealth != null)
                {
                    // Apply accumulated damage to the boss
                    bossHealth.TakeDamage(totalDamage);
                }
                else
                {
                    Debug.LogWarning("Enemy does not have BossHealth component.");
                }
            }
            else
            {
                Debug.LogWarning("Collision with non-enemy object detected.");
            }
        }
    }
}

