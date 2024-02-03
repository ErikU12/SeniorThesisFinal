using UnityEngine;

public class DropableItem : MonoBehaviour
{
    public GameObject itemPrefab; // The item to be dropped
    [Range(0, 100)]
    public float dropChance = 50f; // Drop chance percentage

    void OnDestroy()
    {
        // Check if the drop chance is successful
        if (Random.Range(0f, 100f) <= dropChance)
        {
            DropItem();
        }
    }

    void DropItem()
    {
        // Instantiate the item at the enemy's position
        if (itemPrefab != null)
        {
            Instantiate(itemPrefab, transform.position, Quaternion.identity);
        }
    }
}