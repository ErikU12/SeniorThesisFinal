using TMPro;
using UnityEngine;

namespace Assets4.Scripts
{
    public class PlayerMaster  : MonoBehaviour
    {
        public float moveSpeed = 5f; // Player movement speed
        public GameObject bulletPrefab; // The bullet GameObject to spawn
        public Transform bulletSpawnPoint; // The point from which bullets are spawned
        public float bulletSpeed = 10f; // The speed of the bullets
        public int maxBullets = 10; // Maximum number of bullets you can carry
        public int bulletsInInventory; // Current bullets in the inventory
        public string ammoTag = "Ammo"; // Tag for ammo refill objects
        public int ammoRefillAmount = 5; // Amount of bullets to refill on collision with ammo
        public TextMeshProUGUI bulletCountText; // Reference to the TextMeshPro Text element to display bullet count
        public float rotationOffset = 0f; // Rotation offset to customize the rotation when the direction changes

        private Vector2 _moveDirection; // Direction of player movement

        private void Start()
        {
            bulletsInInventory = maxBullets; // Initialize with the maximum number of bullets
            UpdateBulletCountText(); // Update the bullet count UI
        }

        private void Update()
        {
            // Get input for player movement
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            // Calculate the movement direction based on input
            _moveDirection = new Vector2(horizontalInput, verticalInput).normalized;

            // Update player rotation based on the movement direction
            if (_moveDirection != Vector2.zero)
            {
                float angle = Mathf.Atan2(_moveDirection.y, _moveDirection.x) * Mathf.Rad2Deg + rotationOffset;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                // Adjust the bulletSpawnPoint rotation to match the player's rotation
                bulletSpawnPoint.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }

            // Check if the spacebar is pressed and you have bullets in the inventory
            if (Input.GetKeyDown(KeyCode.Space) && bulletsInInventory > 0)
            {
                // Spawn a bullet in front of the player
                SpawnBullet();
                bulletsInInventory--;
                UpdateBulletCountText(); // Update the bullet count UI
            }
        }

        void SpawnBullet()
        {
            if (bulletPrefab != null && bulletSpawnPoint != null)
            {
                var position = bulletSpawnPoint.position;
                GameObject bullet = Instantiate(bulletPrefab, position, bulletSpawnPoint.rotation);

                Vector3 spawnOffset = bulletSpawnPoint.right * 0.5f;
                Vector3 spawnPosition = position + spawnOffset;

                bullet.transform.position = spawnPosition;

                Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

                bulletRb.velocity = bulletSpawnPoint.up * bulletSpeed;
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

        private void FixedUpdate()
        {
            GetComponent<Rigidbody2D>().velocity = _moveDirection * moveSpeed;
        }
    }
}
