using UnityEngine;

public class ball_elastic_scr : MonoBehaviour
{

    private Transform start_drag_point;

    private bool is_anchored = true;
    public Transform anchorPoint; // Fixed anchor point for the rope
    public float maxLength = 5f;  // Max stretch length due to physics (gravity)
    public float maxDragLength = 10f; // Max stretch length during dragging
    public float tearThreshold = 8f; // Stretch limit after which the rope tears
    public float springForce = 20f; // Elastic pulling force
    public float damping = 5f; // Smooth damping for rope behavior
    public LineRenderer lineRenderer; // Optional: Visualize the rope

    private Rigidbody2D ballRigidbody2D;
    private bool isDragging = false; // Tracks whether the ball is being dragged
    private bool ropeTorn = false; // Tracks whether the rope has been torn
    private bool isAnchoredToNewPlanet = false; // Tracks whether ball is anchored

    private bool hasHitPlanetOnce = false; // Tracks if the ball hit its first planet
    private Transform currentPlanet; // Stores the current planet

    void Start()
    {
        // Get the Rigidbody2D component
        ballRigidbody2D = GetComponent<Rigidbody2D>();

        // Ensure there's a Rigidbody2D attached
        if (ballRigidbody2D == null)
        {
            Debug.LogError("No Rigidbody2D found on the Ball!");
        }

        // Configure LineRenderer (if used)
        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 2;
        }
    }

    void Update()
    {
        // Handle drag input (e.g., touch or mouse input)
        if (!ropeTorn && !isAnchoredToNewPlanet && Input.GetMouseButtonDown(0)) // Screen touch or mouse press
        {


            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Vector2.Distance(touchPosition, transform.position) < 1f) // Touch near the ball
            {
                isDragging = true;
                ballRigidbody2D.isKinematic = true; // Disable physics during dragging
            }
        }

        if (isDragging && Input.GetMouseButton(0)) // Holding the drag
        {
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 directionToAnchor = touchPosition - (Vector2)anchorPoint.position;

            // Limit drag position to maxDragLength
            if (directionToAnchor.magnitude > maxDragLength)
            {
                touchPosition = (Vector2)anchorPoint.position + directionToAnchor.normalized * maxDragLength;
            }

            transform.position = touchPosition; // Move the ball to touch position
        }

        if (Input.GetMouseButtonUp(0)) // Release the drag
        {
            if (isDragging)
            {
                isDragging = false;
                ballRigidbody2D.isKinematic = false; // Re-enable physics after dragging

                // Check if the rope tears
                Vector2 directionToAnchor = (Vector2)(anchorPoint.position - transform.position);
                float stretch = directionToAnchor.magnitude;

                if (stretch > tearThreshold)
                {
                    TearRope(directionToAnchor.normalized, stretch - tearThreshold); // Pass direction and extra stretch for force
                }
            }
        }

        // Update line renderer
        if (lineRenderer != null)
        {
            if (!ropeTorn && anchorPoint != null) // Make sure anchorPoint exists
            {


                lineRenderer.enabled = true;
                lineRenderer.SetPosition(0, anchorPoint.position);
                lineRenderer.SetPosition(1, transform.position);
            }
            else
            {
                lineRenderer.enabled = false; // Hide the rope after it tears or ball is anchored to new planet
            }
        }
    }

    void FixedUpdate()
    {
        if (!ropeTorn && !isDragging && !isAnchoredToNewPlanet) // Apply rope physics only when not dragging, rope is not torn, and not anchored to the new planet
        {
            // Calculate the direction and distance from the ball to the anchor point
            Vector2 directionToAnchor = anchorPoint.position - transform.position;
            float distance = directionToAnchor.magnitude;

            // If the rope stretches beyond the physics-based max length
            if (distance > maxLength)
            {
                // **Spring Force**
                Vector2 pullingForce = directionToAnchor.normalized * springForce * (distance - maxLength);
                ballRigidbody2D.AddForce(pullingForce);

                // **Damping Force**
                Vector2 velocityAlongRope = Vector2.Dot(ballRigidbody2D.linearVelocity, directionToAnchor.normalized) * directionToAnchor.normalized;
                Vector2 dampingForce = -velocityAlongRope * damping;
                ballRigidbody2D.AddForce(dampingForce);
            }
        }
        else if (isAnchoredToNewPlanet && currentPlanet != null) //Make sure planet exists
        {
            // Apply a strong force to keep the ball anchored to the planet
            Vector2 directionToPlanet = currentPlanet.position - transform.position; // Use currentPlanet here
            float distance = directionToPlanet.magnitude;
            float forceMultiplier = 50f; // Adjust this value to control the strength of the anchoring force

            Vector2 anchoringForce = directionToPlanet.normalized * forceMultiplier * distance;
            ballRigidbody2D.AddForce(anchoringForce);
        }
    }

    private void TearRope(Vector2 tearDirection, float extraStretch)
    {
        // Handle the logic for tearing the rope
        ropeTorn = true; // Mark the rope as torn

        // Apply a force to the ball in the direction of the tear
        Vector2 tearForce = tearDirection * (springForce * extraStretch); // Use extra stretch as multiplier for the force
        ballRigidbody2D.AddForce(tearForce, ForceMode2D.Impulse);

        Debug.Log("Rope Torn! Force applied: " + tearForce);
        is_anchored = false;


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Detect collision with a "planet" initially
        if (collision.collider.CompareTag("planet") && !is_anchored)
        {


            is_anchored = true;

            //  hasHitPlanetOnce = true; // This prevents detecting the same planet again

            // Set isAnchoredToNewPlanet to true
            isAnchoredToNewPlanet = true;

            // Get planet's transform and store it
            currentPlanet = collision.transform;

            // Make the anchor point the planet
            anchorPoint = currentPlanet;

            // After detecting collision, switch the planet's collider to a **trigger**
            Collider2D planetCollider = collision.collider;
            planetCollider.isTrigger = true;

            Debug.Log("Initial collision detected: Anchoring to planet.");

            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, anchorPoint.position);
            lineRenderer.SetPosition(1, transform.position);


            isAnchoredToNewPlanet = false;

            ropeTorn = false;

            Invoke("cam_nullifer", 1f);

        }
    }

}
