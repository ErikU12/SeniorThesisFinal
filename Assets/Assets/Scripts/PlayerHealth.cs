using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    [SerializeField]
    private int currentHealth;
    

    private Rigidbody2D _rb;
    private SpriteRenderer _playerRenderer;

    public Color damagedColor = new Color(1f, 0.5f, 0f); // Orange
    public Color criticalColor = Color.red;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerRenderer = GetComponent<SpriteRenderer>();

        currentHealth = maxHealth;
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
        Debug.Log("Collision detected");

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Enemy collision detected");
            TakeDamage(1);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdatePlayerColor();

        // Get the knockback direction (for example, away from the enemy)
        var transform1 = transform;
        var position = transform1.position;
        
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
            SetPlayerColor(Color.white); // Set the color back to normal or any other desired color
        }
    }

    private void SetPlayerColor(Color color)
    {
        _playerRenderer.material.color = color; // Set the color of the Renderer component
    }
}
