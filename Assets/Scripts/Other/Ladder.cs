using UnityEngine;

public class Ladder : MonoBehaviour
{
    public float climbSpeed = 3f; // Speed of climbing

    void OnTriggerStay2D(Collider2D other)
    {
        // Check if the object entering the ladder is the player
        if (other.CompareTag("Player"))
        {
            // Check for vertical input to determine climbing direction
            float verticalInput = Input.GetAxisRaw("Vertical");

            // Move the player up or down the ladder based on input
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(rb.velocity.x, verticalInput * climbSpeed);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Check if the object leaving the ladder is the player
        if (other.CompareTag("Player"))
        {
            // Reset the player's vertical velocity when leaving the ladder trigger area
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(rb.velocity.x, 0f);
        }
    }
}