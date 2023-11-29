using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int startingHealth = 100; 
    private int _currentHealth; 
    private Renderer _playerRenderer; 

    public Color normalColor = Color.blue; 
    public Color damagedColor = Color.yellow; 
    public Color criticalColor = Color.red; 

    private void Start()
    {
        _currentHealth = startingHealth; 
        
        _playerRenderer = GetComponent<Renderer>();
        
        SetPlayerColor(normalColor);
    }

    private void Update()
    {
        if (_currentHealth <= 0)
        {
            SceneManager.LoadScene("GameOverLoadScene");
        }
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        
        UpdatePlayerColor();
    }

    private void UpdatePlayerColor()
    {
        if (_currentHealth <= startingHealth * 0.2f) 
        {
            SetPlayerColor(criticalColor);
        }
        else if (_currentHealth <= startingHealth * 0.5f) 
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
        if (_playerRenderer != null)
        {
            _playerRenderer.material.color = color;
        }
    }
}
