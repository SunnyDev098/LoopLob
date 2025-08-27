using TMPro;
using UnityEngine;
using System.Collections;
public class power_ups_ui_manager : MonoBehaviour
{
    public GameObject cam;


    public GameObject the_ball;

    public GameObject power_up_stuff;

    public GameObject laser_card;
    public GameObject shield_card;
    public GameObject gem_number_go;
    public GameObject back_to_game_btn;
    public GameObject shop_btn;
    public GameObject go_power_up_btn;
    // Set this in the Inspector or via script to limit detection to a specific tag if needed
    string requiredTag;


  
  

    void Update()
    {

      
        // Mouse click (PC/Mac/Web)
        if (Input.GetMouseButtonDown(0))
        {
            if (game_manager_scr.game_running)
            {
                return;
            }

            Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            DetectClick(wp);
            Debug.Log("adw");




            Vector2 mousePos = Input.mousePosition;
            float screenHeight = Screen.height;
            float thresholdY = screenHeight * 0.4f; // Bottom 40% zone

            if (mousePos.y <= thresholdY)
            {
                Debug.Log("dwadaw");


                StartCoroutine(ScaleDownRoutine());
                // gem_number_go.SetActive(false);
                back_to_game();
                // Do your stuff here
            }


        }

        if (Input.GetMouseButtonDown(0)) // 0 = left mouse button
        {
           
        }

    }
    public void open_shop()
    {
        if (game_manager_scr.is_shield_active)
        {
            shield_card.SetActive(false);
        }
        else
        {
            shield_card.SetActive(true);

        }

        if (game_manager_scr.is_laser_active)
        {
            laser_card.SetActive(false);
        }


        gem_number_go.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = game_manager_scr.coin_number.ToString();
        shop_btn.SetActive(false);
        gem_number_go.SetActive(true);
    }
    private void back_to_game()
    {
    


        game_manager_scr.game_started = true;
        game_manager_scr.game_running = true;
        shop_btn.SetActive(true);

        Time.timeScale = game_manager_scr.pause_moment_time_scale;
        power_up_stuff.SetActive(false);
        gem_number_go.SetActive(false);

    }
    private IEnumerator ScaleDownRoutine()
    {
        
        Vector3 initialScale = power_up_stuff.transform.localScale;
        float duration = 0.3f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
           // float t = elapsed / duration;
            //power_up_stuff.transform.localScale = Vector3.Lerp(initialScale, Vector3.zero, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        //power_up_stuff.transform.localScale = Vector3.zero; // Ensure it's fully zero at the end
        
        game_manager_scr.game_started = true ;
        game_manager_scr.game_running = true ;
        power_up_stuff.SetActive(false);
        yield return null;

    }
    void DetectClick(Vector3 worldPoint)
    {
        Vector2 point2D = new Vector2(worldPoint.x, worldPoint.y);
        Collider2D hit = Physics2D.OverlapPoint(point2D);
        // Debug.Log($"Clicked: {hit.gameObject.name}");


        if (hit)
        {
            // >>> Your action here <<<

            Debug.Log("adadwwadw");
            Debug.Log(hit.gameObject.name);


            requiredTag = hit.gameObject.tag;





            switch (requiredTag)
            {
                case "laser_card":
                    Debug.Log(hit.gameObject.tag);


                    if (game_manager_scr.coin_number >= 3)
                    {
                        laser_card.SetActive(false);
                        game_manager_scr.coin_number = game_manager_scr.coin_number - 3;
                        PlayerPrefs.SetInt("coin_number", game_manager_scr.coin_number);
                        game_manager_scr.is_laser_active = true;

                        the_ball.transform.GetChild(0).gameObject.SetActive(true);

                        gem_number_go.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = game_manager_scr.coin_number.ToString();
                    }


                    break;



                case "shield_card":
                    Debug.Log(hit.gameObject.tag);
                    Debug.Log(game_manager_scr.coin_number);

                    if (game_manager_scr.coin_number >= 1)
                    {
                        shield_card.SetActive(false);
                        game_manager_scr.is_shield_active = true;

                        game_manager_scr.coin_number = game_manager_scr.coin_number - 1;
                        PlayerPrefs.SetInt("coin_number", game_manager_scr.coin_number);
                        gem_number_go.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = game_manager_scr.coin_number.ToString();
                        the_ball.GetComponent<ball_scr>().activate_shield();



                    }


                    break;



                case "back_to_game_btn":
                    Debug.Log(hit.gameObject.tag);

                    back_to_game_btn.SetActive(false);
                    laser_card.gameObject.SetActive(false);
                    shield_card.gameObject.SetActive(false);


                    go_power_up_btn.SetActive(true);
                  

                    game_manager_scr.in_power_ups = false;


                    break;

                case "go_power_up_btn":

                    Debug.Log(hit.gameObject.tag);


                    if (!game_manager_scr.is_laser_active)
                    {
                        laser_card.gameObject.SetActive(true);

                    }
                    if (!game_manager_scr.is_shield_active)
                    {
                        shield_card.gameObject.SetActive(true);

                    }



                    gem_number_go.SetActive(true);
                    back_to_game_btn.SetActive(true);

                    go_power_up_btn.SetActive(false);


                    game_manager_scr.in_power_ups = true;

                    break;


            }

            // e.g. Destroy(hit.gameObject);
        }
    }

    void OnMouseDown()
    {
        // Check tag (optional: omit if you only attach this script to "Clickable" objects)
        if (string.IsNullOrEmpty(requiredTag) || CompareTag(requiredTag))
        {
            // ACTION: Replace this with your custom logic
            Debug.Log($"Sprite with tag '{requiredTag}' clicked: {gameObject.name}");
        }



    }

    public void gem_increase()
    {
        game_manager_scr.coin_number = game_manager_scr.coin_number +1;
        PlayerPrefs.SetInt("coin_number", game_manager_scr.coin_number);
        gem_number_go.SetActive (true);
        gem_number_go.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = game_manager_scr.coin_number.ToString();
        Invoke("gem_hider", 3);
    }


    private void gem_hider()
    {
        gem_number_go.SetActive(false);

    }

}
