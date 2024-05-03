using UnityEngine;

public class ScrollingText : MonoBehaviour
{
    public float scrollSpeed = 1.0f; // Speed at which the text scrolls

    void Update()
    {
        // Move the object upward
        transform.Translate(Vector3.up * scrollSpeed * Time.deltaTime);
    }
}