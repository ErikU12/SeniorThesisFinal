using UnityEngine;

public class PlayerKnockback : MonoBehaviour
{
    public float knockbackForce = 10f;

    private Rigidbody2D _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("JumpPad"))
        {
            ApplyKnockback(collision.transform.position);
        }
    }

    public void ApplyKnockback(Vector3 enemyPosition)
    {
        // Get the knockback direction (for example, away from the enemy)
        var transform1 = transform;
        var position = transform1.position;
        Vector2 knockbackDirection = (position - new Vector3(enemyPosition.x, enemyPosition.y, position.z)).normalized;

        // Apply knockback force
        _rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
    }
}