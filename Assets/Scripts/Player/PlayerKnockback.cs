using UnityEngine;

public class PlayerKnockback : MonoBehaviour
{
    public float knockbackForce = 10f;
    public float knockbackDuration = 0.5f;
    private Rigidbody2D _rb;
    private bool _isKnockedBack = false;
    private float _knockbackTimer = 0f;
    private Vector2 _knockbackDirection;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        // Debugging: Print initial mass, friction, and drag
        Debug.Log("Initial Mass: " + _rb.mass);
        Debug.Log("Initial Friction: " + _rb.drag);
        Debug.Log("Initial Linear Drag: " + _rb.drag);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            _knockbackDirection = (transform.position - collision.transform.position).normalized; 
            _isKnockedBack = true;
        }
    }

    private void Update()
    {
        if (_isKnockedBack)
        {
            _knockbackTimer += Time.deltaTime;

            if (_knockbackTimer >= knockbackDuration)
            {
                _isKnockedBack = false;
                _knockbackTimer = 0f;
                _rb.velocity = Vector2.zero; // Reset velocity after knockback duration
            }
            else
            {
                _rb.velocity = _knockbackDirection * knockbackForce;

                // Debugging: Print relevant values during the knockback
                Debug.Log("Knockback Direction: " + _knockbackDirection);
                Debug.Log("Current Velocity: " + _rb.velocity);
            }
        }
    }
}