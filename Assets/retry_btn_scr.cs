using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using VSX.UI;
using TMPro;
public class ButtonHandler : MonoBehaviour
{
    [SerializeField] private Button myButton;
    public GameObject ghost_sender;
    public TextMeshProUGUI myTextMeshPro;
    void Start()
    {
        // Add listener for click event
        myButton.onClick.AddListener(MyButtonClick);
    }

    void MyButtonClick()
    {
        Debug.Log("dawdwawds");


        if (gameObject.CompareTag("game_to_menu_btn"))
        {
            StartCoroutine(UnloadAndLoad("start_scene"));

        }


        else if (gameObject.CompareTag("leave_message_btn"))
        {

         GameObject message_board=   gameObject.transform.GetChild(4).gameObject;
            message_board.SetActive(true);
            message_board.gameObject.transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>().SetText(PlayerPrefs.GetString("username"));
        }

        else if (gameObject.CompareTag("final_send"))
        {

            GameObject message_board = gameObject.transform.parent.GetChild(4).gameObject;

            ghost_sender.GetComponent<ghost_message_scr>().SubmitDeathMessage(PlayerPrefs.GetInt("current_score"), myTextMeshPro.text);
            gameObject.transform.parent.transform.parent.gameObject.SetActive(false);

        }
        else 
        {
            Debug.Log("Button was clicked!");
            string currentSceneName = SceneManager.GetActiveScene().name;

            // Reload the scene
            SceneManager.LoadScene(currentSceneName);

        }

    }
    private IEnumerator UnloadAndLoad(string sceneToLoad)
    {
        // Optionally get the current scene's name
        string currentScene = SceneManager.GetActiveScene().name;

        // Start loading the new scene additively 
        AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
        while (!loadOp.isDone)
            yield return null;

        // Optionally set the new scene as active
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneToLoad));

        // Now unload the previous scene
        AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(currentScene);
        while (!unloadOp.isDone)
            yield return null;
    }
    // Optional: Remove listener when not needed
    void OnDestroy()
    {
        myButton.onClick.RemoveListener(MyButtonClick);
    }
}