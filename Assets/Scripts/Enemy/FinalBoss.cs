using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class TeleportAndFirePoint
{
    public Transform teleportPoint;
    public List<Transform> firePoints = new List<Transform>();
}

public class FinalBoss : MonoBehaviour
{
    public float moveSpeed = 10f; // Speed of the boss
    public float attackRange = 2f; // Range at which the boss attacks the player
    public float slashDistance = 3f; // Distance the boss slashes past the player
    public float slashCooldown = 3f; // Cooldown duration for the slash
    public int damageAmount = 1; // Damage amount when the boss slashes past the player
    public TeleportAndFirePoint[] teleportAndFirePoints; // Combined teleport and fire points
    public float teleportDelay = 1.5f; // Delay after the slash before teleportation
    public GameObject fireballPrefab; // Prefab for the fireball
    public AudioClip fireballSound; // Sound effect for the fireball

    private Transform player; // Reference to the player's transform
    private bool isCooldown = false; // Flag to indicate if the boss is in cooldown after slashing
    private Animator animator; // Reference to the Animator component
    private static readonly int FinalBossAttack = Animator.StringToHash("FinalBossAttack");
    private static readonly int FinalBossRun = Animator.StringToHash("FinalBossRun");
    private static readonly int FinalBossUncloaking = Animator.StringToHash("FinalBossUncloaking");

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
            animator.SetBool(FinalBossRun, true);
        }
        else
        {
            // Reset the "isMoving" parameter to false
            animator.SetBool(FinalBossRun, false);
        }
    }

    void StartSlash()
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

        // Trigger the slash animation
        animator.SetTrigger(FinalBossAttack);

        // Move the boss quickly past the player
        transform.position += direction * slashDistance;

        // Enter cooldown after slashing
        isCooldown = true;

        // Set a cooldown period before the boss can slash again
        Invoke("ResetCooldown", slashCooldown);

        // After slashing, wait for a delay before teleporting
        Invoke("TeleportAfterSlash", teleportDelay);
    }



    void ResetCooldown()
    {
        // Reset cooldown flag
        isCooldown = false;
    }

    void TeleportAfterSlash()
    {
        // Choose a random teleport point index
        int randomIndex = Random.Range(0, teleportAndFirePoints.Length);

        // Teleport to the chosen teleport point
        transform.position = teleportAndFirePoints[randomIndex].teleportPoint.position;

        // Spawn fireballs associated with the teleport point
        SpawnFireballs(randomIndex);
    }

    void SpawnFireballs(int teleportPointIndex)
    {
        // Get the associated fire points for the teleport point
        List<Transform> firePoints = teleportAndFirePoints[teleportPointIndex].firePoints;

        // Check if fire points are valid and fireball prefab is assigned
        if (firePoints != null && fireballPrefab != null)
        {
            // Play sound effect for the fireball
            if (fireballSound != null)
            {
                AudioSource.PlayClipAtPoint(fireballSound, transform.position);
            }

            // Spawn fireballs at each fire point
            foreach (Transform firePoint in firePoints)
            {
                Instantiate(fireballPrefab, firePoint.position, Quaternion.identity);
            }
        }
        else
        {
            Debug.LogWarning("Fire points or fireball prefab is not assigned!");
        }
    }
}




