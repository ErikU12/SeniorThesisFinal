using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFade : MonoBehaviour
{
    public float fadeInDuration = 1.5f; // Duration of the fade in effect
    public float fadeOutDuration = 1.5f; // Duration of the fade out effect
    public float fadeOutDelay = 5f; // Delay before starting the fade out
    public GameObject fadeImage; // Reference to the UI image for fading
    public SceneAsset sceneToLoad; // Reference to the scene to transition to

    private void Start()
    {
        // Start the fade in process
        StartCoroutine(FadeIn());

        // Start the fade out process after the fade in and a delay
        StartCoroutine(DelayedFadeOut(fadeOutDelay));
    }

    IEnumerator FadeIn()
    {
        if (fadeImage != null)
        {
            float fadeInTimer = 0f;
            Color initialColor = fadeImage.GetComponent<UnityEngine.UI.Image>().color;
            Color targetColor = new Color(initialColor.r, initialColor.g, initialColor.b, 0f); // Fade in from fully transparent

            while (fadeInTimer < fadeInDuration)
            {
                fadeInTimer += Time.deltaTime;
                float normalizedTime = Mathf.Clamp01(fadeInTimer / fadeInDuration);
                Color currentColor = Color.Lerp(initialColor, targetColor, normalizedTime);
                fadeImage.GetComponent<UnityEngine.UI.Image>().color = currentColor;
                yield return null;
            }
        }
    }

    IEnumerator DelayedFadeOut(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Start the fade out process
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        if (fadeImage != null)
        {
            float fadeOutTimer = 0f;
            Color initialColor = fadeImage.GetComponent<UnityEngine.UI.Image>().color;
            Color targetColor = new Color(initialColor.r, initialColor.g, initialColor.b, 1f); // Fade out to fully opaque

            while (fadeOutTimer < fadeOutDuration)
            {
                fadeOutTimer += Time.deltaTime;
                float normalizedTime = Mathf.Clamp01(fadeOutTimer / fadeOutDuration);
                Color currentColor = Color.Lerp(initialColor, targetColor, normalizedTime);
                fadeImage.GetComponent<UnityEngine.UI.Image>().color = currentColor;
                yield return null;
            }

            // Load the next scene after fade out completes
            SceneManager.LoadScene(sceneToLoad.name);
        }
    }
}




