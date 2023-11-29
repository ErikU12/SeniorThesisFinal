using UnityEngine;

public class Knockback : MonoBehaviour
{
    public float knockbackForce = 10f; // Adjust this to set the force of the knockback.
    public float knockbackDuration = 0.2f; // Adjust this to set how long the knockback lasts.
    
    private Rigidbody2D rb;
    private float knockbackTimer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Check if the knockback duration has passed.
        if (knockbackTimer > 0)
        {
            knockbackTimer -= Time.deltaTime;
        }
        else
        {
            // Reset velocity after knockback duration has passed.
            rb.velocity = Vector2.zero;
        }
    }

    // Call this function to apply knockback.
    public void ApplyKnockback(Vector2 direction)
    {
        // Ensure knockback doesn't accumulate while the character is still being knocked back.
        if (knockbackTimer <= 0)
        {
            // Apply the knockback force.
            rb.velocity = direction.normalized * knockbackForce;
            
            // Start the knockback timer.
            knockbackTimer = knockbackDuration;
        }
    }
}