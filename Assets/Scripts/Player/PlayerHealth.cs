using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    [SerializeField] private int currentHealth;
    public SpriteRenderer playerSpriteRenderer; // Reference to the SpriteRenderer component
    public Sprite fullHealthSprite; // Sprite to use when health is full
    public Sprite damagedSprite; // Sprite to use when health is 2 or more
    public Sprite criticalSprite; // Sprite to use when health is 1
    public Sprite deadSprite; // Sprite to use when health is 0 (dead)
    public AudioClip damageSound; // Sound effect to play when the player takes damage
    public float invincibleDuration = 2f; // Duration of invincibility after taking damage
    public float knockbackForce = 10f; // Force of knockback
    public float knockbackDuration = 0.5f; // Duration of knockback
    public float laserKnockbackForce = 5f; // Force of knockback from laser

    private Rigidbody2D rb;
    private Vector3 respawnPoint; // Respawn point for the player
    private bool collidedWithCheckpoint = false; // Flag to track if player collided with a checkpoint
    private bool isInvincible = false; // Flag to track if the player is currently invincible
    private float invincibleTimer = 0f; // Timer for tracking invincibility duration
    private bool isKnockedBack = false; // Flag to track if the player is currently knocked back
    private float knockbackTimer = 0f; // Timer for tracking knockback duration
    private Vector2 knockbackDirection; // Direction of knockback

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        respawnPoint = transform.position; // Set initial respawn point to player's position
        UpdatePlayerSprite();
    }

    private void Update()
    {
        // Update invincibility timer if the player is currently invincible
        if (isInvincible)
        {
            invincibleTimer += Time.deltaTime;

            // If invincibility duration has elapsed, make the player vulnerable again
            if (invincibleTimer >= invincibleDuration)
            {
                isInvincible = false;
                invincibleTimer = 0f;
            }
        }

        // Update knockback if the player is currently knocked back
        if (isKnockedBack)
        {
            knockbackTimer += Time.deltaTime;

            // If knockback duration has elapsed, stop knockback
            if (knockbackTimer >= knockbackDuration)
            {
                isKnockedBack = false;
                knockbackTimer = 0f;
            }
            else
            {
                // Apply knockback force
                rb.velocity = knockbackDirection * knockbackForce;
            }
        }

        // Check if player should respawn
        if (currentHealth <= 0)
        {
            if (!collidedWithCheckpoint)
            {
                // Player has not collided with a checkpoint and health is zero, go to game over scene
                SceneManager.LoadScene("GameOver");
            }
            else
            {
                // Player has collided with a checkpoint and health is zero, respawn at checkpoint
                Respawn();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player has the "playermelee" tag and if they are not invincible
        if (gameObject.CompareTag("PlayerMelee") && !isInvincible)
        {
            // Do not take damage if the player has the "playermelee" tag
            return;
        }

        if (!isInvincible && collision.gameObject.CompareTag("Enemy")) // Check if player is not invincible
        {
            TakeDamage(1);
            // Make the player invincible after taking damage
            isInvincible = true;
            // Trigger knockback
            isKnockedBack = true;
            // Calculate knockback direction
            knockbackDirection = (transform.position - collision.transform.position).normalized;
        }
        else if (!isInvincible && collision.gameObject.CompareTag("Laser")) // Check if player is not invincible and collided with a laser
        {
            // Make the player invincible after taking damage
            isInvincible = true;
            // Trigger knockback from laser
            isKnockedBack = true;
            // Calculate knockback direction from laser
            knockbackDirection = (transform.position - collision.transform.position).normalized;
            // Apply knockback force from laser
            rb.velocity = knockbackDirection * laserKnockbackForce;
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Potion"))
        {
            IncreaseHealth(1);
            Destroy(other.gameObject); // Destroy the potion upon collision
        }
        else if (other.gameObject.CompareTag("Checkpoint"))
        {
            collidedWithCheckpoint = true; // Set flag to true when player collides with a checkpoint
            respawnPoint = other.transform.position; // Update respawn point to the checkpoint's position
        }
    }

    public void SetRespawnPoint(Vector3 point)
    {
        respawnPoint = point; // Set the respawn point to the specified point
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdatePlayerSprite();

        // Play the damage sound effect
        if (damageSound != null)
        {
            AudioSource.PlayClipAtPoint(damageSound, transform.position);
        }
    }

    public void IncreaseHealth(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth; // Ensure health doesn't exceed maximum
        }
        UpdatePlayerSprite();
    }

    private void UpdatePlayerSprite()
    {
        // Check current health and set player sprite accordingly
        if (currentHealth == 1)
        {
            playerSpriteRenderer.sprite = criticalSprite; // Change to critical sprite when health is 1
        }
        else if (currentHealth == 2)
        {
            playerSpriteRenderer.sprite = damagedSprite; // Change to damaged sprite when health is 2 or less
        }
        else if (currentHealth == 0)
        {
            playerSpriteRenderer.sprite = deadSprite; // Change to dead sprite when health is 0
        }
        else
        {
            playerSpriteRenderer.sprite = fullHealthSprite; // Set the sprite to full health sprite for other cases
        }
    }

    private void Respawn()
    {
        transform.position = respawnPoint; // Move the player to the respawn point
        currentHealth = maxHealth; // Reset the player's health
        UpdatePlayerSprite(); // Update the player's sprite
    }
}


