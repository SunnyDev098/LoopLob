using TMPro;
using UnityEngine;

public class ClickableSprite_Scr : MonoBehaviour
{

    GameObject power_up_stuff;

    GameObject laser_card;
     GameObject shield_card;
     GameObject gem_number_go;
     GameObject back_to_game_btn;
     GameObject go_power_up_btn;
    // Set this in the Inspector or via script to limit detection to a specific tag if needed
     string requiredTag ;

    private void Start()
    {
        power_up_stuff = GameObject.FindWithTag("power_up_stuff");

        laser_card = GameObject.FindWithTag("laser_card");
        shield_card = GameObject.FindWithTag("shield_card");
        gem_number_go = GameObject.FindWithTag("gem_num_go");
        back_to_game_btn = GameObject.FindWithTag("back_to_game_btn");
        go_power_up_btn = GameObject.FindWithTag("go_power_up_btn");

        if (game_manager_scr.is_laser_active)
        {
            laser_card.gameObject.SetActive(false);

        }
        if (game_manager_scr.is_shield_active)
        {
            shield_card.gameObject.SetActive(false);

        }
        requiredTag = gameObject.tag;
       // gem_number_go.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = game_manager_scr.coin_number.ToString();

    }

    void Update()
    {
        // Mouse click (PC/Mac/Web)
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            DetectClick(wp);
            Debug.Log("adw");

        }
      


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


            requiredTag = hit.gameObject.tag;





            switch (requiredTag)
            {
                case "laser_card":


                    if (game_manager_scr.coin_number >= 3)
                    {
                        laser_card.SetActive(false);
                        game_manager_scr.coin_number = game_manager_scr.coin_number - 3;
                        PlayerPrefs.SetInt("coin_number", game_manager_scr.coin_number);
                        game_manager_scr.is_laser_active = true;

                        gem_number_go.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = game_manager_scr.coin_number.ToString();
                    }


                    break;



                case "shield_card":

                    if (game_manager_scr.coin_number >= 1)
                    {
                        shield_card.SetActive(false);
                        game_manager_scr.is_shield_active = true;

                        game_manager_scr.coin_number = game_manager_scr.coin_number - 1;
                        PlayerPrefs.SetInt("coin_number", game_manager_scr.coin_number);
                        gem_number_go.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = game_manager_scr.coin_number.ToString();

                    }


                    break;



                case "back to game":

                    power_up_stuff.SetActive(false);

                    go_power_up_btn.SetActive(true);

                    game_manager_scr.in_power_ups = false;


                    break;

                case "go_power_up_btn":



                    go_power_up_btn.SetActive(false);


                    power_up_stuff.SetActive(true);

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

    
}
