using UnityEngine;

public class LaserEnemy : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 5.0f;
    public float detectionRange = 5.0f; // Range within which the enemy detects the player.
    public float shootInterval = 2.0f;  // Time between shots

    private Transform _playerTransform;
    private float _lastShotTime;

    private void Start()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _lastShotTime = Time.time;
    }

    private void Update()
    {
        if (_playerTransform == null)
        {
            return; // Player not found
        }

        // Calculate the distance between the enemy and the player.
        float distanceToPlayer = Vector3.Distance(transform.position, _playerTransform.position);

        // Check if the player is within detection range.
        if (distanceToPlayer <= detectionRange)
        {
            // Check if it's time to shoot again.
            if (Time.time - _lastShotTime >= shootInterval)
            {
                Shoot();
                _lastShotTime = Time.time;
            }
        }
    }

    private void Shoot()
    {
        if (_playerTransform == null)
        {
            return; // Player not found
        }

        var transform1 = transform;
        var position = transform1.position;
        Vector3 direction = (_playerTransform.position - position).normalized;

        // Create a new projectile
        GameObject projectile = Instantiate(projectilePrefab, position, Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        // Set the projectile's velocity
        rb.velocity = direction * projectileSpeed;
    }
}