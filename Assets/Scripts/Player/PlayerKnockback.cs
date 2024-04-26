using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float knockbackForce = 10f;
    public float knockbackDuration = 0.5f;
    public float invincibleDuration = 2f;

    private Rigidbody2D rb;
    private bool isInvincible = false;
    private float invincibleTimer = 0f;
    private bool isKnockedBack = false;
    private float knockbackTimer = 0f;
    private Vector2 knockbackDirection;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    private void Update()
    {

        if (isInvincible)
        {
            invincibleTimer += Time.deltaTime;

            if (invincibleTimer >= invincibleDuration)
            {
                isInvincible = false;
                invincibleTimer = 0f;
                GetComponent<SpriteRenderer>().enabled = true;
            }
            else
            {
                GetComponent<SpriteRenderer>().enabled = !GetComponent<SpriteRenderer>().enabled;
            }
        }


        if (isKnockedBack)
        {
            knockbackTimer += Time.deltaTime;

            if (knockbackTimer >= knockbackDuration)
            {
                isKnockedBack = false;
                knockbackTimer = 0f;
            }
            else
            {
                rb.velocity = knockbackDirection * knockbackForce;
            }
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Laser"))
        {
            if (!isInvincible)
            {
                isInvincible = true;
                knockbackDirection = (transform.position - collision.transform.position).normalized;
                isKnockedBack = true;
            }
        }
    }
}