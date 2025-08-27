using UnityEngine;

public class start_scene_cam_scr : MonoBehaviour
{

  

    private float originalOrthoSize; // Works for 9:18 (aspect ratio 0.5)
    private float originalAspectRatio = 0.5f; // 9:18


    private Vector3 velocity = Vector3.zero;
    private float screen_height;

    private void Start()
    {
        originalOrthoSize = Camera.main.orthographicSize;






        float newAspectRatio = (float)Screen.width / Screen.height;
        float newOrthoSize = originalOrthoSize * (originalAspectRatio / newAspectRatio);
        Camera.main.orthographicSize = newOrthoSize;













    }



}