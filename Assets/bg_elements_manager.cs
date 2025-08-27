using UnityEngine;
using System.Collections.Generic;
using UnityEngine.TextCore.Text;

public class ObjectSpawner : MonoBehaviour
{
    public List<Sprite> objectsToSpawn;
    public GameObject sky_object_platform;

    public GameObject the_parent;
    public GameObject le_ball;
    public float spawnInterval = 25f;

    public bool startOnAwake = true;

    private float timer;
    private int currentIndex = 0;

    void Start()
    {
    
       // SpawnNextObject();
    }

    void Update()
    {


  

        timer += Time.deltaTime;

        if (le_ball.transform.position.y > spawnInterval )
        {

            if(currentIndex < 20)
            {
                SpawnNextObject();
                ;

            }
            else
            {
                spawnInterval = 1000000000;
            }
       
        }
    }

    void SpawnNextObject()
    {



        
        var the_x = Random.RandomRange(-4, 4);


        Vector3 the_pos =   new Vector3(the_x, le_ball.transform.position.y+ 50, 30 );
       
       
        // Instantiate the current object
        GameObject the_maded_object =   Instantiate(sky_object_platform, the_pos, Quaternion.identity,transform );

        the_maded_object.transform.localScale = the_maded_object.transform.localScale * Random.RandomRange(0.7f ,1f);

        the_maded_object.GetComponent<SpriteRenderer>().sprite = objectsToSpawn[currentIndex];
        // Move to next object in list (loop back to start if needed)
        currentIndex = currentIndex + 1;

        spawnInterval = le_ball.transform.position.y + currentIndex * 250;


    }

   

   
    
}