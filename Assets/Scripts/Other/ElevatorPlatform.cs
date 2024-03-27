using UnityEngine;

public class ElevatorPlatform : MonoBehaviour
{
    public float moveSpeed = 2f; // Speed of the platform
    public float moveDistance = 4f; // Total distance the platform moves vertically

    private Vector3 _startPosition;
    private Vector3 _endPosition;
    private bool _isMoving = false;

    private Collider2D _platformCollider; // Collider of the platform

    private void Start()
    {
        _startPosition = transform.position;
        _endPosition = _startPosition + Vector3.up * moveDistance;

        // Get the collider component of the platform
        _platformCollider = GetComponent<Collider2D>();
        if (_platformCollider == null)
        {
            Debug.LogError("Collider component not found on the platform.");
        }
    }

    private void Update()
    {
        if (_isMoving)
        {
            float step = moveSpeed * Time.deltaTime;

            // Move the platform towards the end position
            transform.position = Vector3.MoveTowards(transform.position, _endPosition, step);

            // If the platform reaches the end position, stop moving
            if (Vector3.Distance(transform.position, _endPosition) < 0.001f)
            {
                _isMoving = false;
                Debug.Log("Platform reached end position. Stopped moving.");
            }
        }
        else
        {
            // If the platform is not colliding with the player, return to the starting position
            if (!IsCollidingWithPlayer())
            {
                float step = moveSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, _startPosition, step);
            }
        }
    }

    private bool IsCollidingWithPlayer()
    {
        // Check if the platform is colliding with the player
        Collider2D[] colliders = Physics2D.OverlapBoxAll(_platformCollider.bounds.center, _platformCollider.bounds.size, 0f);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Check if the collider belongs to the player
        if (other.gameObject.CompareTag("Player"))
        {
            // Start moving the platform
            _isMoving = true;
            Debug.Log("Player collided with platform. Starting movement...");
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        // Check if the collider belongs to the player
        if (other.gameObject.CompareTag("Player"))
        {
            // Stop moving the platform
            _isMoving = false;
            Debug.Log("Player exited platform. Stopping movement.");
        }
    }
}
