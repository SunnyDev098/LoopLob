using UnityEngine;

public class cam_scr : MonoBehaviour
{

    public GameObject top_bar;
    public GameObject bottom_bar;
    public GameObject left_bar;
    public GameObject right_bar;

    private float originalOrthoSize; // Works for 9:18 (aspect ratio 0.5)
    private float originalAspectRatio = 0.5f; // 9:18

    public float smoothTime = 0.6f;
    public float max_move_speed = 5;
    private Vector3 velocity = Vector3.zero;
    private float screen_height;

    private void Start()
    {
        originalOrthoSize = Camera.main.orthographicSize;






        float newAspectRatio = (float)Screen.width / Screen.height;
        float newOrthoSize = originalOrthoSize * (originalAspectRatio / newAspectRatio);
        Camera.main.orthographicSize = newOrthoSize;











        top_bar.transform.position = new Vector3(0, Camera.main.transform.position.y + Camera.main.orthographicSize, 10)    ;
        bottom_bar.transform.position = new Vector3(0, Camera.main.transform.position.y - Camera.main.orthographicSize, 10)    ;



        float camX = Camera.main.transform.position.x;
        float ortho = Camera.main.orthographicSize;
        float aspect = Camera.main.aspect;




        left_bar.transform.position = new Vector3(camX - ortho * aspect, 0, 10);
        game_manager_scr.left_bar_x = left_bar.transform.position.x +1;
      //  Debug.Log("left_bar_x" + game_manager_scr.left_bar_x);
        // RIGHT
        right_bar.transform.position = new Vector3(camX + ortho * aspect, 0, 10);
        game_manager_scr.right_bar_x = right_bar.transform.position.x -1;
      //  Debug.Log("right_bar_x" + game_manager_scr.right_bar_x);



    }



}