using TMPro;
using UnityEngine;

public class SimpleFPSCounter : MonoBehaviour
{
    public TMP_Text fpsText;
    public float updateRate = 4f; // Updates per second

    private float timer = 0f;
    private int count = 0;

    void Update()
    {
        timer += Time.deltaTime;
        count++;

        if (timer > 1f / updateRate)
        {
            float fps = count / timer;

            var de_fps = (int)fps;
            fpsText.text = de_fps.ToString() + "/FPS";
            timer = 0f;
            count = 0;
        }
    }
}