using UnityEngine;

public class Eye : MonoBehaviour
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
    private float _patrolDirectionChangeCooldown = 0f; // Cooldown timer for patrol direction change
    private float _chargeCooldown = 0f; // Cooldown timer for charging after player detection

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
        if (_isCharging)
        {
            // Continue charging behavior
            Charge();
        }
        else if (distanceToPlayer <= detectionRange)
        {
            if (_chargeCooldown <= 0f)
            {
                // Stop for one second
                StopForOneSecond();

                // Reset charge cooldown
                _chargeCooldown = 1f;
            }
            else
            {
                // Update charge cooldown
                _chargeCooldown -= Time.deltaTime;

                // Check if the cooldown is over to initiate charging
                if (_chargeCooldown <= 0f)
                {
                    InitiateCharge();
                }
            }
        }
        else if (_isPatrolling)
        {
            Patrol();
        }
    }

    void InitiateCharge()
    {
        // Set _isCharging to true to start charging behavior
        _isCharging = true;

        // Invoke a method to stop charging after a delay
        Invoke("StopCharging", 2f); // You may adjust the delay as needed
    }

    void StopForOneSecond()
    {
        // Stop the soldier for one second
        _isPatrolling = false;
        _rb.velocity = Vector2.zero;

        // Invoke a method to resume patrolling after one second
        Invoke("ResumePatrolling", 1f); // You may adjust the delay as needed
    }

    void ResumePatrolling()
    {
        // Resume patrolling behavior
        _isPatrolling = true;
    }

    void Charge()
    {
        Vector2 chargeDirection = (_player.position - transform.position).normalized;

        // Check if the chargeDirection has a non-zero x component
        if (Mathf.Abs(chargeDirection.x) > 0.1f)
        {
            _spriteRenderer.flipX = (chargeDirection.x < 0);
        }

        _rb.AddForce(chargeDirection * chargeForce, ForceMode2D.Impulse);
    }

    void Patrol()
    {
        // Calculate the desired position within the patrol range
        float patrolDistance = Mathf.PingPong(Time.time * moveSpeed, patrolRange);
        Vector3 targetPosition = _patrolStartPosition + new Vector3(patrolDistance * _patrolDirection, 0f, 0f);

        // Calculate the move direction
        Vector3 moveDirection = (targetPosition - transform.position).normalized;

        // Check if the distance to the target position is greater than a small threshold
        if (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            // Move towards the target position
            transform.Translate(moveDirection * (moveSpeed * Time.deltaTime));

            // Flip the sprite based on the move direction
            _spriteRenderer.flipX = (moveDirection.x < 0);
        }
        else if (_patrolDirectionChangeCooldown <= 0f)
        {
            // If the soldier is close to the target position and cooldown allows, reverse the patrol direction
            _patrolDirection *= -1;

            // Flip the sprite based on the new patrol direction
            _spriteRenderer.flipX = (_patrolDirection < 0);

            // Reset the cooldown
            _patrolDirectionChangeCooldown = 0.5f;
        }

        // Update the cooldown timer
        _patrolDirectionChangeCooldown -= Time.deltaTime;
    }

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
        else if (other.CompareTag("Enemy"))
        {
            // If colliding with an object tagged as "Enemy", reverse the patrol direction
            _patrolDirection *= -1;

            // Flip the sprite based on the new patrol direction
            _spriteRenderer.flipX = (_patrolDirection < 0);
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

    void OnDestroy()
    {
        // Ensure that if the soldier is destroyed, any pending Invoke calls are canceled
        CancelInvoke();
    }
}
