using UnityEngine;

public class Invisible : MonoBehaviour
{
    public float originalMoveSpeed = 3f;
    public float moveSpeed = 3f;
    public float detectionRange = 5f;
    public float stoppingDistance = 1f;
    public float patrolRange = 5f;

    private Transform _player;
    private SpriteRenderer _spriteRenderer;
    private bool _isVisible = false;
    private float _visibilityDuration = 2f;
    private float _visibilityTimer;

    private bool _isSlowed = false; // Added to keep track of whether the enemy is slowed down
    private Color _originalColor; // Added to store the original color of the sprite
    private float _originalMoveSpeed; // Added to store the original move speed

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;

        if (_player == null)
        {
            Debug.LogError("Player not found. Make sure the player has the 'Player' tag.");
        }

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originalColor = _spriteRenderer.color; // Store the original color
        _originalMoveSpeed = moveSpeed; // Store the original move speed

        _visibilityTimer = _visibilityDuration;
        MakeInvisible();
    }

    void Update()
    {
        if (!_isVisible)
        {
            // Count down the visibility timer
            _visibilityTimer -= Time.deltaTime;

            if (_visibilityTimer <= 0f)
            {
                // Make the enemy visible again
                MakeVisible();
            }
        }
        else
        {
            float distanceToPlayer = Vector2.Distance(transform.position, _player.position);

            // Check if the player is in range
            if (distanceToPlayer <= detectionRange)
            {
                Vector2 moveDirection = (_player.position - transform.position).normalized;

                if (distanceToPlayer > stoppingDistance)
                {
                    // Check if the enemy is slowed down
                    if (!_isSlowed)
                    {
                        transform.Translate(moveDirection * (moveSpeed * Time.deltaTime));
                    }
                    else
                    {
                        // If slowed down, move at a slower speed
                        transform.Translate(moveDirection * (moveSpeed * 0.5f * Time.deltaTime));
                    }
                    
                    _spriteRenderer.flipX = (moveDirection.x < 0);
                }
            }
            else
            {
                Patrol();
            }
        }
    }

    void Patrol()
    {
        // Implement your patrolling logic here
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Make the enemy visible when the player enters its trigger collider
            MakeVisible();
        }
        else if (other.CompareTag("SlowBullet") && !_isSlowed)
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

    void MakeVisible()
    {
        _isVisible = true;
        _spriteRenderer.enabled = true;

        // Reset the timer
        _visibilityTimer = _visibilityDuration;

        // Reset the color and speed
        ResetEnemyState();
    }

    void MakeInvisible()
    {
        _isVisible = false;
        _spriteRenderer.enabled = false;
    }

    private void ResetEnemyState()
    {
        // Change the color of the enemy back to normal
        _spriteRenderer.color = _originalColor;

        // Reset the enemy speed to normal
        moveSpeed = _originalMoveSpeed;

        _isSlowed = false; // Reset the flag when the enemy is no longer slowed
    }
}
