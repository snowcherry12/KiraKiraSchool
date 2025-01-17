using UnityEngine;

public class CustomLOD : MonoBehaviour
{
    // Array of objects to control
    public GameObject[] targetObjects;

    // Distance threshold to activate/deactivate objects
    public float activationDistance = 20f;

    // Reference to the camera or any point in space from which the distance is measured
    public Transform referencePoint;

    private void Update()
    {
        // If no reference point is set, use the main camera as default
        if (referencePoint == null)
        {
            referencePoint = Camera.main?.transform;
        }

        if (referencePoint == null) return;

        // Iterate over all target objects and enable/disable them based on distance
        foreach (var targetObject in targetObjects)
        {
            if (targetObject != null)
            {
                // Calculate the distance to the reference point
                float distance = Vector3.Distance(targetObject.transform.position, referencePoint.position);

                // Activate the object if within distance, otherwise deactivate it
                if (distance <= activationDistance)
                {
                    if (!targetObject.activeInHierarchy)
                    {
                        targetObject.SetActive(true);
                    }
                }
                else
                {
                    if (targetObject.activeInHierarchy)
                    {
                        targetObject.SetActive(false);
                    }
                }
            }
        }
    }
}
