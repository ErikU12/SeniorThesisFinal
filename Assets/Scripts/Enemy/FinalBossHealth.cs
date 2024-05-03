using UnityEngine;
using System.Collections;

public class FinalBossHealth : MonoBehaviour
{
    public int maxHealth = 20; // Increase max health for the final boss
    public float flashInterval = 0.5f; // Interval for flashing effect
    public float speedMultiplier = 2f; // Multiplier for boss speed and fireball speed when low health
    public Color flashColor = Color.red; // Color for flashing effect
    public GameObject deathPrefab; // Prefab to spawn upon boss death
    public Transform spawnLocation; // Location to spawn the death prefab
    public Animator animator; // Reference to the Animator component
    public AudioSource audioSource; // Reference to the AudioSource component for death sound
    public AudioClip deathSound; // AudioClip for death sound effect
    private static readonly int FinalBossDeath = Animator.StringToHash("FinalBossDeath");

    internal int currentHealth;
    private bool isFlashing = false;
    private bool isFastMode = false;
    private bool hasDied = false; // Flag to prevent multiple calls to Die()
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component attached to this GameObject
    }

    void Update()
    {
        if (currentHealth <= maxHealth / 2 && !isFastMode)
        {
            // Activate fast mode if boss health is low
            isFastMode = true;
            speedMultiplier *= 2; // Double the speed
            GetComponent<FinalBossPhase1>().ActivateLowHealthMode(); // Activate low health mode in the FinalBossPhase1 script
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

        if (currentHealth <= 0 && !hasDied) // Check if the boss has not already died
        {
            Debug.Log("Boss is dead!"); // Add debug log
            Die();
        }
        else if (currentHealth <= maxHealth / 2 && !isFlashing)
        {
            Debug.Log("Boss health is low!"); // Add debug log
            // Start flashing effect if boss health is low
            isFlashing = true;
            StartCoroutine(FlashEffect());
        }
    }

    // Method to handle boss death
    void Die()
    {
        // Set the flag to indicate that the boss has died
        hasDied = true;

        Debug.Log("Boss is dying!"); // Add debug log

        // Play death animation
        animator.SetTrigger(FinalBossDeath);

        // Play death sound effect if AudioClip is assigned
        if (audioSource != null && deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }

        // Invoke method to spawn the death prefab after a delay
        Invoke("SpawnDeathPrefab", 2.0f); // Adjust the delay as needed
    }

    // Method to spawn the death prefab
    void SpawnDeathPrefab()
    {
        // Spawn the death prefab at the specified location
        if (deathPrefab != null && spawnLocation != null)
        {
            Debug.Log("Spawning death prefab!"); // Add debug log
            Instantiate(deathPrefab, spawnLocation.position, Quaternion.identity);
        }

        // Destroy the boss object after the animation finishes
        Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
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
                Debug.Log("Boss hit by a bullet!"); // Add debug log
                TakeDamage(1); // Adjust damage as needed
            }

            // Destroy the bullet on collision
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("FlameShield"))
        {
            Debug.Log("Boss hit by Flameshield!"); // Add debug log
            TakeDamage(3); // Apply 3 damage when colliding with Flameshield
        }
    }
}


