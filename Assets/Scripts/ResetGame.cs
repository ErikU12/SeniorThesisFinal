using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetGame : MonoBehaviour
{
    public string mainMenuSceneName = "MainScene"; // Change this to the name of your main menu scene.

    private void Update()
    {
        // Check for the Space key press
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ResetToMainMenu();
        }
    }

    // You can call this method from a UI button click or any other trigger.
    public void ResetToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}