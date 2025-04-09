using UnityEngine;

public class SliderDoor : MonoBehaviour
{
    public Transform door;            // Reference to the door (usually the object itself or child)
    public Vector3 openPosition;      // The position where the door will open to
    public Vector3 closedPosition;    // The starting position of the door
    public float speed = 3f;          // Speed of the sliding door
    public float activationDistance = 3f;  // The distance at which the door will open or close
    public KeyCode activationKey = KeyCode.E; // The key to press for activation

    private bool isPlayerNearby = false; // Whether the player is close enough to activate the door
    private bool isOpen = false;    // Track if the door is open or closed

    private void Start()
    {
        // Ensure the door starts in the closed position
        door.localPosition = closedPosition;
    }

    private void Update()
    {
        // Check if player is close enough to trigger door
        if (isPlayerNearby && Input.GetKeyDown(activationKey))
        {
            ToggleDoor();
        }

        // Move the door smoothly based on whether it's opening or closing
        if (isOpen)
        {
            door.localPosition = Vector3.Lerp(door.localPosition, openPosition, speed * Time.deltaTime);
        }
        else
        {
            door.localPosition = Vector3.Lerp(door.localPosition, closedPosition, speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Make sure to tag the player with "Player"
        {
            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }
    }

    private void ToggleDoor()
    {
        // Toggle the door's state between open and closed
        isOpen = !isOpen;
    }
}
