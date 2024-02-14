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
        // Check current health and set player color accordingly
        if (currentHealth == 2)
        {
            SetPlayerColor(damagedColor); // Change to orange when health is 2
        }
        else if (currentHealth == 1)
        {
            SetPlayerColor(criticalColor); // Change to red when health is 1
        }
        else
        {
            SetPlayerColor(Color.white); // Set the color back to normal for other cases
        }
    }





    private void SetPlayerColor(Color color)
    {
        _playerRenderer.material.color = color; // Set the color of the Renderer component
    }
}
