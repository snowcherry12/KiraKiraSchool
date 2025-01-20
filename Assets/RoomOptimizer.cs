using UnityEngine;

public class RoomOptimizer : MonoBehaviour
{
    public string targetTag = "Player";  // Tag to check (e.g., "Player")
    
    private void Start()
    {
        // Deactivate all relevant components in children and descendants at the start
        SetAllChildComponentsActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag)) // Check if the collider that entered has the target tag
        {
            SetAllChildComponentsActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(targetTag)) // Check if the collider that exited has the target tag
        {
            SetAllChildComponentsActive(false);
        }
    }

    // Recursive function to activate/deactivate all relevant components in all descendants
    private void SetAllChildComponentsActive(bool isActive)
    {
        // Iterate through all descendants of the parent (including child, grandchildren, etc.)
        foreach (Transform child in transform.GetComponentsInChildren<Transform>())
        {
            // Skip the parent itself
            if (child == transform) continue;

            // Activate/deactivate components based on their type
            Light lightComponent = child.GetComponent<Light>();
            MeshRenderer meshRenderer = child.GetComponent<MeshRenderer>();
            Collider colliderComponent = child.GetComponent<Collider>();

            if (lightComponent != null)
            {
                lightComponent.enabled = isActive;
            }

            /*if (meshRenderer != null)
            {
                meshRenderer.enabled = isActive;
            }*/

            if (colliderComponent != null)
            {
                colliderComponent.enabled = isActive;
            }

            // Add more components as needed (e.g., AudioSource, ParticleSystem, etc.)
        }
    }
}
