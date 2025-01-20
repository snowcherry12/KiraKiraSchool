using UnityEngine;

public class AutoSlideDoor : MonoBehaviour
{
    public Transform leftDoor;             // Left door object
    public Transform rightDoor;            // Right door object
    public Vector3 openLeftPosition;       // Position where the left door will slide to when open
    public Vector3 closedLeftPosition;     // Left door starting position (closed)
    public Vector3 openRightPosition;      // Position where the right door will slide to when open
    public Vector3 closedRightPosition;    // Right door starting position (closed)

    public float speed = 3f;               // Speed of the door movement
    public float closeDelay = 3f;          // Time delay before the door closes after player leaves

    private bool isPlayerNearby = false;   // Track if player is nearby to trigger the door
    private bool isOpen = false;           // Is the door open or closed?
    private float closeTimer = 0f;         // Timer to handle automatic door close

    private void Start()
    {
        // Ensure both doors start in the closed position
        leftDoor.localPosition = closedLeftPosition;
        rightDoor.localPosition = closedRightPosition;
    }

    private void Update()
    {
        // Open or close the door based on player proximity
        if (isPlayerNearby)
        {
            if (!isOpen)
            {
                OpenDoor();
            }
        }
        else
        {
            closeTimer += Time.deltaTime;
            if (closeTimer >= closeDelay && isOpen)
            {
                CloseDoor();
            }
        }

        // Smoothly move the doors
        if (isOpen)
        {
            leftDoor.localPosition = Vector3.Lerp(leftDoor.localPosition, openLeftPosition, speed * Time.deltaTime);
            rightDoor.localPosition = Vector3.Lerp(rightDoor.localPosition, openRightPosition, speed * Time.deltaTime);
        }
        else
        {
            leftDoor.localPosition = Vector3.Lerp(leftDoor.localPosition, closedLeftPosition, speed * Time.deltaTime);
            rightDoor.localPosition = Vector3.Lerp(rightDoor.localPosition, closedRightPosition, speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  // Check if the player is nearby
        {
            isPlayerNearby = true;
            closeTimer = 0f;  // Reset the timer when the player comes near
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }
    }

    private void OpenDoor()
    {
        isOpen = true;  // Set the door to open
    }

    private void CloseDoor()
    {
        isOpen = false; // Set the door to close
    }
}
