using UnityEngine;

public class Deer : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of movement
    public float pauseDuration = 1f; // Duration of pause before changing direction
    private bool movingRight = true; // Flag to track movement direction
    private bool isPaused = false; // Flag to track pause state
    private float pauseTimer = 0f; // Timer for pause duration
    private SpriteRenderer spriteRenderer; // Reference to the sprite renderer component

    void Start()
    {
        // Get the SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Start the initial pause
        isPaused = true;
        pauseTimer = pauseDuration;
    }

    void Update()
    {
        // Countdown pause timer
        if (isPaused)
        {
            pauseTimer -= Time.deltaTime;
            if (pauseTimer <= 0)
            {
                isPaused = false; // Resume movement
                movingRight = !movingRight; // Change direction
                FlipSprite(); // Flip the sprite when changing direction
                pauseTimer = pauseDuration; // Reset the pause timer
            }
        }

        // Move the object continuously
        if (!isPaused)
        {
            // Move right if currently moving right, else move left
            if (movingRight)
                transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
            else
                transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        }

        // Check if it's time to pause
        if (!isPaused)
        {
            pauseTimer -= Time.deltaTime;
            if (pauseTimer <= 0)
            {
                isPaused = true; // Pause movement
                pauseTimer = pauseDuration; // Set the timer for the pause duration
            }
        }
    }

    // Flip the sprite horizontally
    void FlipSprite()
    {
        // Flip the sprite renderer component
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }
}
