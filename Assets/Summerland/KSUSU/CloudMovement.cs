using UnityEngine;
using System.Collections;

public class CloudMovement : MonoBehaviour
{
    public float rotationSpeed = 10f;        // Speed of rotation around the Y-axis
    public float moveRange = 5f;             // Max distance the clouds move up/down
    public float moveInterval = 2f;          // Time it takes to complete a full movement cycle
    public float moveSpeedVariation = 0.5f;  // Variation in speed for each cloud

    public bool alwaysFaceX = false;        // Boolean to control if clouds should always face X-axis

    private Transform[] cloudChildren;      // Cached references to child clouds

    void Start()
    {
        // Cache all the cloud children for efficient access
        cloudChildren = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            cloudChildren[i] = transform.GetChild(i);
        }

        // Start the cloud movement coroutine for each cloud
        foreach (var cloud in cloudChildren)
        {
            StartCoroutine(MoveCloudUpDown(cloud));
        }
    }

    void Update()
    {
        // Rotate the parent object around the Y-axis
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);

        // If alwaysFaceX is true, ensure that each cloud always faces the X direction
        if (alwaysFaceX)
        {
            foreach (var cloud in cloudChildren)
            {
                Vector3 lookDirection = new Vector3(1f, 0f, 0f); // The X direction
                cloud.LookAt(cloud.position + lookDirection);
            }
        }
    }

    // Coroutine to move a cloud up and down using sinusoidal movement
    private IEnumerator MoveCloudUpDown(Transform cloud)
    {
        // Initial Y position
        float initialYPosition = cloud.localPosition.y;

        // Randomize movement speed for this cloud (clouds move at different rates)
        float moveSpeed = Random.Range(1f, moveSpeedVariation + 1f);

        // Loop forever, moving the cloud up and down
        while (true)
        {
            // Create a smooth sinusoidal (sine wave) oscillation between -moveRange and +moveRange
            float timeElapsed = 0f;
            while (timeElapsed < moveInterval)
            {
                // Calculate sine-based movement
                float sineMovement = Mathf.Sin((timeElapsed / moveInterval) * Mathf.PI * 2) * moveRange;
                cloud.localPosition = new Vector3(cloud.localPosition.x, initialYPosition + sineMovement, cloud.localPosition.z);
                timeElapsed += Time.deltaTime * moveSpeed;  // Increase timeElapsed by the adjusted speed
                yield return null;
            }

            // Wait before starting the next move cycle
            yield return new WaitForSeconds(moveInterval);
        }
    }
}
