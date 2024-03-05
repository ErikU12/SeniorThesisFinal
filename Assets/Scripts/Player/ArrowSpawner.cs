using TMPro;
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
    public TextMeshProUGUI bulletCountText;
    private int maxBullets = 10; // Changed maxBullets to private
    public Animator animator;
    private static readonly int PlayerBowAction = Animator.StringToHash("PlayerBowAction");

    public AudioSource bulletSpawnSound;

    private int currentArrowIndex = 0;

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

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
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
        if (bulletCountText != null)
        {
            bulletCountText.text = "Bullets: " + arrowTypes[currentArrowIndex].currentBullets.ToString();
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
}
