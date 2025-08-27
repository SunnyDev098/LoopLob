using UnityEngine;

public class ColorCallbackReceiver : MonoBehaviour
{
    public GameObject main_parent;

    [SerializeField] private BurstColorTransition _colorTransition;
    [SerializeField] private ParticleSystem _particles;

    private void OnEnable()
    {
        _colorTransition.OnColorChanged += HandleColorChange;
        _colorTransition.OnTransitionComplete += HandleComplete;
    }

    private void OnDisable()
    {
        _colorTransition.OnColorChanged -= HandleColorChange;
        _colorTransition.OnTransitionComplete -= HandleComplete;
    }

    private void HandleColorChange(Color currentColor)
    {
       // Debug.Log($"Color changed to: {currentColor}");

        // Example: Change particle color when reaching orange
        if (currentColor.r > 0.9f && currentColor.g < 0.6f)
        {
            var main = _particles.main;
            main.startColor = currentColor;
        }
    }

    private void HandleComplete()
    {
        Debug.Log("Transition complete!");
        GetComponent<AudioSource>().Play();



        if (!game_manager_scr.no_death)
        {
            main_parent.GetComponent<ball_scr>().gege();

        }
    }
}