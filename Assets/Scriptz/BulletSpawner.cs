using UnityEngine;
using TMPro;
namespace Scriptz
{
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
        public int maxBullets = 10; // Maximum number of bullets you can carry
        public int bulletsInInventory; // Current bullets in the inventory
        private void Start()
        {
            UpdateBulletCountText();
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Z) && Time.time >= _lastSpawnTime + spawnDelay)
            {
                SpawnBullet();                    
                _lastSpawnTime = Time.time;
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
                foreach (Collider2D collider in colliders)
                {
                    if (collider.CompareTag("Enemy"))
                    {
                        Vector2 enemyDirection = (collider.transform.position - transform.position).normalized;
                        bullet.transform.right = enemyDirection;
                        bulletRb.velocity = enemyDirection * bulletSpeed;
                        break;
                    }
                }

                
                Destroy(bullet, bulletLifetime);
            }
        }
    }
}
