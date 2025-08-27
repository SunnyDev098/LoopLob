using UnityEngine;
using System.Collections;
public class alien_wandering_scr : MonoBehaviour
{
    public Sprite[] sprites = new Sprite[3]; // Assign your 3 sprites in the Inspector
    private SpriteRenderer spriteRenderer;
    private WaitForSeconds delay = new WaitForSeconds(0.05f);

    [Header("Horizontal X Movement")]
    public float xRange = 3f;     // X movement distance (from center to each side)
    public float xDuration = 2f;  // Time for full left-right-left cycle

    [Header("Vertical Y Movement")]
    public float yRange = 2f;     // Y movement distance (from center to each end)
    public float yDuration = 2f;  // Time for full up-down-up cycle
    public AnimationCurve verticalCurve; // S-curve for smooth y movement

    private Vector3 startPosition;
    private float tX = 0f;
    private float tY = 0f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ball") && !game_manager_scr.no_death)
        {
            if (game_manager_scr.ball_attached)
            {
                return;
            }

            CycleSprites(other.gameObject);

        }


    }
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        startPosition = transform.position;

       
    }

    void Start()
    {
        startPosition = transform.position;

        startPosition = transform.position;

        // Default curve if none assigned: makes movement slow at ends, fast in middle (like sine function).
        if (verticalCurve == null || verticalCurve.length == 0)
            verticalCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    }

    void Update()
    {
        // --- X Axis: back and forth linear
        tX += Time.deltaTime / xDuration;
        float xPingPong = Mathf.PingPong(tX, 1f);
        float xCurved = verticalCurve.Evaluate(xPingPong);       // curve-shape on 0..1
                                                                 // 0..1..0
        float xOffset = Mathf.Lerp(-xRange, xRange, xCurved);  // -xRange .. +xRange

        // --- Y Axis: back and forth with curve
        tY += Time.deltaTime / yDuration;
        float yPingPong = Mathf.PingPong(tY, 1f);                // 0..1..0
        float yCurved = verticalCurve.Evaluate(yPingPong);       // curve-shape on 0..1
        float yOffset = Mathf.Lerp(-yRange, yRange, yCurved);    // -yRange..+yRange

        // Apply result
        transform.position = startPosition + new Vector3(xOffset, yOffset, 0);
    }

   

    public void CycleSprites(GameObject other)
    {
        StartCoroutine(CycleSpritesCoroutine(other));
    }

    private IEnumerator CycleSpritesCoroutine(GameObject other)
    {
       
            spriteRenderer.sprite = sprites[0];
            yield return delay;
            yield return delay;

            spriteRenderer.sprite = sprites[1];
            yield return delay;

            spriteRenderer.sprite = sprites[2];


        if (!other.GetComponent<ball_scr>().shielded)
        {
            other.GetComponent<ball_scr>().gege();

        }

        yield return delay;
            spriteRenderer.sprite = sprites[0];


    }
        // To stop the cycling, you'll need to call StopCoroutine()
    
}


