using UnityEngine;

public class OrbitController : MonoBehaviour
{
    private planet_attribute_scr planetAttributes; // Cached reference to the parent's PlanetAttributes component

    void Start()
    {
        // Cache the PlanetAttributes component from the parent object
        planetAttributes = GetComponentInParent<planet_attribute_scr>();
        if (planetAttributes == null)
        {
            Debug.LogError("Parent object does not have a PlanetAttributes component!", this);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collided object has the "orbit" tag
        if (collision.CompareTag("orbit"))
        {
            if (planetAttributes != null)
            {
                // Toggle the movement direction of the parent planet
                planetAttributes.ToggleMovementDirection();
            }
            else
            {
                Debug.LogWarning("PlanetAttributes reference is missing!", this);
            }
        }
    }
}