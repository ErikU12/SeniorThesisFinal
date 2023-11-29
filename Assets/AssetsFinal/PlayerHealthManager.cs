using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealthManager : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public int maxHP = 3;
    private int currentHP;
    public float knockbackForce = 10f;

    private Renderer playerRenderer;
    private Rigidbody2D rb;

    public Color normalColor = Color.blue;
    public Color damagedColor = new Color(1f, 0.5f, 0f); // Orange
    public Color criticalColor = Color.red;

    private void Start()
    {
        playerRenderer = GetComponent<Renderer>(); // Access the Renderer component of the GameObject
        rb = GetComponent<Rigidbody2D>(); // Access the Rigidbody2D component of the GameObject

        currentHealth = maxHealth;
        currentHP = maxHP;

        SetPlayerColor(normalColor);
    }

    private void Update()
    {
        if (currentHealth <= 0)
        {
            SceneManager.LoadScene("GameOverLoadScene");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage();
            Vector2 knockbackDirection = (transform.position - collision.transform.position).normalized;
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        }
    }

    private void TakeDamage()
    {
        currentHealth--;
        currentHP--;

        UpdatePlayerColor();
    }

    private void UpdatePlayerColor()
    {
        if (currentHealth <= maxHealth * 0.2f)
        {
            SetPlayerColor(criticalColor);
        }
        else if (currentHealth <= maxHealth * 0.5f)
        {
            SetPlayerColor(damagedColor);
        }
        else
        {
            SetPlayerColor(normalColor);
        }
    }

    private void SetPlayerColor(Color color)
    {
        playerRenderer.material.color = color; // Set the color of the Renderer component
    }
}