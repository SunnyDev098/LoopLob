using UnityEngine;

public class laser_aoe_scr : MonoBehaviour
{
    ParticleSystem ps;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        var main = ps.main;
        main.loop = false; // Disable looping
      //  main.duration = 2; // Set duration to freeze time
        ps.Play();
    }

    // Update is called once per frame
    void Update()
    {
        ps.Simulate(Time.unscaledDeltaTime, true, false); // Manually advance time
        ps.time = 1 % ps.main.duration; // Clamp time within duration
    }
}
