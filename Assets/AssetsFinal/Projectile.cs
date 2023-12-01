using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float projectileSpeed = 5.0f;
    private Transform _playerTransform;

    private void Start()
    {
        // Find the player's transform using their tag ("Player")
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        // Rotate the projectile to face the player
        Vector3 direction = (_playerTransform.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void Update()
    {
        // Move the projectile forward
        transform.Translate(Vector2.right * (projectileSpeed * Time.deltaTime));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Handle collision with the player (reduce player health by 1)
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1);
            }

            // Destroy the projectile upon hitting the player
            Destroy(gameObject);
        }
    }
}