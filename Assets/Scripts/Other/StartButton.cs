using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    public string sceneToLoadName; // The name of the scene to load
    public GameObject fadeImage; // Reference to the UI image for fading
    public float fadeOutDuration = 1.5f; // Duration of the fade out effect

    private Button _button;

    private void Start()
    {
        // Get the Button component attached to this GameObject
        _button = GetComponent<Button>();

        // Add a listener to the button's onClick event
        _button.onClick.AddListener(StartFadeOutAndLoadScene);
    }

    private void StartFadeOutAndLoadScene()
    {
        if (!string.IsNullOrEmpty(sceneToLoadName))
        {
            // Start the fade out process
            StartCoroutine(FadeOutAndLoadScene());
        }
        else
        {
            Debug.LogError("Scene to load name is not assigned!");
        }
    }

    IEnumerator FadeOutAndLoadScene()
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

            // Load the specified scene after fade out completes
            SceneManager.LoadScene(sceneToLoadName);
        }
        else
        {
            Debug.LogError("Fade image reference is not set!");
        }
    }
}