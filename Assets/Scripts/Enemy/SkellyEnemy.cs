using UnityEngine;

public class SkellyEnemy : MonoBehaviour
{
    public float chargeSpeed = 5f; // Speed of the charge
    public float chargeCooldown = 2f; // Cooldown between charges
    public float chargeDuration = 1f; // Duration of the charge
    public float knockbackForce = 10f; // Force of knockback when colliding with the player
    public float knockbackDuration = 0.5f; // Duration of knockback when colliding with the player

    private Transform player; // Reference to the player's transform
    private bool isCharging = false; // Flag to track if the enemy is currently charging
    private bool isCoolingDown = false; // Flag to track if the enemy is currently cooling down after a charge
    private float chargeTimer = 0f; // Timer for tracking charge duration
    private float cooldownTimer = 0f; // Timer for tracking cooldown duration

    private SpriteRenderer spriteRenderer; // Reference to the sprite renderer component
    private Animator animator; // Reference to the Animator component
    private Collider2D immuneHitbox; // Reference to the immune hitbox collider
    private static readonly int ShieldEnemyBashRun = Animator.StringToHash("ShieldEnemyBashRun");
    private static readonly int ShieldEnemyRun = Animator.StringToHash("ShieldEnemyRun");
    private static readonly int ShieldEnemyBashStill = Animator.StringToHash("ShieldEnemyBashStill");

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // Find the player's transform
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the sprite renderer component
        animator = GetComponent<Animator>(); // Get the Animator component

        // Find the immune hitbox collider
        immuneHitbox = GetComponentInChildren<Collider2D>();
    }

    private void Update()
    {
        if (!isCharging && !isCoolingDown)
        {
            // If not charging and not cooling down, start charging
            Charge();
        }

        if (isCharging)
        {
            // If charging, move towards the player
            transform.position = Vector2.MoveTowards(transform.position, player.position, chargeSpeed * Time.deltaTime);

            // Flip sprite to face the player
            FlipSprite(player.position.x > transform.position.x);
            
            // Update charge duration timer
            chargeTimer += Time.deltaTime;
            if (chargeTimer >= chargeDuration)
            {
                // If charge duration has elapsed, stop charging
                chargeTimer = 0f;
                isCharging = false;
                isCoolingDown = true;
                animator.SetBool(ShieldEnemyBashRun, true); // Set bash run animation to true
            }
        }

        if (isCoolingDown)
        {
            // If cooling down, update cooldown timer
            cooldownTimer += Time.deltaTime;
            if (cooldownTimer >= chargeCooldown)
            {
                // If cooldown duration has elapsed, stop cooling down
                cooldownTimer = 0f;
                isCoolingDown = false;
                animator.SetBool(ShieldEnemyBashRun, false); // Set bash run animation to false
                animator.SetBool(ShieldEnemyRun, false); // Set run animation to false
                animator.SetBool(ShieldEnemyBashStill, true); // Play attack animation
            }
            else
            {
                // While cooling down, ensure run animation is off
                animator.SetBool(ShieldEnemyRun, false);
                animator.SetBool(ShieldEnemyBashStill, false); // Set bash still animation to false
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // If collided with the player, apply knockback and stop charging
            Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = knockbackDirection * knockbackForce;
            isCharging = false;
        }
    }

    // Method to handle the immune hitbox preventing damage
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet") || other.CompareTag("PlayerMelee"))
        {
            // Ignore damage if hit by a bullet or player melee attack
            // This prevents the enemy from taking damage
            return;
        }
    }

    private void Charge()
    {
        isCharging = true;
        animator.SetBool(ShieldEnemyRun, true); // Set run animation to true
    }

    private void FlipSprite(bool faceRight)
    {
        spriteRenderer.flipX = !faceRight;
    }
}

