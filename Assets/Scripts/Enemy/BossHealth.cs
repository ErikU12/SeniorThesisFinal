using System.Collections;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    public int maxHealth = 10;
    public float flashInterval = 0.5f; // Interval for flashing effect
    public float speedMultiplier = 2f; // Multiplier for boss speed and fireball speed when low health
    public Color flashColor = Color.red; // Color for flashing effect
    public GameObject deathPrefab; // Prefab to spawn upon boss death
    public Transform spawnLocation; // Location to spawn the death prefab

    private int currentHealth;
    private bool isFlashing = false;
    private bool isFastMode = false;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (currentHealth <= maxHealth / 2 && !isFastMode)
        {
            // Activate fast mode if boss health is low
            isFastMode = true;
            speedMultiplier *= 2; // Double the speed
        }
    }

    // Coroutine for flashing effect
    IEnumerator FlashEffect()
    {
        while (isFlashing)
        {
            spriteRenderer.color = flashColor;
            yield return new WaitForSeconds(flashInterval);
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(flashInterval);
        }
    }

    // Method to handle boss taking damage
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
        else if (currentHealth <= maxHealth / 2 && !isFlashing)
        {
            // Start flashing effect if boss health is low
            isFlashing = true;
            StartCoroutine(FlashEffect());
        }
    }

    // Method to handle boss death
    void Die()
    {
        // Spawn the death prefab at the specified location
        if (deathPrefab != null && spawnLocation != null)
        {
            Instantiate(deathPrefab, spawnLocation.position, Quaternion.identity);
        }

        // Destroy the boss object
        Destroy(gameObject);
    }

    // Method to handle collisions with bullets and Flameshield
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            // Get the bullet damage from the Bullet script (assuming it has one)
            Arrow arrow = other.GetComponent<Arrow>();
            if (arrow != null)
            {
                TakeDamage(1); // Adjust damage as needed
            }

            // Destroy the bullet on collision
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("FlameShield"))
        {
            TakeDamage(3); // Apply 3 damage when colliding with Flameshield
        }
    }
}

