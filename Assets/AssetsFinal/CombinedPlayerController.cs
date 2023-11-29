using TMPro;
using UnityEngine;

public class Playermaster2 : MonoBehaviour
{
    public float moveSpeed = 5f;
    private float _originalMoveSpeed;
    public float jumpForce = 7f;
    private bool _isJumping = false;
    private Rigidbody2D _rb;
    public GameObject bulletPrefab; // The bullet GameObject to spawn
    public Transform bulletSpawnPoint; // The point from which bullets are spawned
    public float bulletSpeed = 10f; // The speed of the bullets
    public int maxBullets = 10; // Maximum number of bullets you can carry
    public int bulletsInInventory; // Current bullets in the inventory
    public string ammoTag = "Ammo"; // Tag for ammo refill objects
    public int ammoRefillAmount = 5; // Amount of bullets to refill on collision with ammo
    public TextMeshProUGUI bulletCountText; // Reference to the TextMeshPro Text element to display bullet count

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _originalMoveSpeed = moveSpeed; // Store the original move speed
        bulletsInInventory = maxBullets; // Initialize with the maximum number of bullets
        UpdateBulletCountText(); // Update the bullet count UI
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && bulletsInInventory > 0)
        {
            // Spawn a bullet in front of the player
            SpawnBullet();
            bulletsInInventory--;
            UpdateBulletCountText(); // Update the bullet count UI
        }
        // Horizontal movement
        float moveX = Input.GetAxis("Horizontal");
        Vector2 movement = new Vector2(moveX * moveSpeed, _rb.velocity.y);
        _rb.velocity = movement;

        // Jumping
        if (Input.GetKeyUp(KeyCode.UpArrow) && !_isJumping)
        {
            _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            _isJumping = true;
        }
    }

    void SpawnBullet()
    {
        if (bulletPrefab != null && bulletSpawnPoint != null)
        {
            // Calculate the spawn position in front of the player
            Vector3 spawnPosition = bulletSpawnPoint.position + bulletSpawnPoint.right * 0.5f;

            GameObject bullet = Instantiate(bulletPrefab, spawnPosition, bulletSpawnPoint.rotation);

            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

            bulletRb.velocity = bulletSpawnPoint.right * bulletSpeed; // Use right instead of up for the spawn direction
        }
        else
        {
            Debug.LogWarning("Bullet prefab or spawn point not assigned in the inspector.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(ammoTag))
        {
            bulletsInInventory = Mathf.Min(maxBullets, bulletsInInventory + ammoRefillAmount);
            Destroy(other.gameObject);
            UpdateBulletCountText();
        }
    }

    void UpdateBulletCountText()
    {
        if (bulletCountText != null)
        {
            bulletCountText.text = "Bullets: " + bulletsInInventory.ToString();
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

    // Function to remove power-up effects
    public void RemovePowerUp(float speedMultiplier)
    {
        // Revert the player's speed to the original value
        moveSpeed /= speedMultiplier;

        // Revert the player's color to normal
        GetComponent<SpriteRenderer>().color = Color.white; // You may need to adjust this to your player's original color.
    }
}
