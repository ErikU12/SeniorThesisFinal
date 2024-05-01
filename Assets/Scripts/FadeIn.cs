using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SceneFadeIn : MonoBehaviour
{
    public float fadeInDuration = 1.5f; // Duration of the fade in effect
    public GameObject fadeImage; // Reference to the UI image for fading

    private void Start()
    {
        // Start the fade in process
        StartCoroutine(FadeIn());
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
}