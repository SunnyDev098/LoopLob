using UnityEngine;
using System.Collections;
using static UnityEngine.GraphicsBuffer;
public class cam_shake_scr : MonoBehaviour
{


    public AudioClip danger_alarm;
    public float maxAlpha = 1f;
    public float cycleDuration = 1f;
    public int pulseCount = 3;



    [Header("Settings")]
    public float defaultDuration = 0.3f;
    public float defaultMagnitude = 0.1f;

    private Vector3 originalPos;
    private float currentShakeDuration;
    private float currentMagnitude;
    public GameObject le_ball;
    public GameObject gray_filter;


    private float offest =10;
    void Awake()
    {
        game_manager_scr.ball_attached = true;
    }

    public void Shake(float duration, float magnitude)
    {
        currentShakeDuration = duration;
        currentMagnitude = magnitude;
        StopAllCoroutines();
        StartCoroutine(DoShake());
    }

    IEnumerator DoShake()
    {
        float elapsed = 0f;

        while (elapsed < currentShakeDuration)
        {
            // Easing out (1 - progress)^2
            float progress = elapsed / currentShakeDuration;
            float currentStrength = currentMagnitude * Mathf.Pow(1 - progress, 2);

            Vector3 offset = Random.insideUnitSphere * currentStrength;
            offset.z = 0;
            transform.position = transform.position + offset;

            elapsed += Time.deltaTime;
            yield return null;
        }
    }


    void LateUpdate()
    {
       // if (le_ball == null) return;

        // Calculate vertical distance
    
            // Only move camera upward if target is more than threshold above camera
            if (le_ball.transform.position.y > transform.position.y - offest)
            {

                if (!game_manager_scr.game_started)
                {
                    return;
                }

                if (game_manager_scr.ball_attached)
                {
                    return ;
                }

                // New camera Y position, so that camera stays within threshold
                float targetCameraY = le_ball.transform.position.y + 15;
                Vector3 newPosition = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, le_ball.transform.position.y + offest, Time.deltaTime * 10), transform.position.z);

                transform.position = newPosition;
            }
        
       
    }

    public void alarm_caller()
    {
        GetComponent<AudioSource>().PlayOneShot(danger_alarm);
        StartCoroutine(PulseAlpha(gray_filter.GetComponent<SpriteRenderer>(), 0.2f, 0.5f, 4));
    }

    IEnumerator PulseAlpha(SpriteRenderer sr, float n, float t, int m)
    {
        Color color = sr.color;
        float halfTime = t / 2f;

        for (int i = 0; i < m; i++)
        {
            // Up: 0 -> n
            for (float a = 0; a < halfTime; a += Time.deltaTime)
            {
                float alpha = Mathf.Lerp(0f, n, a / halfTime);
                sr.color = new Color(color.r, color.g, color.b, alpha);
                yield return null;
            }

            // On last cycle, STOP at n
            if (i == m - 1)
            {
                sr.color = new Color(color.r, color.g, color.b, 0);
                yield break;
            }

            // Down: n -> 0
            for (float a = 0; a < halfTime; a += Time.deltaTime)
            {
                float alpha = Mathf.Lerp(n, 0f, a / halfTime);
                sr.color = new Color(color.r, color.g, color.b, alpha);
                yield return null;
            }
        }
    }

}