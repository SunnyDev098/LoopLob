using UnityEngine;

public class station_scr : MonoBehaviour
{
    public float rotationSpeed = 1f; // Speed of Y-axis rotation in degrees per second

    void Update()
    {
        // Calculate the rotation step based on rotationSpeed and deltaTime
        float rotationStep = rotationSpeed * Time.deltaTime;

        // Apply the rotation around the Y-axis (up axis)
        transform.Rotate(0, rotationStep, 0, Space.Self);
    }
}
