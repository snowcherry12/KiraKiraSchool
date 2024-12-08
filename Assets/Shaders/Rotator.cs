using UnityEngine;

public class Rotator : MonoBehaviour
{
    // Public variables to adjust rotation speeds for each axis
    public float rotationSpeedX = 30f; // Speed around the X-axis in degrees per second
    public float rotationSpeedY = 30f; // Speed around the Y-axis in degrees per second
    public float rotationSpeedZ = 30f; // Speed around the Z-axis in degrees per second

    void Update()
    {
        // Rotate the object around each axis independently
        transform.Rotate(rotationSpeedX * Time.deltaTime, 
                         rotationSpeedY * Time.deltaTime, 
                         rotationSpeedZ * Time.deltaTime);
    }
}
