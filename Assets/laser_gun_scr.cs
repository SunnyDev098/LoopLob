using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class laser_gun_scr : MonoBehaviour
{
    private AudioSource audioSource;

   public AudioClip laser_shot_sound;
    public bool is_left;
    private int defined_y;
    Vector3 offset = new Vector3(0, 0, 1);
    [Header("Rotation Settings")]
    public float minAngle = -25f;
    public float maxAngle = 25f;
    public float rotationSpeed = 40f;         // degrees per second

    [Header("Shooting Settings")]
    public GameObject projectilePrefab;       // Your projectile prefab
    public float projectileSpeed = 12f;       // Units per second
    public float timeBetweenShots = 0.15f;    // Time between 3 shots in one burst

    private float targetAngle;
    private bool rotating = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        GetComponent<AudioSource>().volume = game_manager_scr.sfx_volume * 3;

        StartCoroutine(RotateAndShootRoutine());

        if (is_left)
        {
            defined_y = 0;
        }
        else
        {
            defined_y = 180;
        }
    }

    IEnumerator RotateAndShootRoutine()
    {
        while (true)
        {
            // 1. Pick a new target angle within range
            targetAngle = Random.Range(minAngle, maxAngle);
            // 2. Smoothly rotate towards target angle
            yield return StartCoroutine(RotateToAngle(targetAngle));
            // 3. Shoot 3 projectiles in succession with interval
            for (int i = 0; i < 1; i++)
            {
                FireProjectile();
                yield return new WaitForSeconds(timeBetweenShots);
            }
        }
    }

    IEnumerator RotateToAngle(float target)
    {
        rotating = true;

        yield return new WaitForSeconds(0.5f);
        float current = NormalizeAngle(transform.eulerAngles.z);
        while (Mathf.Abs(Mathf.DeltaAngle(current, target)) > 1f)
        {
            current = NormalizeAngle(transform.eulerAngles.z);
            float step = rotationSpeed * Time.deltaTime;
            float newAngle = Mathf.MoveTowardsAngle(current, target, step);
            transform.eulerAngles = new Vector3(0, defined_y, newAngle);
            yield return null;
        }
        transform.eulerAngles = new Vector3(0, defined_y, target); // Snap exactly
        rotating = false;
    }

    void FireProjectile()
    {
        // Spawn projectile at gun's position, with gun's current rotation
        GameObject proj = Instantiate(projectilePrefab, transform.position + offset, transform.rotation);
        audioSource.PlayOneShot(laser_shot_sound);

        // Move it without attaching a script: directly set velocity via Rigidbody2D if available,
        // else fallback to manual velocity update via coroutine
        Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
        Vector2 shootDir = transform.right; // Right is the "forward" direction in Unity 2D

        if (rb != null)
        {
            rb.linearVelocity = shootDir * projectileSpeed;
        }
        else
        {
            // If no Rigidbody, move manually via coroutine:
            StartCoroutine(MoveAndDestroy(proj, shootDir));
        }

        // Destroy after 5 seconds even if it gets stuck
        Destroy(proj, 5f);
    }

    IEnumerator MoveAndDestroy(GameObject obj, Vector2 direction)
    {
        float timer = 0f;
        while (timer < 5f && obj != null)
        {
            obj.transform.position += (Vector3)(direction * projectileSpeed * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }
    }

    float NormalizeAngle(float angle)
    {
        angle = angle % 360f;
        if (angle > 180f) angle -= 360f;
        return angle;
    }
}
