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

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;

        if (_player == null)
        {
            Debug.LogError("Player not found. Make sure the player has the 'Player' tag.");
        }

        _spriteRenderer = GetComponent<SpriteRenderer>();
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
                    transform.Translate(moveDirection * (moveSpeed * Time.deltaTime));
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
        if (other.CompareTag("SlowBullet"))
        {
            // Handle slow bullet collision
        }
        else if (other.CompareTag("Player"))
        {
            // Make the enemy visible on collision with the player
            MakeVisible();
        }
        else if (other.CompareTag("Enemy"))
        {
            // Handle collision with another enemy
        }
    }

    void MakeVisible()
    {
        _isVisible = true;
        _spriteRenderer.enabled = true;

        // Reset the timer
        _visibilityTimer = _visibilityDuration;
    }

    void MakeInvisible()
    {
        _isVisible = false;
        _spriteRenderer.enabled = false;
    }
}




