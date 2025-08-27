using UnityEngine;

[RequireComponent(typeof(Collider2D))] // Ensure there's a collider
public class DirectionCurveTrigger : MonoBehaviour
{
    


    private float enter_time;
    [Header("Attraction Settings")]
    [Tooltip("How strongly the direction curves toward center")]
    public float attractionStrength = 5f;

    [Tooltip("Maximum distance for attraction effect")]
    public float maxAttractionDistance = 5f;

    [Tooltip("Distance where attraction is strongest")]
    public float minAttractionDistance = 0.5f;

    [Tooltip("Controls how quickly attraction increases as ball approaches center")]
    public AnimationCurve attractionCurve = new AnimationCurve(
        new Keyframe(0f, 0f),
        new Keyframe(1f, 1f)
    );

    [Header("Debug")]
    public bool showDebugGizmos = true;


    private void Start()
    {
        GetComponent<AudioSource>().volume =  game_manager_scr.sfx_volume * 3;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("ball")) return;
        enter_time = Time.time;

    }
    private void OnTriggerStay2D(Collider2D other)
    {
        // Only affect objects tagged as "Ball"
        if (!other.CompareTag("ball")) return;

        if (enter_time + 0.5f < Time.time) return;
        // Get the ball's movement controller
        ball_scr ballMovement = other.GetComponent<ball_scr>();
        if (ballMovement == null) return;

        // Calculate vector from ball to our center
        Vector2 toCenter = (Vector2)transform.position - (Vector2)other.transform.position;
        float distance = toCenter.magnitude;

        // Only curve direction if within max distance
        if (distance <= maxAttractionDistance)
        {
            // Calculate normalized distance (0 at max distance, 1 at min distance)
            float normalizedDistance = 1 - Mathf.Clamp01(
                (distance - minAttractionDistance) /
                (maxAttractionDistance - minAttractionDistance)
            );

            // Get curve multiplier from animation curve
            float curveMultiplier = attractionCurve.Evaluate(normalizedDistance);

            // Calculate attraction factor with curve applied
            float attractionFactor = curveMultiplier * 0.7f * attractionStrength * Time.deltaTime;

            // Normalize the direction to center
            Vector2 centerDirection = toCenter.normalized;

            // Get current movement direction from the ball
            Vector2 currentDirection = ballMovement.move_direction;

            // Gradually curve the move direction toward center with variable rate
            Vector2 newDirection = Vector2.Lerp(
                currentDirection.normalized,
                centerDirection,
                attractionFactor
            ).normalized * currentDirection.magnitude;

            // Apply the modified direction back to the ball
            ballMovement.move_direction = newDirection;
        }
    }

    private void OnDrawGizmos()
    {
        if (!showDebugGizmos) return;

        // Draw attraction radius
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, maxAttractionDistance);

        // Draw minimum distance
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, minAttractionDistance);

        // Draw sample attraction strengths
        if (Application.isPlaying)
        {
            Gizmos.color = Color.green;
            for (float r = minAttractionDistance; r <= maxAttractionDistance; r += 0.5f)
            {
                float normalizedDist = 1 - Mathf.Clamp01(
                    (r - minAttractionDistance) /
                    (maxAttractionDistance - minAttractionDistance)
                );
                float strength = attractionCurve.Evaluate(normalizedDist) * attractionStrength;
                Vector3 pos = transform.position + Vector3.right * r;
                Gizmos.DrawLine(pos, pos + Vector3.up * strength * 0.1f);
            }
        }
    }
}