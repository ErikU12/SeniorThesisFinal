using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneFadeButton : MonoBehaviour
{
    public float fadeInDuration = 1.5f; // Duration of the fade in effect
    public float fadeOutDuration = 1.5f; // Duration of the fade out effect
    public GameObject fadeImage; // Reference to the UI image for fading
    public SceneAsset sceneToLoad; // Reference to the scene to transition to

    private bool fadeOutStarted = false; // Flag to track if fade out has started

    private void Start()
    {
        // Start the fade in process
        StartCoroutine(FadeIn());
    }

    private void Update()
    {
        // Check if the fade out has started and fade out is not already in progress
        if (fadeOutStarted && !IsFadingOut())
        {
            // Load the next scene after fade out completes
            SceneManager.LoadScene(sceneToLoad.name);
        }
    }

    IEnumerator FadeIn()
    {
        if (fadeImage != null)
        {
            float fadeInTimer = 0f;
            Color initialColor = fadeImage.GetComponent<Image>().color;
            Color targetColor = new Color(initialColor.r, initialColor.g, initialColor.b, 0f); // Fade in from fully transparent

            while (fadeInTimer < fadeInDuration)
            {
                fadeInTimer += Time.deltaTime;
                float normalizedTime = Mathf.Clamp01(fadeInTimer / fadeInDuration);
                Color currentColor = Color.Lerp(initialColor, targetColor, normalizedTime);
                fadeImage.GetComponent<Image>().color = currentColor;
                yield return null;
            }
        }
    }

    public void StartFadeOut()
    {
        // Start the fade out process
        StartCoroutine(FadeOut());
        fadeOutStarted = true; // Set the flag to true indicating fade out has started
    }

    private bool IsFadingOut()
    {
        // Check if the fade image color alpha is not fully opaque
        return fadeImage != null && fadeImage.GetComponent<Image>().color.a < 1f;
    }

    IEnumerator FadeOut()
    {
        if (fadeImage != null)
        {
            float fadeOutTimer = 0f;
            Color initialColor = fadeImage.GetComponent<Image>().color;
            Color targetColor = new Color(initialColor.r, initialColor.g, initialColor.b, 1f); // Fade out to fully opaque

            while (fadeOutTimer < fadeOutDuration)
            {
                fadeOutTimer += Time.deltaTime;
                float normalizedTime = Mathf.Clamp01(fadeOutTimer / fadeOutDuration);
                Color currentColor = Color.Lerp(initialColor, targetColor, normalizedTime);
                fadeImage.GetComponent<Image>().color = currentColor;
                yield return null;
            }
        }
    }
}

