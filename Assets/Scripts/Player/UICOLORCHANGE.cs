using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    public Color[] colors = { Color.yellow, Color.green, Color.magenta, new Color(0.5f, 0f, 0.5f, 1f) }; // Predefined colors
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
        float scrollDelta = Input.GetAxis("Mouse ScrollWheel");
        if (scrollDelta != 0)
        {
            if (scrollDelta > 0)
            {
                ChangeToNextColor(); // Change to the next color when scrolling up
            }
            else
            {
                ChangeToPreviousColor(); // Change to the previous color when scrolling down
            }
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            ChangeToNextColor(); // Change color when Z key is pressed
        }
    }

    void ChangeToNextColor()
    {
        // Increment the current index
        currentIndex++;
        // If the current index exceeds the bounds of the array, reset it to 0 and apply the original color
        if (currentIndex >= colors.Length)
        {
            currentIndex = 0;
            ApplyOriginalColor();
        }
        else
        {
            ApplyColor();
        }
    }

    void ChangeToPreviousColor()
    {
        // Decrement the current index
        currentIndex--;
        // If the current index goes below 0, set it to the last index
        if (currentIndex < 0)
        {
            currentIndex = colors.Length - 1;
        }
        ApplyColor();
    }

    void ApplyColor()
    {
        // Get the color from the array based on the current index
        Color currentColor = colors[currentIndex];

        // Change the color of the sprite renderer
        spriteRenderer.color = currentColor;

        // Reset sorting layer and order in layer
        spriteRenderer.sortingLayerName = initialSortingLayer;
        spriteRenderer.sortingOrder = initialOrderInLayer;
    }

    void ApplyOriginalColor()
    {
        // Revert to the original color
        spriteRenderer.color = originalColor;

        // Reset sorting layer and order in layer
        spriteRenderer.sortingLayerName = initialSortingLayer;
        spriteRenderer.sortingOrder = initialOrderInLayer;
    }
}
