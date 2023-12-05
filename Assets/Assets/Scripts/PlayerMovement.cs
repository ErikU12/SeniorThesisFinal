using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private float _originalMoveSpeed;
    public float jumpForce = 7f;
    private bool _isJumping;
    private Rigidbody2D _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _originalMoveSpeed = moveSpeed; // Store the original move speed
    }

    private void Update()
    {
        // Horizontal movement
        float moveX = Input.GetAxis("Horizontal");
        Vector2 movement = new Vector2(moveX * moveSpeed, _rb.velocity.y);
        _rb.velocity = movement;

        // Jumping
        if (Input.GetKeyDown(KeyCode.UpArrow) && !_isJumping)
        {
            _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            _isJumping = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Reset jump when landing on the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isJumping = false;
        }
    }

    // Function to apply power-up effects
    public void ApplyPowerUp(float speedMultiplier)
    {
        moveSpeed = _originalMoveSpeed * speedMultiplier;
        GetComponent<SpriteRenderer>().color = Color.red;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void RemovePowerUp(float speedMultiplier)
    {
     
        // Revert the player's speed to the original value
        moveSpeed /= speedMultiplier;
        
        // Revert the player's color to normal
        GetComponent<SpriteRenderer>().color = Color.white; // You may need to adjust this to your player's original color.
    }
}