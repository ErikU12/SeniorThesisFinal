using System.Collections;
using UnityEngine;

public class BossFinisher : MonoBehaviour
{
    public GameObject finisherPrefab; // Finisher prefab to spawn
    public Transform spawnPoint; // Spawn point for the finisher prefab
    public float shakeMagnitude = 0.2f; // Magnitude of shake effect

    private bool isFinisherActivated = false; // Flag to indicate if finisher is activated

    void Update()
    {
        // Check if boss health is 3 or less and finisher is not yet activated
        if (GetComponent<BossHealth>().currentHealth <= 3 && !isFinisherActivated)
        {
            // Perform finisher
            PerformFinisher();
        }
    }

    void PerformFinisher()
    {
        // Shake the boss
        StartCoroutine(ShakeBoss());

        // Spawn the finisher prefab at the spawn point
        if (finisherPrefab != null && spawnPoint != null)
        {
            Instantiate(finisherPrefab, spawnPoint.position, Quaternion.identity);
            isFinisherActivated = true; // Set finisher activation flag
        }
    }

    // Coroutine to shake the boss
    IEnumerator ShakeBoss()
    {
        Vector3 originalPosition = transform.position;

        // Shake effect loop
        while (isFinisherActivated)
        {
            float xOffset = Random.Range(-shakeMagnitude, shakeMagnitude);
            float yOffset = Random.Range(-shakeMagnitude, shakeMagnitude);

            transform.position = originalPosition + new Vector3(xOffset, yOffset, 0);

            yield return null;
        }

        // Reset boss position after finisher
        transform.position = originalPosition;
    }
}