using UnityEngine;

public class EnemyEquipper : MonoBehaviour
{
    public GameObject equippedEnemy; // Reference to the equipped enemy GameObject

    private void Start()
    {
        // Ensure an enemy is provided
        if (equippedEnemy == null)
        {
            Debug.LogError("No enemy assigned to EnemyEquipper script on " + gameObject.name + ". Please assign an enemy GameObject to 'equippedEnemy' in the Inspector.");
            Destroy(gameObject); // Destroy this GameObject if no enemy is assigned
        }
    }

    private void Update()
    {
        // Check if the equipped enemy has been destroyed
        if (equippedEnemy == null)
        {
            // Destroy this GameObject if the equipped enemy is destroyed
            Destroy(gameObject);
        }
    }
}




