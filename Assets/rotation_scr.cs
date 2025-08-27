using UnityEngine;

public class cam_rotation_scr : MonoBehaviour
{
    public Camera this_Camera; // Reference to the camera
    public float rotationSpeed = 5f; // Speed of camera rotation
    public float lerpSpeed = 5f; // Speed of interpolation for smooth rotation

    private bool isPerspective = false; // Tracks the current projection mode
    private Quaternion targetRotation; // Target rotation for smooth interpolation
    private Vector2 touchStartPos; // Stores the starting position of a touch (for Android)
    private bool isAndroidPlatform; // Tracks if the platform is Android



    private bool is_second_cam_on;
    void Start()
    {
        is_second_cam_on = false;
       

        // Initialize target rotation
        targetRotation = this_Camera.transform.rotation;

        // Check if the platform is Android
        isAndroidPlatform = Application.platform == RuntimePlatform.Android;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {

            if (!is_second_cam_on)
            {
                is_second_cam_on = true;

                this_Camera.depth = 2;
            }
            else
            {
                is_second_cam_on = false;
                this_Camera.depth = 0;
                this_Camera.transform.rotation = this_Camera.transform.parent.rotation;


            }
        }

        // Toggle projection mode when a button (e.g., Space) is pressed (for testing in Editor)

        // Smoothly rotate the camera based on input (mouse or touch)
        if (isAndroidPlatform)
        {
            HandleTouchInput(); // Android touch input
        }
        else
        {

            if (is_second_cam_on)
            {
                HandleMouseInput(); // PC mouse input

            }

        }
    }

    

    void HandleMouseInput()
    {
        // Get mouse movement
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

        // Calculate target rotation based on mouse movement
        targetRotation *= Quaternion.Euler(-mouseY, mouseX, 0);

        // Ensure Z-axis rotation is always zero
        Vector3 eulerRotation = targetRotation.eulerAngles;
        eulerRotation.z = 0; // Clamp Z-axis rotation to zero
        targetRotation = Quaternion.Euler(eulerRotation);

        // Smoothly interpolate to the target rotation
        this_Camera.transform.rotation = Quaternion.Lerp(this_Camera.transform.rotation, targetRotation, lerpSpeed * Time.deltaTime);
    }

    void HandleTouchInput()
    {
        if (Input.touchCount > 0) // Check if there is at least one touch
        {
            Touch touch = Input.GetTouch(0); // Get the first touch

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    // Store the starting position of the touch
                    touchStartPos = touch.position;
                    break;

                case TouchPhase.Moved:
                    // Calculate the swipe delta
                    Vector2 touchDelta = touch.position - touchStartPos;

                    // Calculate rotation based on swipe delta
                    float rotationX = touchDelta.y * rotationSpeed * Time.deltaTime;
                    float rotationY = -touchDelta.x * rotationSpeed * Time.deltaTime;

                    // Update target rotation
                    targetRotation *= Quaternion.Euler(rotationX, rotationY, 0);

                    // Ensure Z-axis rotation is always zero
                    Vector3 eulerRotation = targetRotation.eulerAngles;
                    eulerRotation.z = 0; // Clamp Z-axis rotation to zero
                    targetRotation = Quaternion.Euler(eulerRotation);

                    // Update the touch start position for the next frame
                    touchStartPos = touch.position;
                    break;
            }
        }

        // Smoothly interpolate to the target rotation
        this_Camera.transform.rotation = Quaternion.Lerp(this_Camera.transform.rotation, targetRotation, lerpSpeed * Time.deltaTime);
    }
}