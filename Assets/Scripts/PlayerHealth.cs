using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    [SerializeField]
    private int currentHealth;

    public float knockbackForce = 10f;

    private Renderer _playerRenderer;
    private Rigidbody2D _rb;

    public Color damagedColor = new Color(1f, 0.5f, 0f); // Orange
    public Color criticalColor = Color.red;

    private void Start()
    {
        _playerRenderer = GetComponent<Renderer>(); // Access the Renderer component of the GameObject
        _rb = GetComponent<Rigidbody2D>(); // Access the Rigidbody2D component of the GameObject

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
            Vector2 knockbackDirection = (transform.position - collision.transform.position).normalized;
            _rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        }
    }

    public void TakeDamage(int i)
    {
        currentHealth--;

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
            SetPlayerColor(Color.white); // Set the color back to normal or any other desired color
        }
    }

    private void SetPlayerColor(Color color)
    {
        _playerRenderer.material.color = color; // Set the color of the Renderer component
    }
}