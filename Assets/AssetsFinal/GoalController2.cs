using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalController2 : MonoBehaviour
{
    public float range = 5f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, range);
            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                   
                    this.enabled = false;
                    return;
                }
            }

           
            SceneManager.LoadScene("WinScene");
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a wire sphere to visualize the range of the script
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
