using UnityEngine;

public class Wolf : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float detectionRange = 5f; // The range at which the enemy detects the player
    public float attackRange = 2f; // The range at which the enemy starts jumping to attack
    public float stoppingDistance = 1f; // The distance at which the enemy stops moving towards the player
    public float jumpForce = 5f; // Force for the enemy's jump

    private Transform _player;
    private Rigidbody2D _rb;

    private bool _isJumping;
    private bool _hasJumped; // Flag to track if the jump has occurred

    void Start()
    {
        // Find the player GameObject based on the "Player" tag
        _player = GameObject.FindGameObjectWithTag("Player").transform;

        if (_player == null)
        {
            Debug.LogError("Player not found. Make sure the player has the 'Player' tag.");
        }

        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Calculate the distance between the enemy and the player
        float distanceToPlayer = Vector2.Distance(transform.position, _player.position);

        // Check if the player is in both detection and attack range, and the jump has not occurred
        if (distanceToPlayer <= detectionRange && distanceToPlayer <= attackRange && !_hasJumped)
        {
            Jump();
        }

        // Check if the player is in range
        if (distanceToPlayer <= detectionRange)
        {
            // Calculate the direction from the enemy to the player
            Vector2 moveDirection = (_player.position - transform.position).normalized;

            // If the player is not within stopping distance, move towards the player
            if (distanceToPlayer > stoppingDistance)
            {
                _isJumping = false; // Player is out of stopping distance, reset jumping flag
                transform.Translate(moveDirection * (moveSpeed * Time.deltaTime));
            }
        }
    }

    void Jump()
    {
        _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        _isJumping = true;
        _hasJumped = true; // Set the flag to indicate that the jump has occurred
    }

    // Detect when hit by a slow bullet
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SlowBullet"))
        {
            // Adjust the moveSpeed here to slow down the enemy
            moveSpeed = 1f; // Adjust as needed
        }
    }
}
