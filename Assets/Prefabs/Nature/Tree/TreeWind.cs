using UnityEngine;

public class TreeWind : MonoBehaviour
{
    public Transform[] rootBones;  // Bones for the root (bottom of the tree)
    public Transform[] trunkBones; // Bones for the trunk (middle part of the tree)
    public Transform[] leafBones;  // Bones for the leaves (top of the tree)

    public float rootWindStrength = 0.1f;  // Wind strength for the root
    public float trunkWindStrength = 0.5f; // Wind strength for the trunk
    public float leafWindStrength = 1.0f;  // Wind strength for the leaves
    public float windFrequency = 1.0f;     // How quickly the wind oscillates
    public Vector3 windDirection = new Vector3(1, 0, 0); // Wind direction (local space)

    private Quaternion[] originalRootRotations;
    private Quaternion[] originalTrunkRotations;
    private Quaternion[] originalLeafRotations;

    void Start()
    {
        // Store the initial rotations for each group of bones
        originalRootRotations = new Quaternion[rootBones.Length];
        originalTrunkRotations = new Quaternion[trunkBones.Length];
        originalLeafRotations = new Quaternion[leafBones.Length];

        for (int i = 0; i < rootBones.Length; i++)
            originalRootRotations[i] = rootBones[i].localRotation;

        for (int i = 0; i < trunkBones.Length; i++)
            originalTrunkRotations[i] = trunkBones[i].localRotation;

        for (int i = 0; i < leafBones.Length; i++)
            originalLeafRotations[i] = leafBones[i].localRotation;
    }

    void Update()
    {
        ApplyWindToBones();
    }

    void ApplyWindToBones()
    {
        ApplyWindToBoneGroup(rootBones, rootWindStrength);
        ApplyWindToBoneGroup(trunkBones, trunkWindStrength);
        ApplyWindToBoneGroup(leafBones, leafWindStrength);
    }

    void ApplyWindToBoneGroup(Transform[] boneGroup, float windStrength)
    {
        // Convert the local wind direction to world space
        Vector3 worldWindDirection = transform.TransformDirection(windDirection);

        for (int i = 0; i < boneGroup.Length; i++)
        {
            Transform bone = boneGroup[i];

            // Calculate a sine wave for smooth oscillation, with variation for each bone
            float windEffect = Mathf.Sin(Time.time * windFrequency + i) * windStrength;

            // Compute wind rotation using the sine effect and the wind direction (now in world space)
            Vector3 windRotation = worldWindDirection * windEffect;

            // Use quaternion to apply the wind rotation more smoothly (avoids gimbal lock)
            Quaternion windQuat = Quaternion.Euler(windRotation);

            // Apply the wind effect to the bone’s local rotation
            if (boneGroup == rootBones)
                bone.localRotation = originalRootRotations[i] * windQuat;
            else if (boneGroup == trunkBones)
                bone.localRotation = originalTrunkRotations[i] * windQuat;
            else if (boneGroup == leafBones)
                bone.localRotation = originalLeafRotations[i] * windQuat;
        }
    }

    // Optionally, you can expose methods to dynamically change wind properties
    public void SetWindStrength(float strength)
    {
        rootWindStrength = strength;
        trunkWindStrength = strength;
        leafWindStrength = strength;
    }

    public void SetWindDirection(Vector3 direction)
    {
        windDirection = direction.normalized;
    }
}
