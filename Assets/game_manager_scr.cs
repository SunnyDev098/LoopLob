using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public  class game_manager_scr : MonoBehaviour
{




    public Button back_to_menu_btn;
    public Button send_message_btn;

    public static Dictionary<string, string> global_userMessageDic;
    public static bool ghost_messages_collected;
    public static bool game_running;
    public static float pause_moment_time_scale;

    public static bool game_started;
    public static bool in_power_ups;
    public static bool is_laser_active;
    public static bool is_shield_active;
    public static bool is_magnet_active;
    public static float left_bar_x;
    public static float right_bar_x;




    /// <summary>
    /// /////////////////// prefs list
    /// </summary>


    public static int best_score;
    public static int current_score;

    public static int coin_number;
    public static float  music_volume;
    public static float  sfx_volume;



    // Singleton instance
   // public static game_manager_scr Instance { get; private set; }
    public static GameObject cam_target;
    public static bool no_death ;
    public static int current_game_level;
    public static int  next_danger_zone_height;
    public static bool  check_for_danger_zone;
    public static bool ball_attached;
    public static bool inside_danger_zone;

    public static int vfx_volume;

    public Button retry_btn;
    // Game state
    public static bool IsGameOver;
    public int TotalPoints { get; private set; }





    /// <summary>
    /// //////ui section
    /// </summary>



    public GameObject power_up_stuff;


    private void Start()
    {
        pause_moment_time_scale = 1f;
        game_running = true ;
        is_shield_active = false;
        is_laser_active = true;
        is_magnet_active= false;
        next_danger_zone_height = -1;
        check_for_danger_zone = true;
        inside_danger_zone = false;

        no_death = false;
        game_started = true;
        current_game_level = 0;

        Application.runInBackground = true;
        Application.targetFrameRate = 60;
    }
    private void Awake()
    {   
        /*
        // Ensure only one instance of GameManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
        */
        // Initialize game state
        IsGameOver = false;
        TotalPoints = 0;
    }

    // Call this method to end the game
    public void EndGame(int finalScore)
    {
        IsGameOver = true;
        TotalPoints = finalScore;
      //  Debug.Log("Game Over! Final Score: " + TotalPoints);

        retry_btn.gameObject.SetActive(true);
        back_to_menu_btn.gameObject.SetActive(true);


        send_message_btn.gameObject.SetActive(true);
    }

    // Reset the game state (for retry or restart)
    public void ResetGame()
    {
        IsGameOver = false;
        TotalPoints = 0;
    }
}