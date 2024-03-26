using UnityEngine;

public class CrossHair : MonoBehaviour
{
    [SerializeField] private float followSpeed = 5f; // Speed at which the object follows the mouse

    public Texture2D C;
    void Awake()
    {
        Cursor.SetCursor(C,new Vector2(0,0),CursorMode.Auto);
        
    }
    
    void Update()
    {
        // Get the mouse position in world coordinates
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f; // Ensure the z-coordinate is 0 to keep it in the 2D plane

        // Move the object towards the mouse position
        transform.position = Vector3.MoveTowards(transform.position, mousePosition, followSpeed * Time.deltaTime);
    }
}