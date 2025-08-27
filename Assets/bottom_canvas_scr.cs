using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using JetBrains.Annotations;
public class SpriteClickHandler : MonoBehaviour
{
    public TextMeshProUGUI coin_num_gui;
    public GameObject ghost_fetcher;
    public GameObject ghost_card;


    public GameObject the_ball;
    public GameObject shield_card;
    public GameObject fuel_card;
    public GameObject magnet_card;
   // public GameObject the_ball;

    private bool clicking;




    private void Start()
    {
        Debug.Log(game_manager_scr.coin_number);
        coin_num_gui.text = game_manager_scr.coin_number.ToString();
    }
    void Update()
    {
          

        if (Input.GetMouseButtonDown(0))
        {
            // Raycast from the mouse position
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                GameObject clickedObject = hit.collider.gameObject;

                // Check tag
                switch (clickedObject.tag)
                {
                    case "shield_card":
                        Debug.Log("You clicked an enemy!");
                        // Do something related to enemy
                        StartCoroutine(PulseScale(clickedObject,0.1f));
                        //    if (game_manager_scr.coin_number >=50)
                            if (true)
                            {
                              activate_shield();
                            }
                        break;

                    case "fuel_card":
                        Debug.Log("You clicked an item!");
                        StartCoroutine(PulseScale(clickedObject, 0.1f));

                        // Do something related to item
                        break;

                    case "magnet_card":
                        Debug.Log("You clicked a button!");
                        activate_magnet();
                        StartCoroutine(PulseScale(clickedObject, 0.1f));

                        // Trigger your button action
                        break;

                    default:
                        Debug.Log("You clicked something untagged.");
                        StartCoroutine(PulseScale(clickedObject, 0.1f));
                            ghost_fetcher.SetActive(true);
                             ghost_card.SetActive(false);
                        break;
                }
            }
        }
    }


    public IEnumerator PulseScale(GameObject target, float t)
    {
        if (clicking)
        {
            yield break;
        }
        clicking = true;

        
        Vector3 originalScale = target.transform.localScale;
        Vector3 targetScale = originalScale * 0.8f;

        float halfTime = t / 2f;
        float elapsed = 0f;

        // Scale down
        while (elapsed < halfTime)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / halfTime;
            target.transform.localScale = Vector3.Lerp(originalScale, targetScale, progress);
            yield return null;
        }

        // Scale back up
        elapsed = 0f;
        while (elapsed < halfTime)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / halfTime;
            target.transform.localScale = Vector3.Lerp(targetScale, originalScale, progress);
            yield return null;
        }

        // Ensure perfect original scale at the end
        target.transform.localScale = originalScale;
        clicking = false;

    }

    private void activate_shield()
    {
        shield_card.SetActive(false);
        game_manager_scr.is_shield_active = true;

        game_manager_scr.coin_number = game_manager_scr.coin_number - 50;
        PlayerPrefs.SetInt("coin_number", game_manager_scr.coin_number);
        coin_num_gui.GetComponent<TextMeshProUGUI>().text = game_manager_scr.coin_number.ToString();
        the_ball.GetComponent<ball_scr>().activate_shield();

    }

    private void activate_magnet()
    {
        magnet_card.SetActive(false);
        game_manager_scr.is_magnet_active = true;

        game_manager_scr.coin_number = game_manager_scr.coin_number - 50;
        PlayerPrefs.SetInt("coin_number", game_manager_scr.coin_number);
        coin_num_gui.GetComponent<TextMeshProUGUI>().text = game_manager_scr.coin_number.ToString();

        the_ball.GetComponent<ball_scr>().activate_magnet();
    
    }
}
