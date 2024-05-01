using UnityEngine;

public class SkellyEnemy : MonoBehaviour
{
    public float chargeSpeed = 5f; // Speed of the charge
    public float chargeCooldown = 2f; // Cooldown between charges
    public float chargeDuration = 1f; // Duration of the charge
    public float knockbackForce = 10f; // Force of knockback when colliding with the player
    public float knockbackDuration = 0.5f; // Duration of knockback when colliding with the player
    public float detectionRange = 5f; // Detection range for the player
    public GameObject equippedHitbox; // Reference to the hitbox GameObject

    private Transform player; // Reference to the player's transform
    private bool isCharging = false; // Flag to track if the enemy is currently charging
    private bool isCoolingDown = false; // Flag to track if the enemy is currently cooling down after a charge
    private float chargeTimer = 0f; // Timer for tracking charge duration
    private float cooldownTimer = 0f; // Timer for tracking cooldown duration
    private Animator animator; // Reference to the Animator component

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // Find the player's transform
        animator = GetComponent<Animator>(); // Get the Animator component

        // Detach the hitbox from the enemy if it's specified
        if (equippedHitbox != null)
        {
            equippedHitbox.transform.parent = null;
        }
    }

    private void Update()
    {
        // Check if the player is within detection range
        if (Vector2.Distance(transform.position, player.position) <= detectionRange)
        {
            // If not charging and not cooling down, start charging
            if (!isCharging && !isCoolingDown)
            {
                Charge();
            }
        }

        if (isCharging)
        {
            // If charging, move towards the player
            Vector2 targetPosition = new Vector2(player.position.x, transform.position.y); // Maintain current y position
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, chargeSpeed * Time.deltaTime);

            // Update charge duration timer
            chargeTimer += Time.deltaTime;
            if (chargeTimer >= chargeDuration)
            {
                // If charge duration has elapsed, stop charging
                chargeTimer = 0f;
                isCharging = false;
                isCoolingDown = true;
                animator.SetBool("ShieldEnemyBashRun", true); // Set bash run animation to true
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
                animator.SetBool("ShieldEnemyBashRun", false); // Set bash run animation to false
                animator.SetBool("ShieldEnemyRun", false); // Set run animation to false
                animator.SetBool("ShieldEnemyBashStill", true); // Play attack animation
            }
            else
            {
                // While cooling down, ensure run animation is off
                animator.SetBool("ShieldEnemyRun", false);
                animator.SetBool("ShieldEnemyBashStill", false); // Set bash still animation to false
            }
        }

        // Update hitbox position
        if (equippedHitbox != null)
        {
            equippedHitbox.transform.position = transform.position;
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

    private void Charge()
    {
        isCharging = true;
        animator.SetBool("ShieldEnemyRun", true); // Set run animation to true
    }

    public Vector3 hitboxOffset = Vector3.zero; // Offset for the hitbox position
}
