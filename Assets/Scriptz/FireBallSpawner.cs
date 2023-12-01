using UnityEngine;

namespace Scenes.Scripts
{
    public class FireBallSpawner : MonoBehaviour
    {
        public GameObject fireballPrefab;           
        public float fireballSpeed = 10f;          
        public float fireballLifetime = 3f;         
        public string playerTag = "Player";         
        public float enemyRange = 5f;               
        public float spawnDelay = 0.5f;             
        private float _lastSpawnTime;               

        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Z) && Time.time >= _lastSpawnTime + spawnDelay)
            {
                SpawnFireball();                    
                _lastSpawnTime = Time.time;
            }
        }

        
        private void SpawnFireball()
        {
            GameObject fireball = Instantiate(fireballPrefab, transform.position, Quaternion.identity);  
            Rigidbody2D fireballRb = fireball.GetComponent<Rigidbody2D>();

            if (fireballRb != null)
            {
                
                float directionX = Input.GetAxisRaw("Horizontal");
                float directionY = Input.GetAxisRaw("Vertical");

                
                Vector2 direction = new Vector2(directionX, directionY).normalized;
                if (directionX != 0 || directionY != 0)
                {
                    fireball.transform.right = direction;
                }
                fireballRb.velocity = direction * fireballSpeed;

                
                GameObject player = GameObject.FindGameObjectWithTag(playerTag);
                if (player != null)
                {
                    Physics2D.IgnoreCollision(fireball.GetComponent<Collider2D>(), player.GetComponent<Collider2D>());
                }

             
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, enemyRange);
                foreach (Collider2D collider in colliders)
                {
                    if (collider.CompareTag("Enemy"))
                    {
                        Vector2 enemyDirection = (collider.transform.position - transform.position).normalized;
                        fireball.transform.right = enemyDirection;
                        fireballRb.velocity = enemyDirection * fireballSpeed;
                        break;
                    }
                }

                
                Destroy(fireball, fireballLifetime);
            }
        }
    }
}
