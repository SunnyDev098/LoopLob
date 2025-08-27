using UnityEngine;
using System.Collections;
public class difficulty_manager_scr : MonoBehaviour
{
  public  GameObject high_enemy_go;
  public  GameObject the_ball;

    private float  last_lunch;

    private float missile_interval;
    private int missile_number;

    private bool launch_on;
    void Start()
    {
        last_lunch = 0;
        missile_interval = 20;
        missile_number = 1;
        launch_on = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (the_ball.transform.position.y > 300)
        {

            if (last_lunch + missile_interval <Time.time)
            {

                if (launch_on && !game_manager_scr.ball_attached)
                {
                    StartCoroutine(command_lunch_missile());

                    launch_on =false;

                }
               

              
            }

        }


    }


    public IEnumerator command_lunch_missile()
    {


        for (int i =0; i<missile_number; i++)
        {
            high_enemy_go.GetComponent<high_enemy_scr>().launcher_caller();
            yield return new WaitForSeconds(1);
        }


        last_lunch = Time.time;


        if (the_ball.transform.position.y > 2000)
        {
            missile_interval = Random.RandomRange(5, 15);
            missile_number = Random.Range(4, 6);
        }

        else if (the_ball.transform.position.y > 1000)
        {
            missile_interval = Random.RandomRange(7, 20);
            missile_number = Random.Range(3, 5);
        }


        else {

            missile_interval = Random.RandomRange(10, 30);
            missile_number = Random.Range(2, 4);

        }


        launch_on =true;

    }


}
