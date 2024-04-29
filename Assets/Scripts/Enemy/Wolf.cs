using UnityEngine;

using UnityEngine;

public class Wolf : MonoBehaviour
{
    public float originalMoveSpeed = 3f; // Store the original speed
    public float moveSpeed = 3f;
    public float detectionRange = 5f;
    public float attackRange = 2f;
    public float stoppingDistance = 1f;
    public float jumpForce = 5f;

    private Transform _player;
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    private Animator animator; // Reference to the Animator component

    private bool _isJumping;
    private bool _hasJumped;
    private bool _isSlowed; // Flag to track if the enemy is slowed
    private Color _originalColor; // Store the original color
    private static readonly int WolfJump = Animator.StringToHash("WolfJump");
    private static readonly int WolfRun = Animator.StringToHash("WolfRun");

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;

        if (_player == null)
        {
            Debug.LogError("Player not found. Make sure the player has the 'Player' tag.");
        }

        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originalColor = _spriteRenderer.color;

        // Get reference to the Animator component
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, _player.position);

        // Check if the player is in both detection and attack range, and the jump has not occurred
        if (distanceToPlayer <= detectionRange && distanceToPlayer <= attackRange && !_hasJumped)
        {
            Jump();
        }

        // Check if the player is in range
        if (distanceToPlayer <= detectionRange)
        {
            Vector2 moveDirection = (_player.position - transform.position).normalized;

            if (distanceToPlayer > stoppingDistance)
            {
                _isJumping = false;
                transform.Translate(moveDirection * (moveSpeed * Time.deltaTime));

                _spriteRenderer.flipX = (moveDirection.x < 0);
                
                // Set the "WolfRun" animation when the wolf is moving
                animator.SetBool(WolfRun, true);
            }
            else
            {
                // Stop the "WolfRun" animation when the wolf is not moving
                animator.SetBool(WolfRun, false);
            }
        }
    }

    void Jump()
    {
        _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        _isJumping = true;
        _hasJumped = true;

        // Trigger the "WolfJump" animation when the wolf jumps
        animator.SetTrigger(WolfJump);
    }

    // Detect when hit by a slow bullet
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SlowBullet") && !_isSlowed)
        {
            // Change the color of the enemy to blue
            _spriteRenderer.color = Color.blue;

            // Slow down the enemy
            moveSpeed = 1f;

            _isSlowed = true; // Set the flag to indicate that the enemy is slowed

            // Invoke a method to reset the color and speed after a delay
            Invoke("ResetEnemyState", 5f); // Adjust the delay as needed (5 seconds in this case)
        }
    }

    private void ResetEnemyState()
    {
        // Change the color of the enemy back to normal
        _spriteRenderer.color = _originalColor;

        // Reset the enemy speed to normal
        moveSpeed = originalMoveSpeed;

        _isSlowed = false; // Reset the flag when the enemy is no longer slowed
    }
}
