using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScreenFade : MonoBehaviour
{
    public Image fadeImage; // Reference to the Image component used for fading
    public float fadeSpeed = 1.0f; // Speed of the fade animation
    public string sceneToLoad; // Scene to load after fade-out

    private bool isFading = false; // Flag to indicate if fading is in progress

    private void Update()
    {
        // If fading is in progress
        if (isFading)
        {
            // Calculate the new alpha value based on fade speed
            float newAlpha = fadeImage.color.a + fadeSpeed * Time.deltaTime;

            // Clamp alpha value between 0 and 1
            newAlpha = Mathf.Clamp01(newAlpha);

            // Update the color with the new alpha value
            fadeImage.color = new Color(0f, 0f, 0f, newAlpha);

            // If the fade-out is complete (alpha reaches 1), stop fading
            if (newAlpha >= 1.0f)
            {
                isFading = false;
                LoadScene(sceneToLoad);
            }
        }
    }

    public void StartFadeOutOnObeliskCollision()
    {
        isFading = true;
        fadeImage.gameObject.SetActive(true); // Ensure the fade image is active
        fadeImage.color = Color.clear; // Set the initial color to transparent
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obelisk"))
        {
            StartFadeOutOnObeliskCollision();
            // You can add more logic here if needed
        }
    }

    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}