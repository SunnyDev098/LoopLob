using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // For Slider
using TMPro; // For TextMeshProUGUI
using System.Collections;

public class ScenePreloader : MonoBehaviour
{
    public static ScenePreloader instance; // Singleton for global access

    private AsyncOperation asyncLoad;

    [Header("UI References")]
    public Slider progressBar;
    public TextMeshProUGUI progressText;

    public bool autoStart = true;               // Optional: auto-start loading at Start

    public bool IsSceneReady
    {
        get { return asyncLoad != null && asyncLoad.progress >= 0.9f; }
    }

    void Awake()
    {
        // Basic singleton pattern
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    private void Start()
    {
        Application.targetFrameRate = 60;

        if (progressBar != null)
            progressBar.value = 0;

        if (progressText != null)
            progressText.text = "0%";

        if (autoStart)
            PreloadScene("game_scene");   // Or expose public string for scene name
    }

    // Call this to start loading
    public void PreloadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false; // Wait for explicit activation

        while (asyncLoad.progress < 0.9f)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);

            if (progressBar != null)
                progressBar.value = progress;

            if (progressText != null)
                progressText.text = Mathf.RoundToInt(progress * 100f) + "%";

            yield return null;
        }

        // Set display to 100%
        if (progressBar != null)
            progressBar.value = 1f;

        if (progressText != null)
            progressText.text = "100%";
    }

    // Anyone can call this!
    public void ActivatePreloadedScene()
    {
        if (asyncLoad != null)
            asyncLoad.allowSceneActivation = true;
    }
}
