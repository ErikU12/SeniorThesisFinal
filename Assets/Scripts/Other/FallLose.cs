using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLose : MonoBehaviour
{
    // The name of the scene to load when colliding with the "Finish" tag.
    public string gameWinSceneName = "GameWinScene";

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision is with an object tagged as "Finish."
        if (collision.gameObject.CompareTag("Player"))
        {
            // Load the "Game Win" scene.
            SceneManager.LoadScene(gameWinSceneName);
        }
    }
}