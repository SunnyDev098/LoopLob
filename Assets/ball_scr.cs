using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Rendering;
using System.Linq;
using UnityEngine.Advertisements;

public class ball_scr : MonoBehaviour
{
    public GameObject bg_objects_parent;

    public GameObject bottom_canvas;


    public GameObject bottom_bar;


    private bool is_game_started;

    public GameObject power_up_stuff;

    public GameObject arrow;
    public GameObject crown;
    public GameObject final_score_txt;


    public GameObject fire_ball;
    public bool fire_ball_is_active;
    private float last_attach_time;
    private float last_leave_time;
    public GameObject score_object;

    private TextMeshProUGUI plus_txt;
    private int plus_number;
    private TextMeshProUGUI combo_text;
    private int combo_number;

    private int prev_score;

    private float last_tp_time;
    private int tp_counter;
    public SpriteRenderer[] spriteRenderers = new SpriteRenderer[2];
    private Coroutine[] returnCoroutines = new Coroutine[2];
    private float returnDuration = 0.25f;

    private float minAlpha = 0f;
    private float maxAlpha = 20f / 255f;
    private float minScaleX = 2f;
    private float maxScaleX = 3.5f;



    public GameObject teleport_vfx;

    public GameObject laser_go;
    public float doubleClickTimeLimit = 0.3f; // Maximum interval between clicks (seconds)
    private float lastClickTime = -1f;

    public bool shielded;

    public float ball_move_rate;
    public Transform left_vfx;
    public Transform right_vfx;
    public GameObject rotation_vfx;


    public GameObject right_circle;
    public GameObject left_circle;
    private SpriteRenderer right_circle_sp;
    private SpriteRenderer left_circle_sp;

    public GameObject energy_sphere;
    public GameObject red_energy;
    public GameObject sheild;
    public GameObject magnet;


    public Vector2 move_direction;
    private Vector2 og_move_direction;

    [SerializeField] private AnimationCurve decayCurve; // Set in Inspector!
    private bool super_charge_on = false;

    [SerializeField] private float doubleClickTimeThreshold = 0.3f;
    private int clickCount = 0;
    private int coin_number = 0;
    public float normal_or_super_charged_speed;
    public float initial_normal_or_super_charged_speed;

    public GameObject boom_go_vfx;
    public GameObject right_flash;
    public GameObject left_flash;
    private float current_energy_level;
    public float energy_usage_rate;
    public float max_energy_level;
    public Image energy_level_slider;
    public Image energy_level_slider_fill;

    private float _dragValue;
    public float max_drag_level;
    private float stacked_drag;
    private Vector2 _pressPosition;
    public float sensitivity = 0.001f;

    private float currentSmoothedTilt;
    public float smoothingFactor = 0.1f;
    private float initialTiltAngle;
    private Quaternion initialAttitude;
    public float maxSpeed = 10f;
    public float tiltSensitivity = 2f;

    public float cut_speed_index = 1f;
    public float swipe_str = 10f; // Your base horizontal speed

    private Vector2 initialMousePosition;

    private bool swipe_jump_enable;


    private int gravity_state = 0;
    [Header("Gyro Settings")]
    public float maxTiltAngle = 30f; // Maximum tilt degree for full speed
    public float maxMoveSpeed = 7f; // Max horizontal speed
    public float responseCurve = 2f; // Adjust sensitivity (2 = quadratic)

    private Gyroscope gyro;
    private bool gyroEnabled;
    private Quaternion gyroInitialRotation;
    private float horizontalSpeed;

    [SerializeField] private AnimationCurve scaleCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);
    private bool in_black_hole = false;
    public bool black_hole_shrink = false;
    private bool game_over;
    private bool second_jump_called;
    public AnimationCurve anim_curve;
    private int jump_count;
    private float last_edge_hit;
    public GameObject game_manager_go;
    public GameObject hit_vfx_go;
    public GameObject jump_one_vfx_go;
    public GameObject jump_two_vfx_go;
    private bool jump_done;
    public Vector3 hor_move_vector = new Vector3(1, 0, 0);
    private Camera mainCamera;
    private float screenLeftEdge;
    private float screenRightEdge;
    private Vector3 gravity_force;
    private Vector3 gravity_force_vector;
    public float gravity_force_rate;
    public float minBounceSpeed = 3f;
    public SpriteRenderer bg;
    private bool direction_is_left;
    public float min_move_speed = 3;
    public float indexed_move_speed = 7f;
    public float move_speed_reduction_index = 1;
    public float hor_move_speed_reduction_rate = 1;
    public float hor_move_speed;
    bool score_increasing;
    private int last_top_y;
    private int current_score;
    private int last_score;
    public TextMeshProUGUI score_txt;
    public TextMeshProUGUI coin_txt;
    public GameObject env_manager;
    public GameObject planetPrefab;
    public GameObject planet_manager;
    public GameObject first_planet;
    public float screen_x_edge;
    public bool is_anchord = false;
    public float minRotationSpeed = 30f;
    public float maxRotationSpeed = 100f;
    public float rotationAcceleration = 50f;
    public float moveSpeed = 5f;
    private float currentRotationSpeed;
    private bool isButtonPressed = false;
    private SpriteRenderer spriteRenderer;
    private Color startColor = Color.blue;
    private Color targetColor = Color.red;
    private Vector3 originalScale;
    private bool isScaling = false;
    public GameObject planet;
    public bool cutTheRope;
    public bool ballShot;
    public bool hitWithPlanet;
    private float totalRotation;
    private Vector2 ballDirection;
    public TMP_Text totalPointsText;
    public AudioClip bounce_effect;
    public AudioClip get_planet;
    public AudioClip planet_leave;
    public AudioClip fall_effect;
    public AudioClip normal_planet_sound;
    public AudioClip rotate_sound;
    public AudioClip edge_effect;
    public AudioClip batterY_refill_sound;
    public AudioClip coin_effect;
    public AudioClip hit1;
    public AudioClip hit2;
    public AudioClip hit3;
    public AudioClip hit4;
    public AudioClip hit5;
    public AudioClip hit6;
    private AudioSource audioSource;










    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("missile") & !game_manager_scr.no_death & !shielded)
        {

            if (game_manager_scr.ball_attached)
            {
                return;
            }
            gege();
        }


        if (other.CompareTag("beam_emitter") & !game_manager_scr.no_death & !shielded)
        {

            if (game_manager_scr.ball_attached)
            {
                return;
            }


            gege();
        }

        if (fire_ball_is_active)
        {
            GameObject moob_go = Instantiate(boom_go_vfx, other.gameObject.transform.position, Quaternion.identity);
            Destroy(moob_go, 0.8f);

            Destroy(other.gameObject);

        }

        if (other.CompareTag("black_hole"))
        {

            in_black_hole = true;

        }

        if (other.CompareTag("safe_zone"))
        {

            game_manager_scr.inside_danger_zone = false;
            mainCamera.GetComponent<cam_shake_scr>().gray_filter.SetActive(false);
            game_manager_scr.check_for_danger_zone = false;

        }

        if (other.CompareTag("red_zone"))
        {

            Time.timeScale = 1.4f;

        }

        if (other.CompareTag("blue_zone"))
        {

            Time.timeScale = 0.6f;

        }
        

        if (other.CompareTag("coin"))
        {
            if (game_manager_scr.is_magnet_active)
            {
                return;
            }
            // coin_number = coin_number + 1;

            //   coin_txt.text = coin_number.ToString();
            other.transform.GetChild(0).gameObject.SetActive(false);
            other.transform.GetChild(1).gameObject.SetActive(true);

            Destroy(other.gameObject,2);
            audioSource.PlayOneShot(coin_effect);
        }
        
        if (other.CompareTag("spike") & !game_manager_scr.no_death & !shielded)
        {

            gege();

        }
        if (other.CompareTag("laser") & !game_manager_scr.no_death & !shielded )
        {
            if (game_manager_scr.ball_attached)
            {
                return;
            }
            gege();

        }
        if (other.CompareTag("battery"))
        {

            red_energyff();
            energy_sphere.GetComponent<BurstColorTransition>().ResetColor();
            energy_sphere.GetComponent<BurstColorTransition>().StartTransition();


            // current_energy_level = max_energy_level;
            //energy_level_slider.GetComponent<Slider>().value =1;
            Destroy(other.gameObject);
            audioSource.PlayOneShot(batterY_refill_sound);


        }


        if (other.CompareTag("shield_power"))
        {

          

            game_manager_go.GetComponent<power_ups_ui_manager>().gem_increase();
            Destroy(other.gameObject);
            audioSource.PlayOneShot(batterY_refill_sound);


        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("red_zone"))
        {

            Time.timeScale = 1;

        }

        if (other.CompareTag("blue_zone"))
        {

            Time.timeScale = 1f;

        }

        if (other.CompareTag("black_hole"))
        {

            ball_move_rate = 1;
            in_black_hole = false;

            //   moveSpeed = moveSpeed * 1.8f;
            //  move_direction = og_move_direction;
        }
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("black_hole"))
        {
            ball_move_rate = ball_move_rate - Time.deltaTime * 0.25f;


         //   Debug.Log("ssss");
        }
    }


    void Start()
    {

        GetComponent<AudioSource>().volume =  game_manager_scr.sfx_volume *3;


        is_game_started = false;

        right_circle_sp = right_circle.GetComponent<SpriteRenderer>();
        left_circle_sp = left_circle.GetComponent<SpriteRenderer>();

        spriteRenderers.Append(left_circle_sp);
        spriteRenderers.Append(right_circle_sp);

        red_energy.gameObject.GetComponent<ParticleSystem>().Stop();
        red_energy.gameObject.GetComponent<ParticleSystem>().playbackSpeed = 0.25f;

        ball_move_rate = 1f;
        mainCamera = Camera.main;


        Vector3 viewportPositionr = new Vector3(1f, 0.17f, mainCamera.nearClipPlane + 3f);

        Vector3 worldPositionr = mainCamera.ViewportToWorldPoint(viewportPositionr);

        right_vfx.position = worldPositionr;




        Vector3 viewportPositionl = new Vector3(0f, 0.17f, mainCamera.nearClipPlane + 3f);

        Vector3 worldPositionl = mainCamera.ViewportToWorldPoint(viewportPositionl);

        left_vfx.position = worldPositionl;
        initial_normal_or_super_charged_speed = normal_or_super_charged_speed;
        current_energy_level = max_energy_level;



        swipe_jump_enable = false;
        // InitializeGyro();

        second_jump_called = false;
        game_over = false;
        audioSource = GetComponent<AudioSource>();
        jump_count = 0;
        jump_done = false;
        gravity_force_vector = new Vector3(0, -1, 0);
        gravity_force = Vector3.zero;
        CalculateScreenEdges();
        moveSpeed = indexed_move_speed;
        score_increasing = false;
        last_top_y = 0;
        game_manager_scr.cam_target = first_planet.gameObject;
        totalRotation = 0;
        originalScale = transform.localScale;
        currentRotationSpeed = minRotationSpeed;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void LateUpdate()
    {


        if (game_manager_scr.inside_danger_zone)
        {

        }
        else if (game_manager_scr.check_for_danger_zone)
        {
            if (transform.position.y > game_manager_scr.next_danger_zone_height)
            {
                if (game_manager_scr.next_danger_zone_height > 0)
                {

                    game_manager_scr.inside_danger_zone = true;
                    mainCamera.GetComponent<cam_shake_scr>().alarm_caller();
                }

            }
        }


        Vector2 oppositeDir = -move_direction;

        // Find the angle for this vector (in degrees)
        float angle = Mathf.Atan2(oppositeDir.y, oppositeDir.x) * Mathf.Rad2Deg;

        // Add -90 degrees (or subtract 90)
        angle -= 90f;

        // Now apply the rotation
        // If 2D object (SpriteRenderer, etc)
        var target_rot = Quaternion.Euler(0, 0, angle);





        fire_ball.transform.rotation = Quaternion.RotateTowards(fire_ball.transform.rotation, target_rot, 200 * Time.deltaTime);


        if (game_manager_scr.IsGameOver )
        {

            if (game_over)
            {
                return;
            }

            gege();




        }
    

    }


    void Update()
    {

        if (Input.GetKeyDown(KeyCode.H))
        {
            Time.timeScale = 0.2f;
        }





        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {

            //  StartCoroutine(ReduceEnergyCoroutine(false, 1 / max_energy_level, 0.1f));
            move_direction = Quaternion.Euler(0, 0, Time.deltaTime * 1000) * move_direction; // Rotate left 15°
            PlayPulse(0);
        }



        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            move_direction = Quaternion.Euler(0, 0, -Time.deltaTime * 1000) * move_direction; // Rotate left 15°
            PlayPulse(1);


            // StartCoroutine(ReduceEnergyCoroutine(true, 1 / max_energy_level, 0.1f));


            //   move_direction = Quaternion.Euler(0, 0, newAngle) * Vector2.up;
            //   move_direction = move_direction.normalized * move_direction.magnitude;

        }

        //   path_bending();


        if (gravity_state == 1)
        {
            hor_move_speed = hor_move_speed - 1 * 50 * Time.deltaTime;
        }
        else if (gravity_state == 2)
        {
            hor_move_speed = hor_move_speed + 1 * 50 * Time.deltaTime;

        }



        if (!game_over)
        {

           

            //  HandleMouseSwipe();

            fall_down_check();
            HandleRotation();
            edge_handler();
            HandleInput();

        }



    }

    void edge_handler()
    {
        CheckEdges();
    }



    void HandleRotation()
    {
        if (!cutTheRope)
        {
            float rotationAmount = currentRotationSpeed * Time.deltaTime;
            transform.RotateAround(planet.transform.position, Vector3.back, rotationAmount);
            totalRotation += rotationAmount;
        }
    }

    void HandleInput()
    {

    
      


        if (planet != null)
        {
            arrow.transform.eulerAngles = planet.transform.position - transform.position;

        }

        if (Input.GetMouseButtonDown(0) )
        {
            Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 point2D = new Vector2(wp.x, wp.y);
            Collider2D hit = Physics2D.OverlapPoint(point2D);

            if (hit != null && hit.gameObject.tag == "shop")
            {
                power_up_stuff.SetActive(true);
               game_manager_scr.pause_moment_time_scale = Time.timeScale;
                Time.timeScale = 0.0f;

                game_manager_scr.game_running = false;

                game_manager_go.GetComponent<power_ups_ui_manager>().open_shop();
                return;

            }
        }

      


        if ((Input.GetMouseButtonDown(0) && is_anchord) )
        {
            if(Input.mousePosition.y < Screen.height*0.15f)
            {
                return;
            }


            if (game_over)
            {
                return;
            }
            if (!is_game_started)
            {
                is_game_started = true;

            }

            if (last_attach_time + 0.3f > Time.time)
            {
                return;
            }
            /*
            if (laser_go.GetComponent<laser_scr>().hitted_planet != null)
            {
               // StartCoroutine(teleport(new Vector3(laser_go.GetComponent<laser_scr>().hit.point.x, laser_go.GetComponent<laser_scr>().hit.point.y, transform.position.z), 0.0f));
            }
            else
            {
                tp_counter = 0;
                game_manager_scr.ball_attached = false;
                audioSource.PlayOneShot(planet_leave);

            }
            */
            game_manager_scr.ball_attached = false;
            audioSource.PlayOneShot(planet_leave);
            bottom_canvas.SetActive(false);

            laser_go.SetActive(false);
            energy_sphere.GetComponent<BurstColorTransition>().StartTransition();
            red_energy.gameObject.GetComponent<ParticleSystem>().Play();
            // Debug.Log("ss");


            //  arrow.gameObject.SetActive(false);
            game_manager_scr.cam_target = transform.gameObject;
            is_anchord = false;
            cutTheRope = true;
            planet.GetComponent<planet_attribute_scr>().activated_orbit.SetActive(false);
            planet.GetComponent<planet_attribute_scr>().attached_planet = false;






            move_direction = (planet.transform.position - transform.position);
            //    move_direction = gameObject.transform.up;
            move_direction = move_direction.normalized * 1.2f;
            move_direction = new Vector2(-move_direction.y, move_direction.x);
            //  arrow.transform.rotation = 
            //   Debug.Log(move_direction);
            //  moveSpeed = move_direction.x * 15 * currentRotationSpeed * 0.003f * cut_speed_index;
            // moveSpeed =cut_speed_index;
            //  hor_move_speed = -move_direction.y * 15 * currentRotationSpeed * 0.003f * cut_speed_index;
            transform.SetParent(null);

            if (totalRotation < 360)
            {
                totalRotation = 0;
            }
        }

        // Debug.Log("damn");
        if (cutTheRope && !is_anchord)
        {
            // handle_energy();

            if (!game_manager_scr.game_running)
            {
                return;
            }

            if (Input.GetMouseButtonDown(0))
            {

                if (lastClickTime + 0.2f > Time.time)
                {
                    // Double-click detected!
                    //  Debug.Log("Double click (or tap) detected!");
                    // OnDoubleClick();

                }
                lastClickTime = Time.time; // Reset


            }
            if (Input.GetMouseButtonDown(0))
            {



                if (Input.mousePosition.x > Screen.width / 2f)
                {
                    //   move_direction = Quaternion.Euler(0, 0, Time.deltaTime *- 70) * move_direction; // Rotate left 15°

                    audioSource.PlayOneShot(rotate_sound);
                    GameObject vfx_hit = Instantiate(rotation_vfx, transform.position, Quaternion.identity);
                    Destroy(vfx_hit, 1);
                    move_direction = Quaternion.Euler(0, 0, -Time.deltaTime * 700) * move_direction; // Rotate left 15°
                    PlayPulse(1);
                }
                else
                {
                    //  move_direction = Quaternion.Euler(0, 0, Time.deltaTime * 70) * move_direction; // Rotate left 15°

                    audioSource.PlayOneShot(rotate_sound);
                    GameObject vfx_hit = Instantiate(rotation_vfx, transform.position, Quaternion.identity);
                    Destroy(vfx_hit, 1);

                    move_direction = Quaternion.Euler(0, 0, Time.deltaTime * 700) * move_direction; // Rotate left 15°
                    PlayPulse(0);
                }

            }




            MoveBall();


        }
    }

    void MoveBall()
    {

        // Debug.Log("moving");
        //    (-gravity_force_vector * moveSpeed * Time.deltaTime * normal_or_super_charged_speed) + gravity_force + (hor_move_vector * hor_move_speed * Time.deltaTime * normal_or_super_charged_speed);

        bg_objects_parent.transform.position += new Vector3(0, move_direction.y * Time.deltaTime * normal_or_super_charged_speed * cut_speed_index * ball_move_rate*0.8f, 0);
        if (Input.GetKey(KeyCode.Space))
        {
            transform.position += (new Vector3(move_direction.x * normal_or_super_charged_speed * Time.deltaTime * cut_speed_index, move_direction.y * Time.deltaTime * normal_or_super_charged_speed * cut_speed_index*5, 0)) * ball_move_rate;

        }
        else
        {
            transform.position += (new Vector3(move_direction.x * normal_or_super_charged_speed * Time.deltaTime * cut_speed_index, move_direction.y * Time.deltaTime * normal_or_super_charged_speed * cut_speed_index, 0)) * ball_move_rate;

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        /*
        if (collision.gameObject.CompareTag("bad_planet"))
        {
            if (last_edge_hit + 0.3f < Time.time)
            {
                audioSource.PlayOneShot(edge_effect);
                move_direction.x = -move_direction.x;

                GameObject vfx_hit = Instantiate(hit_vfx_go, transform.position, Quaternion.identity);
                Destroy(vfx_hit, 0.7f);
                last_edge_hit = Time.time;
            }

        }
        */
     //   if (collision.gameObject.CompareTag("planet") & !game_manager_scr.no_death)
        if (collision.gameObject.CompareTag("planet") )
        {

            collision.gameObject.GetComponent<planet_attribute_scr>().moving_planet = false;
            collision.gameObject.GetComponent<planet_attribute_scr>().attached_planet = true;
            if (collision.gameObject.GetComponent<planet_attribute_scr>().is_bomber)
            {
                collision.gameObject.GetComponent<planet_attribute_scr>().ResetAndReduceAgain();
            }
            else
            {
                bottom_canvas.SetActive(true);

            }


            if (transform.position.y < 1000)
            {
                currentRotationSpeed = minRotationSpeed + (transform.position.y / 40);

            }

            last_attach_time = Time.time;
            game_manager_scr.ball_attached = true;

            if (game_manager_scr.is_laser_active)
            {
                laser_go.SetActive(true);
                laser_go.GetComponent<laser_scr>().attached_planet = collision.gameObject;


            }

            red_energyff();

            //  transform.rotation = Quaternion.identity;

            energy_sphere.GetComponent<BurstColorTransition>().ResetColor();
            reset_energy_level();
            swipe_jump_enable = false;

            // collision.gameObject.GetComponent<planet_attribute_scr>().ResetAndReduceAgain();

            bg.color = Color.white;
            gravity_state = 0;
            hor_move_speed = 0f;

            collision.gameObject.transform.parent = null;
            currentRotationSpeed = minRotationSpeed;


            if (tp_counter == 0)
            {
                audioSource.PlayOneShot(normal_planet_sound);

            }

            else if (tp_counter == 1)
            {

                audioSource.PlayOneShot(get_planet);



            }
            second_jump_called = false;
            StopCoroutine("ChangeTimeScaleEased");
            Time.timeScale = 1;
            jump_count = 0;
            //  arrow.gameObject.SetActive(true);
            jump_done = false;

            if (direction_is_left)
            {
                direction_is_left = false;
                bg.color = Color.white;
                gravity_force = Vector3.zero;
                gravity_force_vector = new Vector3(0, -1, 0);
            }

            gravity_force = Vector3.zero;
            moveSpeed = indexed_move_speed;
            game_manager_scr.cam_target = collision.gameObject;
            hitWithPlanet = true;
            planet = collision.gameObject;
            cutTheRope = false;
            ballDirection = Vector2.zero;
            is_anchord = true;
            collision.gameObject.transform.GetChild(0).GetChild(0).GetComponent<CircleCollider2D>().isTrigger = false;
            planet.GetComponent<planet_attribute_scr>().activated_orbit.SetActive(true);
            planet.GetComponent<planet_attribute_scr>().scale_call();


            GameObject score_obj = Instantiate(score_object, planet.transform.position + new Vector3(-6, -2, 0), Quaternion.identity);
            plus_txt = score_obj.transform.GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
            combo_text = score_obj.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
            if (collision.gameObject.transform.position.y > last_top_y)
            {
                prev_score = ConvertTextToInt(score_txt);





                plus_number = (int)(collision.gameObject.transform.position.y) - last_top_y;
                plus_txt.text = "+".ToString() + plus_number.ToString();

                //   Debug.Log(tp_counter + "sss");
                if (tp_counter > 1)
                {
                    score_obj.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(true);

                    if (!score_increasing)
                    {
                        StartCoroutine(LerpInt(prev_score, prev_score + plus_number * tp_counter, 0.5f));
                        combo_text.text = "+" + tp_counter.ToString();
                    }
                    else
                    {
                        StopCoroutine("ConvertTextToInt");
                        StartCoroutine(LerpInt(prev_score, prev_score + plus_number * tp_counter, 0.5f));
                        combo_text.text = "+" + tp_counter.ToString();

                    }

                }
                else
                {
                    score_obj.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(false);

                    if (!score_increasing)
                    {
                        StartCoroutine(LerpInt(prev_score, prev_score + plus_number, 0.5f));
                    }
                    else
                    {
                        StopCoroutine("ConvertTextToInt");
                        StartCoroutine(LerpInt(prev_score, prev_score + plus_number, 0.5f));
                    }

                }

                PlayerPrefs.SetInt("current_score", prev_score + plus_number);


                last_top_y = (int)collision.gameObject.transform.position.y;


            }
        }

        if (collision.gameObject.CompareTag("worm_hole"))
        {
            GameObject planet = Instantiate(planetPrefab, collision.gameObject.transform.position, Quaternion.identity);
            planet.transform.localScale = planet.transform.localScale * 2;
            // planet_manager.GetComponent<PlanetManager>().ConvertPortalToPlanetAndCreateNewSet();
        }

        /*
        if (collision.gameObject.CompareTag("core_black_hole"))
        {
            hor_move_speed = 0;
            moveSpeed = 0;

            if (!black_hole_shrink)
            {
                StartCoroutine(ScaleOverTime());
                black_hole_shrink = true;
                transform.parent = collision.gameObject.transform;
                game_over = true;
                game_manager_go.GetComponent<game_manager_scr>().EndGame(last_score);

                bg.color = Color.gray;
            }
        }
        */
    }

    IEnumerator LerpInt(int a, int z, float time)
    {
        score_increasing = true;
        float elapsedTime = 0f;
        int currentValue = a;

        while (elapsedTime < time)
        {
            float t = elapsedTime / time;
            currentValue = (int)Mathf.Lerp(a, z, t);
            elapsedTime += Time.deltaTime;
            score_txt.SetText(currentValue.ToString());
            yield return null;
        }

        currentValue = z;
        score_txt.SetText(currentValue.ToString());
        score_increasing = false;
    }

    int ConvertTextToInt(TextMeshProUGUI textComponent)
    {
        string text = textComponent.text;
        if (int.TryParse(text, out int result))
        {
            return result;
        }
        else
        {
            Debug.LogError("Text is not a valid number: " + text);
            return 0;
        }
    }

    void CalculateScreenEdges()
    {
        screenLeftEdge = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, 0)).x + 0.1f;
        screenRightEdge = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x - 0.1f;
    }

    void CheckEdges()
    {
        float objectX = transform.position.x;

        if (objectX <= screenLeftEdge && (last_edge_hit + 0.3f < Time.time))
        {
            audioSource.PlayOneShot(edge_effect);
            move_direction.x = -move_direction.x;
            hor_move_speed += 0.5f;
            gravity_force = Vector3.zero;
            //  mainCamera.GetComponent<cam_shake_scr>().Shake(0.3f, move_direction.x );
            GameObject vfx_hit = Instantiate(hit_vfx_go, transform.position, Quaternion.identity);
            Destroy(vfx_hit, 0.7f);
            last_edge_hit = Time.time;
        }

        if (objectX >= screenRightEdge && (last_edge_hit + 0.3f < Time.time))
        {
            audioSource.PlayOneShot(edge_effect);
            move_direction.x = -move_direction.x;
            hor_move_speed -= 0.5f;
            gravity_force = Vector3.zero;
            //   mainCamera.GetComponent<cam_shake_scr>().Shake(0.3f, move_direction.x);
            GameObject vfx_hit = Instantiate(hit_vfx_go, transform.position, Quaternion.identity);
            Destroy(vfx_hit, 0.7f);
            last_edge_hit = Time.time;
        }
    }

    void fall_down_check()
    {
        if (transform.position.y <  bottom_bar.transform.position.y  && !game_over)
        {


            game_manager_go.GetComponent<game_manager_scr>().EndGame(last_score);
            audioSource.PlayOneShot(fall_effect);
            game_over = true;



            gege();
        }
    }

    public static IEnumerator ChangeTimeScaleEased(float startValue, float endValue, float duration, AnimationCurve curve = null)
    {
        if (curve == null)
        {
            curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        }

        float elapsed = 0f;
        Time.timeScale = startValue;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = curve.Evaluate(Mathf.Clamp01(elapsed / duration));
            Time.timeScale = Mathf.Lerp(startValue, endValue, t);
            yield return null;
        }

        Time.timeScale = endValue;
    }

    private IEnumerator RotateZAxisCoroutine(float duration)
    {
        arrow.gameObject.SetActive(true);
        Time.timeScale = 0.3f;
        transform.eulerAngles = new Vector3(0, 0, -45);
        float startRotation = transform.rotation.eulerAngles.z;
        float endRotation = startRotation - 90;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float zRotation = Mathf.Lerp(startRotation, endRotation, elapsedTime / duration);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x,
                                               transform.rotation.eulerAngles.y,
                                               zRotation);
            yield return null;
        }
        arrow.gameObject.SetActive(false);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x,
                                           transform.rotation.eulerAngles.y,
                                           endRotation);
        Time.timeScale = 1f;
        yield return null;
    }

    private IEnumerator ScaleOverTime()
    {
        float elapsedTime = 0f;
        var time = 3f;

        while (elapsedTime < time)
        {
            float progress = Mathf.Clamp01(elapsedTime / time);
            float curveValue = scaleCurve != null ? scaleCurve.Evaluate(progress) : progress;
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, curveValue);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = Vector3.zero;
    }


    private void gravity_changer()
    {
        /*

        if (!black_hole_shrink)
        {
            if (gravity_state == 0)
            {
                bg.color = Color.blue;
                gravity_state = 1;
                hor_move_speed = -0;
            }
            else if (gravity_state == 1)
            {
                bg.color = Color.red;
                hor_move_speed = +0;
                gravity_state = 2;


            }
        }
        */



    }
    void InitializeGyro()
    {
        if (SystemInfo.supportsGyroscope)
        {
            Input.gyro.enabled = true;
            gyroEnabled = true;
            initialAttitude = Input.gyro.attitude;
            initialTiltAngle = initialAttitude.eulerAngles.z;
            Debug.Log("Gyro initialized. Initial tilt: " + initialTiltAngle);
        }
        else
        {
            Debug.LogWarning("Gyroscope not supported - using keyboard controls");
            gyroEnabled = false;
        }
    }
    float NormalizeAngle(float angle)
    {
        // Convert to -180 to 180 range
        angle %= 360;
        if (angle > 180) angle -= 360;
        return angle;
    }


    float GetNormalizedTiltChange()
    {
        if (!gyroEnabled) return 0f;

        // Get current tilt
        float currentTilt = Input.gyro.attitude.eulerAngles.z;

        // Normalize angles (convert to -180 to 180 range)
        float normalizedInitial = NormalizeAngle(initialTiltAngle);
        float normalizedCurrent = NormalizeAngle(currentTilt);

        // Calculate change from initial
        float tiltChange = normalizedCurrent - normalizedInitial;

        // Apply smoothing
        currentSmoothedTilt = Mathf.Lerp(currentSmoothedTilt, tiltChange, smoothingFactor);

        return currentSmoothedTilt;
    }

    void UpdateHorizontalSpeed(float tiltChange)
    {
        // Calculate speed change based on tilt
        float speedChange = tiltChange * tiltSensitivity;

        // Update horizontal speed (clamped to max speed)
        hor_move_speed += Mathf.Clamp(speedChange * Time.deltaTime,
            -maxSpeed,
            maxSpeed
        );


    }

    private void handle_energy()
    {
        current_energy_level -= Time.deltaTime * 10 * energy_usage_rate;

        energy_level_slider.GetComponent<Slider>().value = current_energy_level / max_energy_level;


        if (current_energy_level <= 0.1)
        {

            if (game_manager_scr.no_death)
            {



            }

            else
            {

                GameObject moob_go = Instantiate(boom_go_vfx, transform.position, Quaternion.identity);
                Destroy(moob_go, 0.8f);

                energy_sphere.SetActive(false);
                game_manager_go.GetComponent<game_manager_scr>().EndGame(last_score);
                game_over = true;
            }

        }


    }
    private void reset_energy_level()
    {


        current_energy_level = max_energy_level;

        energy_level_slider.GetComponent<Slider>().value = current_energy_level / max_energy_level;


    }


    void OnDoubleClick()
    {

        StartCoroutine(DecayRoutine(3f, 0.4f));


        Debug.Log("Double Click Detected!");
        // Your double click logic here
    }

    public void StartDecay(float peakindex, float decayDuration)
    {
        // Stop previous decay if running

        // Instantly set to peak (or modify if you want a linear increase)

        // Start decaying with curve
    }

    private IEnumerator DecayRoutine(float charge_index, float duration)
    {

        if (!super_charge_on)
        {
            audioSource.PlayOneShot(bounce_effect);

            GameObject vfx_hit = Instantiate(hit_vfx_go, transform.position, Quaternion.identity);
            Destroy(vfx_hit, 0.5f);

            normal_or_super_charged_speed = charge_index;

            var charged_value = charge_index;

            //  Debug.Log(normal_or_super_charged_speed);

            // Debug.Log("chartge _on");
            super_charge_on = true;





            //  Debug.Log(normal_or_super_charged_speed);

            //  Debug.Log("chartge _on");
            //
            float elapsed = 0f;

            while (elapsed < duration)
            {
                // Debug.Log(normal_or_super_charged_speed);

                //   Debug.Log("chartge _on");
                elapsed += Time.deltaTime;
                float t = elapsed / duration;

                // Use AnimationCurve to control decay
                float curveT = decayCurve.Evaluate(t);
                normal_or_super_charged_speed = Mathf.Lerp(charged_value, initial_normal_or_super_charged_speed, t) * curveT;

                yield return null;
            }

            normal_or_super_charged_speed = initial_normal_or_super_charged_speed; // Ensure exact value at end
            super_charge_on = false;
            yield return null;
        }

    }

    private IEnumerator ReduceEnergyCoroutine(bool is_right, float amountToReduce, float duration)
    {



        if (is_right)
        {
            move_direction = Quaternion.Euler(0, 0, -9f) * move_direction; // Rotate left 15°
            audioSource.PlayOneShot(rotate_sound);

            GameObject moob_go = Instantiate(right_flash, right_vfx.position, Quaternion.EulerAngles(0, -90, 0), mainCamera.transform);
            Destroy(moob_go, 1.5f);

            energy_level_slider_fill.GetComponent<Image>().color = Color.red;


        }
        else
        {
            move_direction = Quaternion.Euler(0, 0, 9f) * move_direction; // Rotate left 15°
            audioSource.PlayOneShot(rotate_sound);

            GameObject moob_go = Instantiate(left_flash, left_vfx.position, Quaternion.EulerAngles(0, 90, 0), mainCamera.transform);
            Destroy(moob_go, 1.5f);
            energy_level_slider_fill.GetComponent<Image>().color = Color.red;




        }
        float startValue = energy_level_slider.GetComponent<Slider>().value;
        float endValue = startValue - amountToReduce;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            energy_level_slider.GetComponent<Slider>().value = Mathf.Lerp(startValue, endValue, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        energy_level_slider_fill.GetComponent<Image>().color = Color.yellow;

        // Ensure the final value is exact
        energy_level_slider.GetComponent<Slider>().value = endValue;
    }
    private IEnumerator teleport(Vector3 target_pos, float totalDuration)
    {
        tp_counter = tp_counter + 1;

        if (last_tp_time + 2 > Time.time)
        {

            switch (tp_counter)
            {




                case 2:
                    //  Debug.Log("2");
                    audioSource.PlayOneShot(hit1);

                    break;
                case 3:
                    //   Debug.Log("3");
                    audioSource.PlayOneShot(hit2);

                    break;
                case 4:
                    //   Debug.Log("4");
                    audioSource.PlayOneShot(hit3);

                    break;
                case 5:
                    //  Debug.Log("5");
                    audioSource.PlayOneShot(hit4);

                    break;
                case 6:
                    //  Debug.Log("6");
                    audioSource.PlayOneShot(hit5);

                    break;

                case 7:
                    //  Debug.Log("7");
                    audioSource.PlayOneShot(hit6);

                    tp_counter = tp_counter - 1;
                    //  activate_fireball();
                    break;


            }

        }

        else
        {
            tp_counter = 1;
        }

        last_tp_time = Time.time;




        game_manager_scr.ball_attached = false;

        GameObject vfx1 = Instantiate(teleport_vfx, transform.position, Quaternion.identity);
        Destroy(vfx1, 1f);

        Vector3 startPosition = transform.position;
        int steps = 3;

        // Time per step
        float stepDuration = totalDuration / steps;

        for (int i = 1; i <= steps; i++)
        {
            Vector3 stepTarget = Vector3.Lerp(startPosition, target_pos, (float)i / steps);
            float elapsed = 0f;

            Vector3 stepStart = transform.position;

            while (elapsed < stepDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / stepDuration);
                transform.position = Vector3.Lerp(stepStart, stepTarget, t);
                yield return null;
            }

            // Ensure exact position at step end
            transform.position = stepTarget;


            GameObject vfx2 = Instantiate(teleport_vfx, transform.position, Quaternion.identity);
            Destroy(vfx2, 1f);
        }

        game_manager_scr.ball_attached = false;
        yield return new WaitForSeconds(0.3f);
        game_manager_scr.ball_attached = true;


    }


    public void gege()
    {

        GameObject moob_go = Instantiate(boom_go_vfx, transform.position, Quaternion.identity);
        Destroy(moob_go, 0.8f);

        energy_sphere.SetActive(false);
        game_manager_go.GetComponent<game_manager_scr>().EndGame(last_score);
        game_over = true;

        Destroy(red_energy);
        // red_energy.gameObject.SetActive(false);
        transform.GetComponent<CircleCollider2D>().enabled = false;
        Debug.Log(current_score);
        game_manager_scr.current_score = current_score;
        Debug.Log(current_score);
        //  Debug.Log(game_manager_scr.best_score);

        PlayerPrefs.SetInt("coin_number", game_manager_scr.coin_number);
        if (ConvertTextToInt(score_txt) > PlayerPrefs.GetInt("best_score"))
        {
           StartCoroutine( new_record());
        }
        final_score_txt.gameObject.SetActive(true);
        final_score_txt.GetComponent<TextMeshProUGUI>().text = ConvertTextToInt(score_txt).ToString();
    }

    private void red_energyff()
    {


        var par = red_energy.gameObject.GetComponent<ParticleSystem>();


        par.Stop(); // Stop if currently playing
        par.Clear(); // Clear all existing particles
        par.time = 0; // Reset the playback time


    }

    public void enemy_kill(GameObject enemy)
    {

        Destroy(enemy);

        GameObject moob_go = Instantiate(boom_go_vfx, enemy.transform.position, Quaternion.identity);
        Destroy(moob_go, 0.8f);

    }
    /// <summary>
    /// /////////////////////////////////// right left pulse
    /// </summary>
    // Call this with 0 or 1 to pulse either sprite independently
    public void PlayPulse(int spriteIndex)
    {
        if (spriteRenderers[spriteIndex] == null) return;

        SetAlpha(spriteIndex, maxAlpha);
        SetScaleX(spriteIndex, maxScaleX);

        // Stop return if already running
        if (returnCoroutines[spriteIndex] != null)
            StopCoroutine(returnCoroutines[spriteIndex]);

        returnCoroutines[spriteIndex] = StartCoroutine(ReturnToRest(spriteIndex));
    }

    private IEnumerator ReturnToRest(int spriteIndex)
    {
        SpriteRenderer renderer = spriteRenderers[spriteIndex];
        float t = 0;
        float startAlpha = renderer.color.a;
        float startScaleX = renderer.transform.localScale.x;

        while (t < returnDuration)
        {
            t += Time.deltaTime;
            float normalized = Mathf.Clamp01(t / returnDuration);

            SetAlpha(spriteIndex, Mathf.Lerp(startAlpha, minAlpha, normalized));
            SetScaleX(spriteIndex, Mathf.Lerp(startScaleX, minScaleX, normalized));

            yield return null;
        }

        SetAlpha(spriteIndex, minAlpha);
        SetScaleX(spriteIndex, minScaleX);
        returnCoroutines[spriteIndex] = null;
    }

    private void SetAlpha(int idx, float alpha)
    {
        Color c = spriteRenderers[idx].color;
        c.a = alpha;
        spriteRenderers[idx].color = c;
    }

    private void SetScaleX(int idx, float x)
    {
        Vector3 s = spriteRenderers[idx].transform.localScale;
        s.x = x;
        spriteRenderers[idx].transform.localScale = s;
    }


    private void activate_fireball()
    {
        fire_ball.gameObject.SetActive(true);
        GetComponent<TrailRenderer>().enabled = false;
        tp_counter = 0;
        fire_ball_is_active = true;
        game_manager_scr.no_death = true;
    }

    private void deactive_fireball()
    {
        fire_ball.gameObject.SetActive(false);
        tp_counter = 0;
        GetComponent<TrailRenderer>().enabled = true;
        fire_ball_is_active = false;
        game_manager_scr.no_death = false;


    }
    public  void activate_shield()
    {
        sheild.gameObject.SetActive(true);
        sheild.gameObject.GetComponent<shield_scr>().set_time();


        audioSource.PlayOneShot(batterY_refill_sound);
    }
    public void activate_magnet()
    {
        magnet.gameObject.SetActive(true);


        audioSource.PlayOneShot(batterY_refill_sound);
    }
    private IEnumerator new_record()
    {
        

           PlayerPrefs.SetInt("best_score", ConvertTextToInt(score_txt));
           game_manager_scr.best_score = ConvertTextToInt(score_txt);

        yield return new WaitForSeconds(0);

           crown.SetActive(true);




     }
}