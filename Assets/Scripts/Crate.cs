using UnityEngine;

public class Crate : MonoBehaviour
{
    public Animator animator; // Reference to the Animator component for playing the destruction animation
    private static readonly int BoxBreak = Animator.StringToHash("BoxBreak");

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Play destruction animation if animator is assigned
            if (animator != null)
            {
                animator.SetTrigger(BoxBreak);
            }

            // Destroy the crate GameObject after a short delay
            Destroy(gameObject, 0.5f);
        }
    }
}