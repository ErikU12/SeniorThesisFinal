using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    public Color[] colors = { Color.red, Color.blue, Color.yellow, Color.green, Color.magenta, new Color(0.5f, 0f, 0.5f, 1f) }; // Predefined colors
    private int currentIndex = 0; // Index of the current color
    private Color originalColor; // Original color of the sprite

    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component
    private string initialSortingLayer; // Initial sorting layer name
    private int initialOrderInLayer; // Initial order in layer

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component not found on the GameObject.");
            enabled = false; // Disable this script to prevent further errors
            return;
        }

        // Store the original color of the sprite
        originalColor = spriteRenderer.color;

        initialSortingLayer = spriteRenderer.sortingLayerName; // Store initial sorting layer
        initialOrderInLayer = spriteRenderer.sortingOrder; // Store initial order in layer
    }

    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            ChangeToNextColor(); // Change to the next color when Z key is pressed
        }
    }

    void ChangeToNextColor()
    {
        // Ensure there are colors defined in the array
        if (colors == null || colors.Length == 0)
        {
            Debug.LogError("No colors defined in the 'colors' array.");
            return;
        }

        // If the current index exceeds the bounds of the array, reset it to 0
        if (currentIndex >= colors.Length)
        {
            currentIndex = 0;
        }

        // Get the next color from the array
        Color nextColor = colors[currentIndex];

        // Increment the current index
        currentIndex++;

        // Check if the next color is the original color
        if (currentIndex >= colors.Length && !nextColor.Equals(originalColor))
        {
            spriteRenderer.color = originalColor; // Revert to the original color
        }
        else
        {
            // Change the color of the sprite renderer
            spriteRenderer.color = nextColor;
        }

        // Reset sorting layer and order in layer
        spriteRenderer.sortingLayerName = initialSortingLayer;
        spriteRenderer.sortingOrder = initialOrderInLayer;
    }
}
