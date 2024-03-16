using UnityEngine;

public class FloatingPlatform : MonoBehaviour
{
    public float moveSpeed = 2f; // Speed of the platform
    public float moveDistance = 4f; // Total distance the platform moves vertically

    private Vector3 _startPosition;
    private Vector3 _endPosition;
    private Vector3 _previousPosition;

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

        // Initialize previous position
        _previousPosition = transform.position;
    }

    private void Update()
    {
        float step = moveSpeed * Time.deltaTime;

        // Move the platform towards the end position
        transform.position = Vector3.MoveTowards(transform.position, _endPosition, step);

        // If the platform reaches the end position, change direction
        if (Vector3.Distance(transform.position, _endPosition) < 0.001f)
        {
            Vector3 temp = _startPosition;
            _startPosition = _endPosition;
            _endPosition = temp;
        }

        // Calculate the difference in position between current and previous frame
        Vector3 movementDelta = transform.position - _previousPosition;

        // Update the player position if it's on the platform
        foreach (Collider2D collider in Physics2D.OverlapBoxAll(_platformCollider.bounds.center, _platformCollider.bounds.size, 0f))
        {
            if (collider.CompareTag("Player"))
            {
                collider.transform.position += movementDelta;
            }
        }

        // Update previous position
        _previousPosition = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider belongs to the player
        if (other.CompareTag("Player"))
        {
            // Ignore collisions between the platform and the player
            Physics2D.IgnoreCollision(_platformCollider, other, true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the collider belongs to the player
        if (other.CompareTag("Player"))
        {
            // Re-enable collisions between the platform and the player
            Physics2D.IgnoreCollision(_platformCollider, other, false);
        }
    }
}

