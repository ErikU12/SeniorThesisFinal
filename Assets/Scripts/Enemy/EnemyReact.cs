using UnityEngine;

public class EnemyReact : MonoBehaviour
{
    public float knockbackForce = 5.0f;
    public float flashDuration = 0.2f;
    public Color flashColor = Color.red;

    private SpriteRenderer _enemyRenderer;
    private Color _originalColor;

    private void Start()
    {
        _enemyRenderer = GetComponent<SpriteRenderer>();
        _originalColor = _enemyRenderer.color;
    }

    public void ReactToHit(Vector2 hitPosition)
    {
        // Bounce back the enemy
        Rigidbody2D enemyRb = GetComponent<Rigidbody2D>();
        if (enemyRb != null)
        {
            var position = transform.position;
            Vector2 knockbackDirection = (position - new Vector3(hitPosition.x, hitPosition.y, position.z)).normalized;
            enemyRb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        }

        // Flash the enemy with the specified color
        FlashEnemy();
    }

    private void FlashEnemy()
    {
        StartCoroutine(FlashRoutine());
    }

    private System.Collections.IEnumerator FlashRoutine()
    {
        // Flash the enemy with the specified color
        Color color = flashColor;
        _enemyRenderer.color = color;

        // Wait for a short duration
        yield return new WaitForSeconds(flashDuration);

        // Return the enemy to its original color
        color = _originalColor;
        _enemyRenderer.color = color;
    }
}
