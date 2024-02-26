using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public float minJumpForce = 2f; // Minimum jump force for short jumps
    public float maxJumpTime = 0.2f; // Maximum time the jump button can be held for maximum jump height
    public float jumpCooldown = 1.0f; // Cooldown duration for jumping

    private bool _isJumping;
    private bool _isGrounded; // Track whether the player is currently on the ground
    private float _jumpCooldownTimer; // Timer for jump cooldown
    private Rigidbody2D _rb;
    private Animator _animator;
    private static readonly int PlayerRunning1 = Animator.StringToHash("PlayerRunning1");
    private float _jumpStartTime;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _jumpCooldownTimer = 0f; // Initialize cooldown timer
    }

    private void Update()
    {
        // Horizontal movement
        float moveX = Input.GetAxis("Horizontal");
        Vector2 movement = new Vector2(moveX * moveSpeed, _rb.velocity.y);
        _rb.velocity = movement;
        _animator.SetFloat(PlayerRunning1, Mathf.Abs(movement.x));

        // Update jump cooldown timer
        if (_jumpCooldownTimer > 0)
        {
            _jumpCooldownTimer -= Time.deltaTime;
        }

        // Jumping
        if (_isGrounded && !_isJumping && _jumpCooldownTimer <= 0)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                _isJumping = true;
                _jumpStartTime = Time.time;
                _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
                _jumpCooldownTimer = jumpCooldown; // Reset cooldown timer
            }
        }

        // Short jump
        if ((Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W)) && _isJumping && _rb.velocity.y > minJumpForce)
        {
            _isJumping = false; // Reset jump state immediately upon releasing the jump button
            _rb.velocity = new Vector2(_rb.velocity.x, minJumpForce);
        }

        // Limit maximum jump height
        if (_isJumping && (Time.time - _jumpStartTime >= maxJumpTime))
        {
            _isJumping = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // Check if colliding with the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            // Loop through all contact points of the collision
            foreach (ContactPoint2D contact in collision.contacts)
            {
                // Check if the contact point is below the player
                if (contact.point.y < transform.position.y)
                {
                    _isGrounded = true;
                    _jumpCooldownTimer = 0f; // Reset jump cooldown timer
                    return; // Exit the loop once grounded contact is found
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Set the grounded flag to false when leaving the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGrounded = false;
        }
    }
}
