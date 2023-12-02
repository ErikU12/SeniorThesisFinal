using UnityEngine;

public class Knockback : MonoBehaviour
{
    public float knockbackForce = 10f; // Adjust this to set the force of the knockback.
    public float knockbackDuration = 0.2f; // Adjust this to set how long the knockback lasts.
    
    private Rigidbody2D _rb;
    private float _knockbackTimer;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Check if the knockback duration has passed.
        if (_knockbackTimer > 0)
        {
            _knockbackTimer -= Time.deltaTime;
        }
        else
        {
            // Reset velocity after knockback duration has passed.
            _rb.velocity = Vector2.zero;
        }
    }

    // Call this function to apply knockback.
    public void ApplyKnockback(Vector2 direction)
    {
        // Ensure knockback doesn't accumulate while the character is still being knocked back.
        if (_knockbackTimer <= 0)
        {
            // Apply the knockback force.
            _rb.velocity = direction.normalized * knockbackForce;
            
            // Start the knockback timer.
            _knockbackTimer = knockbackDuration;
        }
    }
}