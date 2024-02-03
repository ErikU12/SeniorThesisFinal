using TMPro;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;
    public float bulletLifetime = 3f;
    public string playerTag = "Player";
    public float enemyRange = 5f;
    public float spawnDelay = 0.5f;
    public Vector2 spawnOffset = Vector2.zero; // Add spawn offset
    private float _lastSpawnTime;
    public string ammoTag = "Ammo";
    public int ammoRefillAmount = 5;
    public TextMeshProUGUI bulletCountText;
    public int maxBullets = 10;
    private int _bulletsInInventory;
    public Animator animator;
    private static readonly int PlayerBowAction = Animator.StringToHash("PlayerBowAction");

    public AudioSource bulletSpawnSound; // Add this line
    // Assign your sound effect to this variable in the Unity Editor

    private void Start()
    {
        _bulletsInInventory = maxBullets; // Set initial bullets in the inventory
        UpdateBulletCountText();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= _lastSpawnTime + spawnDelay && _bulletsInInventory > 0)
        {
            SpawnBullet();
            _lastSpawnTime = Time.time;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(ammoTag))
        {
            _bulletsInInventory = Mathf.Min(maxBullets, _bulletsInInventory + ammoRefillAmount);
            Destroy(other.gameObject);
            UpdateBulletCountText();
        }
    }

    void UpdateBulletCountText()
    {
        if (bulletCountText != null)
        {
            bulletCountText.text = "Bullets: " + _bulletsInInventory.ToString();
        }
    }

    private void SpawnBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position + (Vector3)spawnOffset, Quaternion.identity);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

        if (bulletRb != null)
        {
            // Get the mouse position in the world space
            if (Camera.main != null)
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                // Set the bullet direction towards the mouse position
                Vector2 direction = (mousePosition - transform.position).normalized;
                bullet.transform.right = direction;
                bulletRb.velocity = direction * bulletSpeed;
            }

            GameObject player = GameObject.FindGameObjectWithTag(playerTag);
            if (player != null)
            {
                Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), player.GetComponent<Collider2D>());
            }

            _bulletsInInventory--; // Reduce the bullets in the inventory
            UpdateBulletCountText();

            // Trigger the animation when the bullet is spawned
            if (animator != null)
            {
                // Set the normalized time of the animation to 0
                animator.Play(PlayerBowAction, -1, 0f);
            }

            // Play the bullet spawn sound effect
            if (bulletSpawnSound != null)
            {
                bulletSpawnSound.Play();
            }

            Destroy(bullet, bulletLifetime);
        }
    }
}
