using UnityEngine;

public class ImmuneToDamage : MonoBehaviour
{
    // Define the layers that this hitbox should ignore collisions with
    public LayerMask ignoredLayers;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision is with an object on the ignored layers
        if (((1 << collision.gameObject.layer) & ignoredLayers) != 0)
        {
            // Ignore the collision
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        }
    }
}