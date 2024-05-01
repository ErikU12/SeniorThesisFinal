using UnityEngine;

public class FinalBoss : MonoBehaviour
{
    public float moveSpeed = 10f; // Speed of the boss
    public float attackRange = 2f; // Range at which the boss attacks the player
    public int damageAmount = 1; // Damage amount when the boss slashes past the player

    private Transform player; // Reference to the player's transform
    private bool isCooldown = false; // Flag to indicate if the boss is in cooldown after slashing
    public Animator animator; // Reference to the Animator component
    private static readonly int HoodedEnemyAttack = Animator.StringToHash("HoodedEnemyAttack");
    private static readonly int HoodedEnemyRunning = Animator.StringToHash("HoodedEnemyRunning");
    private static readonly int HoodedEnemyUnCloaked = Animator.StringToHash("HoodedEnemyUnCloaked");

    void Start()
    {
        // Find the player's transform
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Get reference to the Animator component
        animator = GetComponent<Animator>();

        // Set a transition to hooded enemy uncloaked on awake
        animator.SetTrigger(HoodedEnemyUnCloaked);
    }

    void Update()
    {
        // If not in cooldown, move towards the player
        if (!isCooldown)
        {
            MoveTowardsPlayer();
        }

        // Check if the player is within attack range
        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            // Trigger attack animation and initiate attack
            animator.SetTrigger(HoodedEnemyAttack);
            StartSlash();
        }
        else
        {
            // Player is out of attack range, reset attack animation trigger
            animator.SetBool(HoodedEnemyAttack, false);
        }
    }

    void MoveTowardsPlayer()
    {
        // Calculate direction towards the player
        Vector3 direction = (player.position - transform.position).normalized;

        // Move the boss towards the player
        transform.Translate(direction * (moveSpeed * Time.deltaTime));

        // Set the appropriate animation parameter based on movement
        if (direction.x != 0 || direction.y != 0)
        {
            // Trigger the "isMoving" parameter to transition to the moving animation
            animator.SetBool(HoodedEnemyRunning, true);
        }
        else
        {
            // Reset the "isMoving" parameter to false
            animator.SetBool(HoodedEnemyRunning, false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if collided with the player while slashing
        if (other.CompareTag("Player"))
        {
            // Deal damage to the player
            DealDamage(other.gameObject);
        }
    }

    void DealDamage(GameObject target)
    {
        // Check if the target has a health component
        PlayerHealth health = target.GetComponent<PlayerHealth>();
        if (health != null)
        {
            // Deal damage to the target
            health.TakeDamage(damageAmount);
        }
    }

    public void StartSlash()
    {
        // Move the boss quickly past the player
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);

        // Enter cooldown after slashing
        isCooldown = true;

        // Set a cooldown period before the boss can slash again
        Invoke("ResetCooldown", 1f);

        // Trigger the "Slash" parameter to transition to the slashing animation
        animator.SetTrigger(HoodedEnemyAttack);
    }

    void ResetCooldown()
    {
        // Reset cooldown flag
        isCooldown = false;
    }
}
