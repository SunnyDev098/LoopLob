using UnityEngine;
using UnityEngine.UI;

public class toggle_no_death_scr : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        

        transform.GetComponent<Toggle>().onValueChanged.AddListener(OnToggleValueChanged) ;

    }

    // Update is called once per frame
    void Update()
    {
     
    }

    public void OnToggleValueChanged(bool isOn)
    {
        if (isOn)
        {
            // This code will run when the toggle is switched ON
            Debug.Log("Toggle is ON - Performing action!");
            game_manager_scr.no_death = true;
        }
        else
        {
            // Optional: Code for when toggle is switched OFF
            Debug.Log("Toggle is OFF");
        }
    }
}
