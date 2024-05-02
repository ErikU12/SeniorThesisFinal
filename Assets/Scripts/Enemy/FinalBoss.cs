using UnityEngine;

public class FinalBoss : MonoBehaviour
{
    public float moveSpeed = 10f; // Speed of the boss
    public float attackRange = 2f; // Range at which the boss attacks the player
    public float slashDistance = 3f; // Distance the boss slashes past the player
    public float slashCooldown = 3f; // Cooldown duration for the slash
    public int damageAmount = 1; // Damage amount when the boss slashes past the player

    private Transform player; // Reference to the player's transform
    private bool isCooldown = false; // Flag to indicate if the boss is in cooldown after slashing
    public Animator animator; // Reference to the Animator component
    private static readonly int FinalBossAttack = Animator.StringToHash("FinalBossAttack");
    private static readonly int HoodedEnemyRunning = Animator.StringToHash("HoodedEnemyRunning");
    private static readonly int FinalBossUncloaking = Animator.StringToHash("FinalBossUncloaking");
    private static readonly int FinalBossSlash = Animator.StringToHash("FinalBossSlash");

    void Start()
    {
        // Find the player's transform
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Get reference to the Animator component
        animator = GetComponent<Animator>();

        // Set a transition to hooded enemy uncloaked on awake
        animator.SetTrigger(FinalBossUncloaking);
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
            // If not in cooldown, start the slash
            if (!isCooldown)
            {
                StartSlash();
            }
        }
        else
        {
            // Player is out of attack range, reset attack animation trigger
            animator.SetBool(FinalBossAttack, false);
        }
    }

    void MoveTowardsPlayer()
    {
        // Calculate direction towards the player
        Vector3 direction = (player.position - transform.position).normalized;

        // Flip the sprite if needed
        if (direction.x < 0)
        {
            // If the player is to the left, flip the sprite
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (direction.x > 0)
        {
            // If the player is to the right, ensure sprite faces right
            transform.localScale = new Vector3(1f, 1f, 1f);
        }

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

    void StartSlash()
    {
        // Calculate direction towards the player
        Vector3 direction = (player.position - transform.position).normalized;

        // Trigger the slash animation
        animator.SetTrigger(FinalBossAttack);

        // Move the boss quickly past the player
        transform.position += direction * slashDistance;

        // Enter cooldown after slashing
        isCooldown = true;

        // Set a cooldown period before the boss can slash again
        Invoke("ResetCooldown", slashCooldown);
    }

    void ResetCooldown()
    {
        // Reset cooldown flag
        isCooldown = false;
    }
}
