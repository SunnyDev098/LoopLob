using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(LineRenderer))]
public class laser_scr : MonoBehaviour
{

    public GameObject ball;
    public GameObject attached_planet;
    public float maxDistance = 20f;           // Max laser length
    public LayerMask collisionLayers;         // Layers to detect collision with
    Vector2 laserDirection;
    private LineRenderer lineRenderer;
   public RaycastHit2D hit;
    Vector3 laserStart;
    public GameObject hitted_planet;

    void Start()
    {


        lineRenderer = GetComponent<LineRenderer>();

        // Optional: Set laser width and color here or in Inspector
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.02f;
        lineRenderer.positionCount = 2;
    }

    void Update()
    {
        laserStart = transform.position;

        laserDirection = ball.transform.position - attached_planet.transform.position;  // For 2D typically right is forward direction
        laserDirection = new Vector2(laserDirection.y, -laserDirection.x);
        hit = Physics2D.Raycast(laserStart, laserDirection, maxDistance, collisionLayers);

        if (hit.collider != null)
        {
            // Hit something, laser ends at hit point
            SetLaserPositions(laserStart, hit.point);

            if (hit.collider.gameObject.CompareTag("planet"))
            {
                hitted_planet = hit.collider.gameObject;
            }
        }
        else
        {
            // No hit, laser ends at max distance forward
            SetLaserPositions(laserStart, laserStart + (Vector3)(laserDirection * maxDistance));
            hitted_planet = null;

        }
    }

    void SetLaserPositions(Vector3 start, Vector3 end)
    {
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }
}
