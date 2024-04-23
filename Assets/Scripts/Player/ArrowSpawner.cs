
using UnityEngine;
using System.Collections.Generic;

public class ArrowSpawner : MonoBehaviour
{
    [System.Serializable]
    public class ArrowType
    {
        public GameObject arrowPrefab;
        public int initialBulletsHeld; // Added initial bullets held for each arrow type
        public int ammoRefillAmount;
        public string ammoTag;
        [HideInInspector] public int currentBullets; // Track current bullets for each arrow type
    }

    public List<ArrowType> arrowTypes;
    public float bulletSpeed = 10f;
    public float bulletLifetime = 3f;
    public string playerTag = "Player";
    public float enemyRange = 5f;
    public float spawnDelay = 0.5f;
    public Vector2 spawnOffset = Vector2.zero;
    private float _lastSpawnTime;
    private int maxBullets = 10; // Changed maxBullets to private
    public Animator animator;
    private static readonly int PlayerBowAction = Animator.StringToHash("PlayerBowAction");
    public AudioSource bulletSpawnSound;
    public int currentArrowIndex = 0;
    public SpriteRenderer playerSpriteRenderer; // Reference to the SpriteRenderer component
    public Sprite tenArrowSprite; // Sprite to use when health is full
    public Sprite nineArrowsprite;
    public Sprite eightArrowSprite; // Sprite to use when health is 2 or more
    public Sprite sevenArrowSprite; // Sprite to use when health is 1
    public Sprite sixArrowSprite; // Sprite to use when health is 0 (dead)
    public Sprite fiveArrowSprite;
    public Sprite fourArrowSprite;
    public Sprite threeArrowSprite;
    public Sprite twoArrowSprite;
    public Sprite oneArrowSprite;
    public Sprite zeroArrowSprite;
    private void Start()
    {
        InitializeInventory();
        UpdateBulletCountText();
    }

    private void InitializeInventory()
    {
        foreach (ArrowType arrowType in arrowTypes)
        {
            arrowType.currentBullets = arrowType.initialBulletsHeld; // Initialize bullets for each arrow type
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= _lastSpawnTime + spawnDelay && arrowTypes[currentArrowIndex].currentBullets > 0)
        {
            SpawnBullet();
            _lastSpawnTime = Time.time;
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0 || Input.GetKeyDown(KeyCode.Z))
        {
            CycleArrow();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            // Handle scrolling down (e.g., switch to previous arrow type)
            // Add your code here for what should happen when the scroll wheel is scrolled down
            CycleArrow();
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        foreach (ArrowType arrowType in arrowTypes)
        {
            if (other.CompareTag(arrowType.ammoTag))
            {
                arrowType.currentBullets = Mathf.Min(maxBullets, arrowType.currentBullets + arrowType.ammoRefillAmount);
                Destroy(other.gameObject);
                UpdateBulletCountText();
                return;
            }
        }
    }

    void UpdateBulletCountText()
    {
        {
            UpdatePlayerSprite(); // Call UpdatePlayerSprite whenever the bullet count changes
        }
    }


    private void SpawnBullet()
    {
        ArrowType currentArrow = arrowTypes[currentArrowIndex];
        GameObject bullet = Instantiate(currentArrow.arrowPrefab, transform.position + (Vector3)spawnOffset, Quaternion.identity);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

        if (bulletRb != null)
        {
            if (Camera.main != null)
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 direction = (mousePosition - transform.position).normalized;
                bullet.transform.right = direction;
                bulletRb.velocity = direction * bulletSpeed;
            }

            GameObject player = GameObject.FindGameObjectWithTag(playerTag);
            if (player != null)
            {
                Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), player.GetComponent<Collider2D>());
            }

            currentArrow.currentBullets--; // Reduce bullets for the current arrow type
            UpdateBulletCountText();

            if (animator != null)
            {
                animator.Play(PlayerBowAction, -1, 0f);
            }

            if (bulletSpawnSound != null)
            {
                bulletSpawnSound.Play();
            }

            Destroy(bullet, bulletLifetime);
        }
    }

    private void CycleArrow()
    {
        currentArrowIndex++;
        if (currentArrowIndex >= arrowTypes.Count)
        {
            currentArrowIndex = 0;
        }
        UpdateBulletCountText(); // Update bullet count text after cycling arrow types
    }
    // Check current health and set player sprite accordingly
        private void UpdatePlayerSprite()
        {
            // Check current number of arrows and set player sprite accordingly
            int currentBullets = arrowTypes[currentArrowIndex].currentBullets;
            switch (currentBullets)
            {
                case 10:
                    playerSpriteRenderer.sprite = tenArrowSprite;
                    break;
                case 9:
                    playerSpriteRenderer.sprite = nineArrowsprite;
                    break;
                case 8:
                    playerSpriteRenderer.sprite = eightArrowSprite;
                    break;
                case 7:
                    playerSpriteRenderer.sprite = sevenArrowSprite;
                    break;
                case 6:
                    playerSpriteRenderer.sprite = sixArrowSprite;
                    break;
                case 5:
                    playerSpriteRenderer.sprite = fiveArrowSprite;
                    break;
                case 4:
                    playerSpriteRenderer.sprite = fourArrowSprite;
                    break;
                case 3:
                    playerSpriteRenderer.sprite = threeArrowSprite;
                    break;
                case 2:
                    playerSpriteRenderer.sprite = twoArrowSprite;
                    break;
                case 1:
                    playerSpriteRenderer.sprite = oneArrowSprite;
                    break;
                case 0:
                default:
                    playerSpriteRenderer.sprite = zeroArrowSprite;
                    break;
            }
        }

}