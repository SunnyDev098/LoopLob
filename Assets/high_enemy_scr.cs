using UnityEngine;
using System.Collections;
using UnityEditor;
public class high_enemy_scr : MonoBehaviour
{
    public   AudioSource m_AudioSource;
    public AudioClip warn_sign_sfx;

    public GameObject missile_sign;
    public GameObject missile;
    public GameObject the_ball;


    public GameObject top_bar;
    public GameObject left_bar;
    public GameObject right_bar;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_AudioSource.volume =  game_manager_scr.sfx_volume * 3;


       // Debug.Log("it _wroks");
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {


        }
    }



    public  IEnumerator lunch_missile()
    {
        float interval = 0.12f;

        float the_x = Random.Range(game_manager_scr.left_bar_x +1, game_manager_scr.right_bar_x - 1);

        GameObject maded_sign =     Instantiate(missile_sign, top_bar.transform.position, Quaternion.identity);
       // maded_sign .transform.position = new Vector3(the_x, top_bar.transform.position.y -0.9f,10);
        maded_sign .transform.position = new Vector3(the_x, top_bar.transform.position.y -0.9f,10);
        maded_sign.transform.parent = top_bar.transform;


        m_AudioSource.PlayOneShot(warn_sign_sfx);


        

        yield return new WaitForSeconds(interval);




        maded_sign.SetActive(false);



        yield return new WaitForSeconds(interval);
        maded_sign.SetActive(true);
         m_AudioSource.PlayOneShot(warn_sign_sfx);



        yield return new WaitForSeconds(interval);




        maded_sign.SetActive(false);



        yield return new WaitForSeconds(interval);
        maded_sign.SetActive(true);
         m_AudioSource.PlayOneShot(warn_sign_sfx);

        yield return new WaitForSeconds(interval);




        maded_sign.SetActive(false);



        yield return new WaitForSeconds(interval);
        maded_sign.SetActive(true);
         m_AudioSource.PlayOneShot(warn_sign_sfx);



        yield return new WaitForSeconds(interval);
        maded_sign.SetActive(false);





        GameObject maded_missile = Instantiate(missile, top_bar.transform.position, Quaternion.identity);
        maded_missile.transform.position = new Vector3(the_x, top_bar.transform.position.y +3, 10);
        maded_missile.transform.rotation = Quaternion.Euler(0, 0, 180);
        float elapsed = 0f;
        maded_missile.GetComponent<AudioSource>().volume = game_manager_scr.sfx_volume * 3;
        while (elapsed < 4f)
        {
            if (maded_missile != null)
            {
                maded_missile.transform.Translate(Vector3.up * 25f * Time.deltaTime);
            }
            elapsed += Time.deltaTime;

            yield return null; // Wait for next frame
        }
        if (maded_missile != null)
        {
            Destroy(maded_missile);


        }

        Destroy(maded_sign);


    }


    public void launcher_caller()
    {
            StartCoroutine(lunch_missile());
    }
}
