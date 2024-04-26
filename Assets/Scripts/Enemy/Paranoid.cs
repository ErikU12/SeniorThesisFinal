using UnityEngine;

public class Paranoid : MonoBehaviour
{
    public float originalMoveSpeed = 3f; // Store the original speed
    public float moveSpeed = 3f;
    public float detectionRange = 5f;
    public float chargingDistance = 2f; // The distance at which the soldier starts charging
    public float stoppingDistance = 1f;
    public float chargeForce = 10f; // Force for the charging movement
    public float patrolRange = 5f; // The range within which the enemy walks back and forth

    private Transform _player;
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;

    private bool _isCharging;
    private bool _isSlowed; // Flag to track if the soldier is slowed
    private bool _isPatrolling; // Flag to track if the soldier is patrolling
    private Vector3 _patrolStartPosition; // Store the initial patrol position
    private float _patrolDirection = 1f; // Store the current patrol direction (1 for right, -1 for left)
    private Color _originalColor; // Store the original color

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

        // Store the initial patrol position
        _patrolStartPosition = transform.position;

        // Start patrolling
        _isPatrolling = true;
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, _player.position);

        // Check if the player is in charging range and the soldier is not slowed
        if (distanceToPlayer <= chargingDistance && !_isSlowed)
        {
            Charge();
        }
        // Check if the player is in range
        else if (distanceToPlayer <= detectionRange)
        {
            Vector2 moveDirection = (_player.position - transform.position).normalized;

            if (distanceToPlayer > stoppingDistance && !_isCharging)
            {
                Move(moveDirection);
            }
        }
        // Check if the enemy is patrolling
        else if (_isPatrolling)
        {
            Patrol();
        }
    }

    void Move(Vector2 direction)
    {
        _isCharging = false;
        _rb.velocity = direction * moveSpeed;
        FlipSprite(direction.x < 0);
    }

    void Charge()
    {
        Vector2 chargeDirection = (_player.position - transform.position).normalized;
        Move(chargeDirection);
    }

    void Patrol()
    {
        // Move back and forth within the patrol range
        float patrolDistance = Mathf.PingPong(Time.time * moveSpeed, patrolRange);
        transform.position = _patrolStartPosition + new Vector3(patrolDistance * _patrolDirection, 0f, 0f);

        // Flip the sprite based on the patrol direction
        FlipSprite(patrolDistance < patrolRange / 2f);
    }

    // Detect when hit by a slow bullet
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SlowBullet") && !_isSlowed)
        {
            // Change the color of the soldier to blue
            _spriteRenderer.color = Color.blue;

            // Slow down the soldier
            moveSpeed = 1f;

            _isSlowed = true; // Set the flag to indicate that the soldier is slowed

            // Invoke a method to reset the color and speed after a delay
            Invoke("ResetSoldierState", 5f); // Adjust the delay as needed (5 seconds in this case)
        }
    }

    private void ResetSoldierState()
    {
        // Change the color of the soldier back to normal
        _spriteRenderer.color = _originalColor;

        // Reset the soldier speed to normal
        moveSpeed = originalMoveSpeed;

        _isSlowed = false; // Reset the flag when the soldier is no longer slowed
    }

    private void FlipSprite(bool faceRight)
    {
        _spriteRenderer.flipX = !faceRight;
    }
}



