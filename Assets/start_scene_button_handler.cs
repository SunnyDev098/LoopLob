using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class start_scene_button_handler : MonoBehaviour
{
    public Button play_myButton; // Assign in Inspector
    public Button options_myButton; // Assign in Inspector
    public GameObject loading_go;
    void Start()
    {
        play_myButton.onClick.AddListener(OnplayButtonClick);
        options_myButton.onClick.AddListener(OnoptionButtonClick);
    }

    void OnplayButtonClick()
    {
        // Your code here!
        Debug.Log("play was clicked!");
        SceneManager.LoadScene("game_scene");
        loading_go.SetActive(true);
    }


    void OnoptionButtonClick()
    {
        // Your code here!
        Debug.Log("option was clicked!");
        // Do whatever you want here.
    }
}
