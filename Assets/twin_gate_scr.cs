using UnityEngine;

public class twin_gate_scr : MonoBehaviour
{
    [Tooltip("Cooldown to avoid instant ping-pong")]
  //  public float cooldown = 0.5f;

    private static readonly System.Collections.Generic.Dictionary<Transform, float> lastTeleportTime
        = new System.Collections.Generic.Dictionary<Transform, float>();

    private void OnTriggerEnter2D(Collider2D other)
    {

        Debug.Log("dadw");
        if (!other.gameObject.CompareTag("ball"))
        {
            return;
        }
        /*
        // Cooldown check
        if (lastTeleportTime.TryGetValue(other.transform, out float lastTime)
            && Time.time - lastTime < 0.5f)
        {
            return;

        }
        */
        // Find sibling gate (the other child)
        Transform parent = transform.parent;
        if (parent == null || parent.childCount < 2)
        {
            Debug.LogWarning("TwinGateChild needs a parent with at least two children.");
            return;
        }

        Transform twinGate = null;
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child != transform) // skip self
            {
                twinGate = child;
                break;
            }
        }

        if (twinGate != null)
        {
            twinGate.GetComponent<BoxCollider2D>().enabled = false;
            // Move object to twin's position
            other.transform.position = twinGate.position;

            // Apply cooldown to prevent instant looping
            lastTeleportTime[other.transform] = Time.time;
        }
    }
}
