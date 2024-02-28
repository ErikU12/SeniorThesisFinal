using System.Collections;
using UnityEngine;

public class SpriteFlash : MonoBehaviour
{
    public Color flashColor = Color.white; // Color to flash
    public float flashDuration = 1f; // Duration of each flash in seconds
    public float intervalBetweenFlashes = 1f; // Time between each flash in seconds

    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;

        // Start the flash loop
        StartCoroutine(FlashLoop());
    }

    IEnumerator FlashLoop()
    {
        while (true)
        {
            // Flash bright
            spriteRenderer.color = flashColor;
            yield return new WaitForSeconds(flashDuration);

            // Revert to original color
            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(intervalBetweenFlashes);
        }
    }
}