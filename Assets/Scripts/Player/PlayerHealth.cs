using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    [SerializeField] private int currentHealth;
    public SpriteRenderer playerSpriteRenderer; // Reference to the SpriteRenderer component
    public Sprite fullHealthSprite; // Sprite to use when health is full
    public Sprite damagedSprite; // Sprite to use when health is 2 or more
    public Sprite criticalSprite; // Sprite to use when health is 1
    public Sprite deadSprite; // Sprite to use when health is 0 (dead)

    private Rigidbody2D _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        UpdatePlayerSprite();
    }

    private void Update()
    {
        if (currentHealth <= 0)
        {
            SceneManager.LoadScene("GameOver");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(1);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdatePlayerSprite();
    }

    public void IncreaseHealth(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth; // Ensure health doesn't exceed maximum
        }
        UpdatePlayerSprite();
    }

    private void UpdatePlayerSprite()
    {
        // Check current health and set player sprite accordingly
        if (currentHealth == 1)
        {
            playerSpriteRenderer.sprite = criticalSprite; // Change to critical sprite when health is 1
        }
        else if (currentHealth == 2)
        {
            playerSpriteRenderer.sprite = damagedSprite; // Change to damaged sprite when health is 2 or less
        }
        else if (currentHealth == 0)
        {
            playerSpriteRenderer.sprite = deadSprite; // Change to dead sprite when health is 0
        }
        else
        {
            playerSpriteRenderer.sprite = fullHealthSprite; // Set the sprite to full health sprite for other cases
        }
    }
}
