using UnityEngine;

public class Spawner : MonoBehaviour
{
    [System.Serializable]
    public class SpawnOption
    {
        public GameObject prefab; // The prefab to spawn
        [Range(0, 100)]
        public float spawnChance; // Spawn chance percentage
    }

    public SpawnOption[] spawnOptions; // Array of spawn options
    public float spawnRateInSeconds = 2.0f; // The time interval between spawns in seconds

    private float _nextSpawnTime;

    private void Update()
    {
        if (Time.time >= _nextSpawnTime)
        {
            // Calculate the total spawn chance
            float totalSpawnChance = 0f;
            foreach (var option in spawnOptions)
            {
                totalSpawnChance += option.spawnChance;
            }

            // Randomly select a spawn option based on their individual probabilities
            float randomValue = Random.Range(0f, totalSpawnChance);
            float cumulativeChance = 0f;
            GameObject selectedPrefab = null;

            foreach (var option in spawnOptions)
            {
                cumulativeChance += option.spawnChance;

                if (randomValue <= cumulativeChance)
                {
                    selectedPrefab = option.prefab;
                    break;
                }
            }

            // Instantiate the selected prefab at the spawner's position and rotation
            if (selectedPrefab != null)
            {
                var transform1 = transform;
                GameObject zombie = Instantiate(selectedPrefab, transform1.position, transform1.rotation);

                // Remove references to MoveX and MoveY
                Wolf wolf = zombie.GetComponent<Wolf>();
                if (wolf != null)
                {
                    // Adjust the rotation of the instantiated zombie based on its movement direction
                    Vector3 movementDirection = wolf.GetComponent<Rigidbody2D>().velocity.normalized;
                    if (movementDirection != Vector3.zero)
                    {
                        float angle = Mathf.Atan2(movementDirection.y, movementDirection.x) * Mathf.Rad2Deg;
                        zombie.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                    }
                }
            }

            // Calculate the next spawn time in seconds
            _nextSpawnTime = Time.time + spawnRateInSeconds;
        }
    }
}
