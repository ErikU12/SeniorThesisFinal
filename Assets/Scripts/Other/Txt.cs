using UnityEngine;
using UnityEngine.UI;

public class Txt : MonoBehaviour
{
    public string textToShow = "Default Text"; // Text to display in the text box
    public GameObject canvasPrefab; // Prefab of the canvas to spawn
    public GameObject textBoxPrefab; // Prefab of the text box to spawn

    public Vector3 canvasPositionOffset = Vector3.zero; // Offset for canvas position
    public Vector3 textBoxPositionOffset = Vector3.zero; // Offset for text box position

    void Start()
    {
        // Spawn the canvas
        GameObject canvas = Instantiate(canvasPrefab, transform.position + canvasPositionOffset, Quaternion.identity);

        // Ensure the canvas is a child of the object this script is attached to
        canvas.transform.SetParent(transform, false);

        // Spawn the text box as a child of the canvas
        GameObject textBox = Instantiate(textBoxPrefab, canvas.transform.position + textBoxPositionOffset, Quaternion.identity);
        textBox.transform.SetParent(canvas.transform, false);

        // Set the text of the text box
        Text textComponent = textBox.GetComponentInChildren<Text>();
        if (textComponent != null)
        {
            textComponent.text = textToShow;
        }
    }
} 