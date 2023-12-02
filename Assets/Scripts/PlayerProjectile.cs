using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public float projectileSpeed = 5.0f;

    private void Update()
    {
        // Get the horizontal input axis
        float horizontalInput = Input.GetAxis("Horizontal");

        // Move the projectile based on the input
        transform.Translate(Vector2.right * (projectileSpeed * horizontalInput * Time.deltaTime));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collider has the "Enemy" tag
        if (collision.CompareTag("Enemy"))
        {
            // Destroy the projectile upon hitting an object with the "Enemy" tag
            Destroy(gameObject);
        }
    }
}