using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResetGame : MonoBehaviour
{
    public string mainMenuSceneName = "MainScene"; // Change this to the name of your main menu scene.
    public float fadeDuration = 1.0f; // Duration of the fade-out effect
    public Image fadeImage; // Reference to the UI image used for fading

    private bool isFading = false; // Flag to prevent multiple fade-out calls

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
        if (!isFading)
        {
            isFading = true;
            StartCoroutine(FadeOutAndLoadMainMenu());
        }
    }

    private IEnumerator FadeOutAndLoadMainMenu()
    {
        float timer = 0f;
        Color originalColor = fadeImage.color;
        Color targetColor = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);

        while (timer < fadeDuration)
        {
            fadeImage.color = Color.Lerp(originalColor, targetColor, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        // Ensure the fade image is fully faded out
        fadeImage.color = targetColor;

        // Load the main menu scene
        SceneManager.LoadScene(mainMenuSceneName);
    }
}