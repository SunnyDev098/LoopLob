using UnityEngine;

public class BallEdgeBounceTransform : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float minBounceSpeed = 3f;
    private Vector2 moveDirection;

    private Camera mainCam;
    private float leftEdge, rightEdge;

    void Start()
    {
        mainCam = Camera.main;
        CalculateScreenEdges();

        // Initialize with random direction
        moveDirection = Random.insideUnitCircle.normalized;
    }

    void CalculateScreenEdges()
    {
        Vector3 bottomLeft = mainCam.ScreenToWorldPoint(new Vector3(0, 0, mainCam.nearClipPlane));
        Vector3 topRight = mainCam.ScreenToWorldPoint(new Vector3(Screen.width, 0, mainCam.nearClipPlane));

        leftEdge = bottomLeft.x + (transform.localScale.x / 2f);
        rightEdge = topRight.x - (transform.localScale.x / 2f);
    }

    void Update()
    {
        // Move the ball using Transform
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        Vector2 pos = transform.position;

        // Left edge collision
        if (pos.x <= leftEdge && moveDirection.x < 0)
        {
            moveDirection = Vector2.Reflect(moveDirection, Vector2.right);
            moveDirection = moveDirection.normalized * Mathf.Max(moveDirection.magnitude, minBounceSpeed);
            Debug.Log("Bounced LEFT (Transform)");
        }
        // Right edge collision
        else if (pos.x >= rightEdge && moveDirection.x > 0)
        {
            moveDirection = Vector2.Reflect(moveDirection, Vector2.left);
            moveDirection = moveDirection.normalized * Mathf.Max(moveDirection.magnitude, minBounceSpeed);
            Debug.Log("Bounced RIGHT (Transform)");
        }
    }

    // Optional: Visualize movement direction
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)moveDirection);
    }
}