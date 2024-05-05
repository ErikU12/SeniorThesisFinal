using UnityEngine;

[System.Serializable]
public class DropTableItem
{
    public GameObject itemPrefab;
    [Range(0, 100)]
    public float dropChance = 50f;
}

public class DropableItem : MonoBehaviour
{
    public DropTableItem[] dropTable;

    void OnDestroy()
    {
        // Calculate the total drop chance
        float totalDropChance = 0f;
        foreach (DropTableItem item in dropTable)
        {
            totalDropChance += item.dropChance;
        }

        // Randomly select an item to drop
        float randomValue = Random.Range(0f, totalDropChance);
        float cumulativeChance = 0f;
        foreach (DropTableItem item in dropTable)
        {
            cumulativeChance += item.dropChance;
            if (randomValue <= cumulativeChance)
            {
                DropItem(item);
                break;
            }
        }
    }

    void DropItem(DropTableItem item)
    {
        // Instantiate the selected item at the enemy's position
        if (item.itemPrefab != null)
        {
            GameObject droppedItem = Instantiate(item.itemPrefab, transform.position, Quaternion.identity);
            // Detach dropped item from the enemy object
            droppedItem.transform.parent = null;
            // Ignore collision with enemy
            Physics2D.IgnoreCollision(droppedItem.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
    }
}