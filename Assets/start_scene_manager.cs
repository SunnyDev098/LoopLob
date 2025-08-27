using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using System.Threading.Tasks;

public class start_scene_manager : MonoBehaviour
{

    public bool leader_board_success;
    public GameObject loading;
    public GameObject failed_username;
    public GameObject failed_connection;



    public GameObject mycanvas;


    public Camera main_cam;
    public Camera option_cam;
    public Camera leader_board_cam;

    [Header("Assign Your Buttons Here")]
    public Button Play;
    public Button option;
    public Button tutorial;
    public Button back_to_menu_option;
    public Button back_to_menu_tutorial;


    private float last_click_time;
    void Start()
    {
        Play.onClick.AddListener(HandleButtonA);
        option.onClick.AddListener(HandleButtonB);
        tutorial.onClick.AddListener(HandleButtonC);
        back_to_menu_option.onClick.AddListener(HandleButtonD);
        back_to_menu_tutorial.onClick.AddListener(HandleButtonE);
    }

    void HandleButtonA()
    {
        if(last_click_time +0.5f > Time.time)
        {
            return;
        }

        last_click_time = Time.time;

        if (ScenePreloader.instance != null && ScenePreloader.instance.IsSceneReady)
        {
            // Scene is ready, so activate it!
            ScenePreloader.instance.ActivatePreloadedScene();
        }
        else
        {

            mycanvas.SetActive(true);

        }

            Debug.Log("Button A pressed!");
        // Put your logic here
    }

    void HandleButtonB()
    {
        if (last_click_time + 0.5f > Time.time)
        {
            return;
        }


        last_click_time = Time.time;

        main_cam.depth = 0;
        main_cam.gameObject.GetComponent<AudioListener>().enabled = false;

        option_cam.depth = 10;
        option_cam.gameObject.GetComponent<AudioListener>().enabled = true;

    }

   async void HandleButtonC()
    {



        if (!PlayerPrefs.HasKey("username"))
        {

            failed_username.SetActive(true);
            await Task.Delay(3000);
            failed_username.SetActive(false);


            return;
        }
        loading.SetActive(true);
        loading.GetComponent<ParticleSystem>().Play();

        leader_board_success = false;


        if (last_click_time + 0.5f > Time.time)
        {
            return;
        }


        last_click_time = Time.time;



        GetComponent<LeaderboardManager>().SubmitAndGetTopInfo();


        await Task.Delay(3000);

        if (leader_board_success)
        {
            main_cam.depth = 0;
            main_cam.gameObject.GetComponent<AudioListener>().enabled = false;

            leader_board_cam.depth = 10;
            leader_board_cam.gameObject.GetComponent<AudioListener>().enabled = true;
            loading.SetActive(false);


        }
        else
        {
            loading.SetActive(false);

            failed_connection.SetActive(true);

            await Task.Delay(4000);
            failed_connection.SetActive(false);


        }





        // Put your logic here
    }

    void HandleButtonD()
    {
        if (last_click_time + 0.5f > Time.time)
        {
            return;
        }


        last_click_time = Time.time;


        main_cam.depth = 10;
        main_cam.gameObject.GetComponent<AudioListener>().enabled = true;

        option_cam.depth = 0;
        option_cam.gameObject.GetComponent<AudioListener>().enabled = false;

        Debug.Log("Button D pressed!");
        // Put your logic here
    }

    void HandleButtonE()
    {

        if (last_click_time + 0.5f > Time.time)
        {
            return;
        }


        last_click_time = Time.time;


        main_cam.depth = 10;
        main_cam.gameObject.GetComponent<AudioListener>().enabled = true;

        leader_board_cam.depth = 0;
        leader_board_cam.gameObject.GetComponent<AudioListener>().enabled = false;

        Debug.Log("Button E pressed!");

    }
}
