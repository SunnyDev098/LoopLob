using UnityEngine;
using System.Collections;
public class shield_scr : MonoBehaviour
{

    private float start_time;
    public GameObject boom_vfx;
    public GameObject ball;
    private void OnTriggerEnter2D(Collider2D other)
    {


        if (other.CompareTag("spike") || other.CompareTag("alien_wandering") || other.CompareTag("laser") || other.CompareTag("missile") || other.CompareTag("beam_emitter"))
        {

            if (game_manager_scr.ball_attached)
            {
                return;
            }
            Debug.Log(other.gameObject.name);

            GameObject moob_go = Instantiate(boom_vfx, other.transform.position, Quaternion.identity);
            Destroy(moob_go, 0.8f);

            Destroy(other.gameObject);

            StartCoroutine(end_shield());
            
        }

    }

    private void OnEnable()
    {
        ball.GetComponent<ball_scr>().shielded = true;

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        start_time = Time.time ;

    }

    // Update is called once per frame
    void Update()
    {
        
       
    }

    public void set_time()
    {
        start_time = Time.time ;
    }
    IEnumerator end_shield( )
    {
        var gege = GetComponent<SpriteRenderer>();


        
        gege.enabled = false;

        yield return new WaitForSeconds(0.2f);
        gege.enabled = true;
        yield return new WaitForSeconds(0.2f);

        gege.enabled = false;

        yield return new WaitForSeconds(0.2f);
        gege.enabled = true;
        yield return new WaitForSeconds(0.2f);


        gege.enabled = false;

        yield return new WaitForSeconds(0.2f);
        gege.enabled = true;
        yield return new WaitForSeconds(0.8f);
        ball.GetComponent<ball_scr>().shielded = false;
        gameObject.SetActive(false);
        game_manager_scr.is_shield_active = false;


        yield return null;

    }

}
