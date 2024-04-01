using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float sprintSpeed = 10f; // Speed when sprinting
    public float jumpForce = 7f;
    public float minJumpForce = 2f; // Minimum jump force for short jumps
    public float maxJumpTime = 0.2f; // Maximum time the jump button can be held for maximum jump height
    public float jumpCooldown = 1.0f; // Cooldown duration for jumping
    public float climbSpeed = 3f; // Speed of climbing

    private bool _isJumping;
    private bool _isGrounded; // Track whether the player is currently on the ground
    private bool _isClimbing; // Track whether the player is currently climbing
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
        float speed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed; // Use sprint speed if Shift is held down
        Vector2 horizontalMovement = new Vector2(moveX * speed, _rb.velocity.y);

        // Vertical movement (climbing)
        float moveY = Input.GetAxis("Vertical");
        Vector2 verticalMovement = new Vector2(0f, moveY * climbSpeed);

        Vector2 movement;

        // Update movement based on climbing state
        if (_isClimbing)
        {
            // Allow only vertical movement while climbing
            movement = verticalMovement;
        }
        else
        {
            // Allow horizontal movement if not climbing
            movement = horizontalMovement;
        }

        _rb.velocity = movement;
        _animator.SetFloat(PlayerRunning1, Mathf.Abs(movement.x));

        // Update jump cooldown timer
        if (_jumpCooldownTimer > 0)
        {
            _jumpCooldownTimer -= Time.deltaTime;
        }

        // Jumping
        if ((_isGrounded || _isClimbing) && !_isJumping && _jumpCooldownTimer <= 0)
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
        if (collision.gameObject.CompareTag("Ground") && !_isClimbing)
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

    private void OnTriggerStay2D(Collider2D other)
    {
        // Check if colliding with a ladder
        if (other.CompareTag("Ladder"))
        {
            // Start climbing if the player presses the up or down arrow key
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) ||
                Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                _isClimbing = true;
                _isJumping = false; // Disable jumping while climbing
                _rb.gravityScale = 0f; // Disable gravity while climbing
            }
            // Stop climbing if the player releases the up or down arrow key
            if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W) ||
                Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S))
            {
                _isClimbing = false;
                _rb.gravityScale = 1f; // Restore gravity after climbing
            }
        }
    }
}


