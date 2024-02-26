using UnityEngine;

public class FloatingPlatform : MonoBehaviour
{
    public float moveSpeed = 2f; // Speed of the platform
    public float moveDistance = 4f; // Total distance the platform moves vertically

    private Vector3 _startPosition;
    private Vector3 _endPosition;
    private Vector3 _moveDirection;

    void Start()
    {
        _startPosition = transform.position;
        _endPosition = _startPosition + Vector3.up * moveDistance;
        _moveDirection = Vector3.up;
    }

    void Update()
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
            _moveDirection = -_moveDirection;
        }
    }
}