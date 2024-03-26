using UnityEngine;

public class AmmoAttraction : MonoBehaviour
{
    public string playerTag = "Player"; // Tag of the player object
    public string[] ammoTags; // Array of tags for different types of ammo
    public float attractionForce = 10f; // Force of attraction
    public float attractionDistance = 10f; // Maximum distance for attraction

    private GameObject player;

    private void Start()
    {
        // Find the player object based on the tag
        player = GameObject.FindGameObjectWithTag(playerTag);
        if (player == null)
        {
            Debug.LogError("Player object not found. Make sure it has the correct tag.");
        }
    }

    private void Update()
    {
        if (player != null)
        {
            // Iterate through each ammo tag
            foreach (string ammoTag in ammoTags)
            {
                // Find all ammo objects with the current tag
                GameObject[] ammoObjects = GameObject.FindGameObjectsWithTag(ammoTag);

                // Iterate through each ammo object
                foreach (GameObject ammoObject in ammoObjects)
                {
                    // Calculate the direction from the ammo to the player
                    Vector3 direction = player.transform.position - ammoObject.transform.position;

                    // Calculate the distance between the ammo and the player
                    float distance = direction.magnitude;

                    // If the distance is within the attraction range, apply attraction force
                    if (distance <= attractionDistance)
                    {
                        // Calculate attraction force based on distance
                        float attractionFactor = 1f - (distance / attractionDistance);
                        Vector3 attractionForceVector = direction.normalized * attractionForce * attractionFactor;

                        // Apply the attraction force to the ammo object
                        Rigidbody2D rb = ammoObject.GetComponent<Rigidbody2D>();
                        if (rb != null)
                        {
                            rb.AddForce(attractionForceVector);
                        }
                    }
                }
            }
        }
    }
}
