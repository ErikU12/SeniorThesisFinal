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
    private float _lastSpawnTime;
    public string ammoTag = "Ammo";
    public int ammoRefillAmount = 5;
    public TextMeshProUGUI bulletCountText;
    public int maxBullets = 10; 
    private int _bulletsInInventory;
    public Animator Animator;

    private void Start()
    {
        _bulletsInInventory = maxBullets; // Set initial bullets in the inventory
        UpdateBulletCountText();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= _lastSpawnTime + spawnDelay && _bulletsInInventory > 0)
        {
           // Animator.SetBool("PlayerBowAction", true);
            SpawnBullet();
            _lastSpawnTime = Time.time;
            
        }
        else
        {
           // Animator.SetBool("PlayerBowAction",false);
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
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

        if (bulletRb != null)
        {
            float directionX = Input.GetAxisRaw("Horizontal");
            float directionY = Input.GetAxisRaw("Vertical");

            Vector2 direction = new Vector2(directionX, directionY).normalized;
            if (directionX != 0 || directionY != 0)
            {
                bullet.transform.right = direction;
            }
            bulletRb.velocity = direction * bulletSpeed;

            GameObject player = GameObject.FindGameObjectWithTag(playerTag);
            if (player != null)
            {
                Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), player.GetComponent<Collider2D>());
            }

            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, enemyRange);
            foreach (Collider2D collider2D1 in colliders)
            {
                if (collider2D1.CompareTag("Enemy"))
                {
                    Vector2 enemyDirection = (collider2D1.transform.position - transform.position).normalized;
                    bullet.transform.right = enemyDirection;
                    bulletRb.velocity = enemyDirection * bulletSpeed;
                    break;
                }
            }

            _bulletsInInventory--; // Reduce the bullets in the inventory
            UpdateBulletCountText();
            Destroy(bullet, bulletLifetime);
        }
    }
}