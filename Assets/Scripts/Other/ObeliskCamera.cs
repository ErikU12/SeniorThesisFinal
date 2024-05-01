using UnityEngine;

public class CameraZoomOut : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public float maxZoomDistance = 10f; // Maximum distance at which the camera zooms out
    public float maxFieldOfView = 1000f; // Maximum field of view when zoomed out

    private Camera cam; // Reference to the Camera component

    void Start()
    {
        // Get reference to the Camera component
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        if (player == null)
        {
            Debug.LogWarning("Player reference not set in CameraZoomOut script.");
            return;
        }

        // Calculate the distance between the camera and the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Calculate the desired field of view based on the distance
        float targetFieldOfView = Mathf.Lerp(cam.fieldOfView, maxFieldOfView, distanceToPlayer / maxZoomDistance);

        // Clamp the field of view to the specified range
        targetFieldOfView = Mathf.Clamp(targetFieldOfView, cam.fieldOfView, maxFieldOfView);

        // Apply the calculated field of view
        cam.fieldOfView = targetFieldOfView;
    }
}