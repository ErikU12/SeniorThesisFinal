using Enemy;
using UnityEngine;

public class LaserDeflect : MonoBehaviour
{
    public float damageAmount = 10f; // Amount of damage to apply to enemies
    public Color deflectedColor = Color.blue; // Color to change to after deflection

    private bool hasBeenDeflected = false; // Flag to track if the laser has been deflected

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasBeenDeflected && other.CompareTag("SlowBullet"))
        {
            // Reverse the direction of the laser
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = -rb.velocity;
            }

            // Change the color of the laser
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                renderer.color = deflectedColor;
            }

            // Set the flag to true
            hasBeenDeflected = true;
        }
        else if (hasBeenDeflected && other.CompareTag("Enemy"))
        {
            if (other.isTrigger) // Check if the collider is set as a trigger
            {
                // Damage the enemy
                if (other.TryGetComponent(out RoboEyeHealth roboeyeHealth))
                {
                    roboeyeHealth.TakeDamage(1);
                }
            }
            else
            {
                // Damage the enemy
                if (other.TryGetComponent(out BlasterHealth blasterHealth))
                {
                    blasterHealth.TakeDamage(1);
                }
                else if (other.TryGetComponent(out SniperHealth sniperHealth))
                {
                    sniperHealth.TakeDamage(1);
                }
            }

            // Change the color of the enemy to blue
            SpriteRenderer enemyRenderer = other.GetComponent<SpriteRenderer>();
            if (enemyRenderer != null)
            {
                enemyRenderer.color = Color.blue;
            }
        }
    }
}
