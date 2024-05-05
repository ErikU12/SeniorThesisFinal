using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerHealthTutorial : MonoBehaviour
{
    public int maxHealth = 100;
    [SerializeField] private int currentHealth;
    public SpriteRenderer playerSpriteRenderer; 
    public Sprite fullHealthSprite; 
    public Sprite damagedSprite; 
    public Sprite criticalSprite; 
    public Sprite deadSprite; 
    public AudioClip damageSound; 
    public float invincibleDuration = 2f; 
    public float knockbackForce = 10f; 
    public float knockbackDuration = 0.5f; 
    public float laserKnockbackForce = 5f; 
    public Animator playerAnimator; // Reference to the Animator component for the player

    private Rigidbody2D rb;
    private Vector3 respawnPoint; 
    private bool collidedWithCheckpoint = false; 
    private bool isInvincible = false; 
    private float invincibleTimer = 0f; 
    private bool isKnockedBack = false; 
    private float knockbackTimer = 0f; 
    private Vector2 knockbackDirection; 
    private ArrowSpawner arrowSpawner;
    private static readonly int PlayerDeath = Animator.StringToHash("PlayerDeath");

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        respawnPoint = transform.position; 
        UpdatePlayerSprite();
        arrowSpawner = GetComponent<ArrowSpawner>(); 
    }

    private void Update()
    {
        if (isInvincible)
        {
            invincibleTimer += Time.deltaTime;

            if (invincibleTimer >= invincibleDuration)
            {
                isInvincible = false;
                invincibleTimer = 0f;
            }
        }

        if (isKnockedBack)
        {
            knockbackTimer += Time.deltaTime;

            if (knockbackTimer >= knockbackDuration)
            {
                isKnockedBack = false;
                knockbackTimer = 0f;
            }
            else
            {
                rb.velocity = knockbackDirection * knockbackForce;
            }
        }

        if (currentHealth <= 0)
        {
            if (!collidedWithCheckpoint)
            {
                // Play death animation
                playerAnimator.SetTrigger(PlayerDeath);

                // Wait until the death animation finishes playing
                float deathAnimationLength = GetAnimationLength(playerAnimator, "PlayerDeath");
                StartCoroutine(WaitForDeathAnimation(deathAnimationLength));
            }
            else
            {
                Respawn();
            }
        }
    }

    private IEnumerator WaitForDeathAnimation(float duration)
    {
        yield return new WaitForSeconds(duration);

        // Load the "GameOver" scene
        SceneManager.LoadScene("GameOverTutorial");
    }

    private float GetAnimationLength(Animator animator, string triggerName)
    {
        float length = 0f;

        // Get the AnimatorController attached to the Animator component
        RuntimeAnimatorController animatorController = animator.runtimeAnimatorController;

        // Check if the AnimatorController exists
        if (animatorController != null)
        {
            // Iterate over all the animations in the AnimatorController
            foreach (AnimationClip clip in animatorController.animationClips)
            {
                // Find the animation clip with the specified trigger name
                if (clip.name.Equals(triggerName))
                {
                    // Get the length of the animation clip
                    length = clip.length;
                    break;
                }
            }
        }

        return length;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (gameObject.CompareTag("PlayerMelee") && !isInvincible)
        {
            return;
        }

        if (!isInvincible && collision.gameObject.CompareTag("Enemy")) 
        {
            TakeDamage(1);
            isInvincible = true;
            isKnockedBack = true;
            knockbackDirection = (transform.position - collision.transform.position).normalized;
        }
        else if (!isInvincible && collision.gameObject.CompareTag("Laser")) 
        {
            isInvincible = true;
            isKnockedBack = true;
            knockbackDirection = (transform.position - collision.transform.position).normalized;
            rb.velocity = knockbackDirection * laserKnockbackForce;
        }
        else if (!isInvincible && collision.gameObject.CompareTag("Water")) 
        {
            TakeDamage(100); // Deal 100 damage on collision with water
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Potion"))
        {
            IncreaseHealth(1);
            Destroy(other.gameObject); 
        }
        else if (other.gameObject.CompareTag("Checkpoint"))
        {
            collidedWithCheckpoint = true; 
            respawnPoint = other.transform.position; 
        }
    }

    public void SetRespawnPoint(Vector3 point)
    {
        respawnPoint = point; 
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdatePlayerSprite();

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
            currentHealth = maxHealth; 
        }
        UpdatePlayerSprite();
    }

    private void UpdatePlayerSprite()
    {
        if (currentHealth == 1)
        {
            playerSpriteRenderer.sprite = criticalSprite; 
        }
        else if (currentHealth == 2)
        {
            playerSpriteRenderer.sprite = damagedSprite; 
        }
        else if (currentHealth == 0)
        {
            playerSpriteRenderer.sprite = deadSprite; 
        }
        else
        {
            playerSpriteRenderer.sprite = fullHealthSprite; 
        }
    }

    private void Respawn()
    {
        transform.position = respawnPoint; 
        currentHealth = maxHealth; 
        UpdatePlayerSprite(); 

        if (arrowSpawner != null)
        {
            arrowSpawner.ResetArrowCount(); // Reset arrow count upon respawn
        }
    }
}