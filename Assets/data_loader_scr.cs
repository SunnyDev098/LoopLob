using UnityEngine;

public class data_loader_Scr : MonoBehaviour
{
    // Example variables to be assigned from PlayerPrefs if they exist
    private float musicVolume;
    private int highScore;
    private string unlockedSkin;

    void Start()
    {
        // Check if "musicVolume" exists


        PlayerPrefs.SetInt("current_score", 0);



        if (PlayerPrefs.HasKey("coin_number"))
        {
            game_manager_scr.coin_number = PlayerPrefs.GetInt("coin_number");
        }
        else
        {

            game_manager_scr.coin_number = 0;
        }

        if (PlayerPrefs.HasKey("music_volume"))
        {
            game_manager_scr.music_volume = PlayerPrefs.GetFloat("music_volume");
        }
        else
        {

            game_manager_scr.music_volume = 1f;
        }



        if (PlayerPrefs.HasKey("sfx_volume"))
        {
            game_manager_scr.sfx_volume = PlayerPrefs.GetFloat("sfx_volume");
        }
        else
        {

            game_manager_scr.music_volume = 1f;
        }







        if (PlayerPrefs.HasKey("best_score"))
        {
            game_manager_scr.best_score = PlayerPrefs.GetInt("best_score");
        }
        else
        {

            game_manager_scr.best_score = 0;
        }

        if (PlayerPrefs.HasKey("coin_number"))
        {
            game_manager_scr.coin_number = PlayerPrefs.GetInt("coin_number");
           // game_manager_scr.coin_number = 10;

        }
        else
        {

            game_manager_scr.coin_number = 10;
        }


    }
}
