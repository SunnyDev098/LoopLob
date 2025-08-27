using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class planet_attribute_scr : MonoBehaviour
{

    private float random_delay;
    private float elapsed_time =0f;
    private bool delay_done;

    public bool moving_planet;
    [Header("Horizontal X Movement")]
    public float xRange = 3f;

    float minxLimit;
    float maxxLimit;


    // X movement distance (from center to each side)
     float xDuration;  // Time for full left-right-left cycle
    public AnimationCurve verticalCurve; // S-curve for smooth y movement

    private Vector3 startPosition;

    private float tX = 0f;




    public bool is_bomber;











    public bool attached_planet;
    public float planet_life_time = 3.5f;
    public GameObject boom_go_vfx;
    [Header("Colors")]
    [SerializeField] private Color fullHealthColor = Color.blue;
    [SerializeField] private Color midHealthColor = Color.yellow;
    [SerializeField] private Color lowHealthColor = Color.red;


    public GameObject slider_image;
    public Image radialFillImage; 
    
    private float fillAmount = 1f;
  private float drainSpeed = 40f; // Health drain per second
   private float maxHealth = 100f;
    public float currentHealth = 100f;


    public GameObject activated_orbit;
    public bool is_activated_orbit;
    public bool IsMovingRight;
    public bool IsShaking;
    public bool Visited;
    private Vector3 orbit_normal_size;

    private Vector3 rightVector = new Vector3(0.003f, 0, 0);
    private Vector3 leftVector = new Vector3(-0.003f, 0, 0);
    public int movingScope = 2000;
    public int movingCursor = 1000;
    public float orbitRadius;

    void Start()
    {
        xDuration = Random.Range(1f, 2f); // 10% variation, for example


         minxLimit = Mathf.Max(-xRange, game_manager_scr.left_bar_x + transform.localScale.x+3 );
         maxxLimit = Mathf.Min(xRange, game_manager_scr.right_bar_x - transform.localScale.x -3);

        // Now lerp between these clamped limits


        startPosition = transform.position;

        // Default curve if none assigned: makes movement slow at ends, fast in middle (like sine function).
        if (verticalCurve == null || verticalCurve.length == 0)
            verticalCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);


        attached_planet = false;

        currentHealth = 1f;

        orbit_normal_size = activated_orbit.transform.localScale;
        is_activated_orbit = false;

        activated_orbit.SetActive(false);
        orbitRadius = (transform.localScale.x * 5f) / 2;
        IsMovingRight = Random.Range(1.1f, 3.1f) >= 2.05f; // Initialize movement direction
        IsShaking = Random.Range(1.1f, 2.1f) >= 1.7f; // Initialize shaking state
        Visited = false; // Initialize Visited property



        /*
        if (gameObject.CompareTag("bad_planet"))
        {
            activated_orbit.SetActive(true);


        }
        */
    }

    private void Update()
    {

     

       
            if (moving_planet)
            {
                // --- X Axis: back and forth linear
                tX += Time.deltaTime / xDuration;
                float xPingPong = Mathf.PingPong(tX, 1f);
                float xCurved = verticalCurve.Evaluate(xPingPong);       // curve-shape on 0..1
                                                                         // 0..1..0
                float xOffset = Mathf.Lerp(minxLimit, maxxLimit, xCurved);  // -xRange .. +xRange




                transform.position = startPosition + new Vector3(xOffset, 0, 0);



            }
        
       
    }

    void HandleShaking()
    {
        // Check if the planet is within the screen bounds
        if (Mathf.Abs(transform.position.x) + orbitRadius < Mathf.Abs(2.8f))
        {
            if (movingCursor < movingScope && movingCursor > 0)
            {
                MovePlanet();
            }
            else
            {
                ToggleMovementDirection();
            }
        }
        else
        {
            ToggleMovementDirection();
            if (transform.position.x + orbitRadius >= 2.8f)
                movingCursor = movingScope - 1;
            if (transform.position.x - orbitRadius <= -2.8f)
                movingCursor = 1;
            MovePlanet();
        }
        movingCursor += IsMovingRight ? 1 : -1;
    }

    void MovePlanet()
    {
        // Move the planet based on its current direction
        transform.position += (IsMovingRight ? rightVector : leftVector) * Time.deltaTime * 80;
    }

    // Method to toggle movement direction
    public void ToggleMovementDirection()
    {
        IsMovingRight = !IsMovingRight;
    }

    public void scale_call()
    {
        StartCoroutine(ScaleOverTime(0.2f));
    }

    IEnumerator ScaleOverTime( float duration)
    {
        Vector3 originalScale = Vector3.zero;
        float currentTime = 0f;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float progress = Mathf.Clamp01(currentTime / duration);

            // Optionally, you can use an easing function here instead of linear
            activated_orbit.transform.localScale = Vector3.Lerp(originalScale, orbit_normal_size, progress);

            yield return null; // Wait until next frame
        }

        // Ensure the final scale is exactly the target scale
        activated_orbit.transform.localScale = orbit_normal_size;
    }






    IEnumerator ReduceFillOverTime()
    {
        var elapsed_time = 0f;

        transform.GetChild(0).transform.GetChild(3).transform.GetChild(2).gameObject.SetActive(true);

        while (planet_life_time > elapsed_time)
        {
            elapsed_time = elapsed_time +  Time.deltaTime;

            currentHealth =  1 - (elapsed_time / planet_life_time);


           // Debug.Log(currentHealth);
            // Wait until next frame

            radialFillImage.GetComponent<Slider>().value = currentHealth;
            yield return null;


        }

        transform.GetChild(0).gameObject.SetActive(false);

        GameObject moob_go = Instantiate(boom_go_vfx, transform.position, Quaternion.identity);

        if (game_manager_scr.ball_attached && attached_planet)
        {
            yield return new WaitForSeconds(0.2f);
            game_manager_scr.IsGameOver = true;


        }

        Destroy(moob_go,10);
     
        Destroy(transform.gameObject,11);
        yield return null;
        Debug.Log("sssssss222");

    }



    // Call this to restart the reduction
    public void ResetAndReduceAgain()
    {
        slider_image.SetActive(true);

        StartCoroutine(ReduceFillOverTime());

    }
}