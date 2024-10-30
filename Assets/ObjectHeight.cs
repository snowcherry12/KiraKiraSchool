using UnityEngine;

public class ObjectHeight : MonoBehaviour
{
    void Start()
    {
        // Get the renderer component of the object
        Renderer renderer = GetComponent<Renderer>();
        
        if (renderer != null)
        {
            // Get the bounds of the object
            Bounds bounds = renderer.bounds;
            
            // Calculate the height
            float height = bounds.size.y;
            
            // Print the height in meters
            Debug.Log("Object Height: " + height + " meters");
        }
        else
        {
            Debug.LogError("Renderer not found on the object");
        }
    }
}
