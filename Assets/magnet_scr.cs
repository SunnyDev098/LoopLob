using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class magnet_scr : MonoBehaviour
{
    public Transform the_ball;
    public TextMeshProUGUI coins_gui;

    [Header("Sound Settings")]
    public AudioClip pickupClip;
    public AudioSource[] audioSources; // Assign 2–3 in Inspector

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("coin"))
        {
            collision.gameObject.GetComponent<CircleCollider2D>().enabled = false;
            StartCoroutine(MoveTowardAndShrink(collision.gameObject, the_ball));
        }
    }

    public IEnumerator MoveTowardAndShrink(GameObject objectToMove, Transform target)
    {
        PlayPickupSound();

        float duration = 0.3f;

        if (objectToMove == null || target == null || duration <= 0f)
            yield break;

        Vector3 startPos = objectToMove.transform.position;
        Vector3 startScale = objectToMove.transform.localScale;
        Vector3 endScale = Vector3.zero;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            // Smooth easing
            float smoothT = t * t * (3f - 2f * t);

            // Track target's *moving* position
            Vector3 currentTargetPos = target.position;

            objectToMove.transform.position = Vector3.Lerp(startPos, currentTargetPos, smoothT);
            objectToMove.transform.localScale = Vector3.Lerp(startScale, endScale, smoothT);

            yield return null;
        }

        // Final snap
        objectToMove.transform.position = target.position;
        objectToMove.transform.localScale = endScale;

        // Update coins
        game_manager_scr.coin_number++;
        coins_gui.text = game_manager_scr.coin_number.ToString();

        // Play pickup sound

        Destroy(objectToMove, 2);
    }

    private void PlayPickupSound()
    {
        if (pickupClip == null || audioSources == null || audioSources.Length == 0)
            return;

        // Find a free source, or interrupt the first one if none free
        foreach (AudioSource src in audioSources)
        {
            if (!src.isPlaying)
            {
                src.PlayOneShot(pickupClip);
                return;
            }
        }

        // If all are busy, force play on the first
        audioSources[0].PlayOneShot(pickupClip);
    }
}
