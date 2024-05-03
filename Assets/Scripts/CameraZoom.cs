using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public Transform playerTransform; // Reference to the player's transform
    public Transform obeliskTransform; // Reference to the obelisk's transform
    public float minOrthographicSize = 5f; // Minimum orthographic size of the camera
    public float maxOrthographicSize = 10f; // Maximum orthographic size of the camera
    public float zoomSpeed = 5f; // Speed at which the camera zooms
    public float startZoomDistance = 10f; // Distance at which the zoom effect starts

    private Camera mainCamera; // Reference to the main camera

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // Calculate the distance between the player and the obelisk
        float distanceToObelisk = Vector3.Distance(playerTransform.position, obeliskTransform.position);

        // Check if the player is within the startZoomDistance
        if (distanceToObelisk <= startZoomDistance)
        {
            // Calculate the target orthographic size based on the distance
            float targetOrthographicSize = Mathf.Clamp(distanceToObelisk, minOrthographicSize, maxOrthographicSize);

            // Smoothly interpolate towards the target orthographic size
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, targetOrthographicSize, zoomSpeed * Time.deltaTime);
        }
    }
}
