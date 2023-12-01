using TMPro;
using UnityEngine;

namespace Assets4.Scripts
{
    public class PlayerMaster : MonoBehaviour
    {
        public float moveSpeed = 5f;
        public GameObject bulletPrefab;
        public float bulletSpeed = 10f;
        public int maxBullets = 10;
        public int bulletsInInventory;
        public string ammoTag = "Ammo";
        public int ammoRefillAmount = 5;
        public TextMeshProUGUI bulletCountText;
        public float rotationOffset = 0f;

        private Vector2 _moveDirection;

        private void Start()
        {
            bulletsInInventory = maxBullets;
            UpdateBulletCountText();
        }

        private void Update()
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            _moveDirection = new Vector2(horizontalInput, verticalInput).normalized;

            if (_moveDirection != Vector2.zero)
            {
                float angle = Mathf.Atan2(_moveDirection.y, _moveDirection.x) * Mathf.Rad2Deg + rotationOffset;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }

            if (Input.GetKeyDown(KeyCode.Space) && bulletsInInventory > 0)
            {
                SpawnBullet();
                bulletsInInventory--;
                UpdateBulletCountText();
            }
        }

        void SpawnBullet()
        {
            if (bulletPrefab != null)
            {
                // Instantiate the bullet at the player's position
                GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);

                // Get the Rigidbody2D component of the bullet
                Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

                // Determine the initial direction based on the player's orientation
                Vector2 initialDirection = (GetComponent<SpriteRenderer>().flipX) ? Vector2.left : Vector2.right;

                // Set the velocity of the bullet based on the player's orientation
                bulletRb.velocity = initialDirection * bulletSpeed;

                // Change the bullet's direction based on player input
                float directionX = Input.GetAxisRaw("Horizontal");
                float directionY = Input.GetAxisRaw("Vertical");

                Vector2 direction = new Vector2(directionX, directionY).normalized;
                if (directionX != 0 || directionY != 0)
                {
                    bullet.transform.up = direction;
                }
            }
            else
            {
                Debug.LogWarning("Bullet prefab not assigned in the inspector.");
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


