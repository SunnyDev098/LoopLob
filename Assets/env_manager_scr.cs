using UnityEditor;
using UnityEngine;
using System.Collections;
public class env_manager_scr : MonoBehaviour
{


    public Material[] sky_mats_list;
    public float targetExposure = 1.0f; // The desired exposure value
    public float duration = 2.0f; // Time it takes to adjust exposure
    private Material currentSkyboxMaterial;


    private void Start()
    {
        /*
        Material skyboxMaterial = AssetDatabase.LoadAssetAtPath<Material>("Assets/___sky_materials/DSGWP.mat");


        RenderSettings.skybox = skyboxMaterial;
        currentSkyboxMaterial = skyboxMaterial;
        RenderSettings.skybox.SetFloat("_Exposure", 2);
        */

    }
    private void Update()
    {
        /*
        

        if (Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log("gege");


            // Start gradually changing the exposure
            if (currentSkyboxMaterial != null)
            {
                StartCoroutine(minus_ChangeExposureOverTime( ));

                Debug.Log("gege");
            }

        }
        */
       
    }
    /*
    [MenuItem("Tools/Set New Skybox")]
    public  void SetNewSkybox()
    {
        // Load a Skybox Material from Resources or Assets
        Material skyboxMaterial = AssetDatabase.LoadAssetAtPath<Material>("Assets/sky_materials/DSB.mat");

        if (skyboxMaterial != null)
        {

            Debug.Log("not null sky");

            // Assign it to RenderSettings
            RenderSettings.skybox = skyboxMaterial;

            // Update the GI
            DynamicGI.UpdateEnvironment();
        }
        else
        {
            Debug.LogError("Skybox material not found!");
        }
    }

    IEnumerator ChangeExposureOverTime(float target)
    {
        if (currentSkyboxMaterial.HasProperty("_Exposure"))
        {
            float initialExposure = 0;
            float elapsedTime = 0f;
            duration = 0.1f;
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float newExposure = Mathf.Lerp(initialExposure, target, elapsedTime / duration);
                currentSkyboxMaterial.SetFloat("_Exposure", newExposure);
                yield return null; // Wait until the next frame
            }

            // Ensure the final value is exactly the target
            currentSkyboxMaterial.SetFloat("_Exposure", target);
        }

        else
        {
            Debug.LogError("The Skybox material does not have an _Exposure property!");
        }

    }


    IEnumerator minus_ChangeExposureOverTime()
    {
       
            float initialExposure = currentSkyboxMaterial.GetFloat("_Exposure"); 
            float elapsedTime = 0f;

          var duration = 0.1f;
         
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float newExposure = Mathf.Lerp(initialExposure, 0, elapsedTime / duration);
                currentSkyboxMaterial.SetFloat("_Exposure", newExposure);
                yield return null; // Wait until the next frame
            }

            // Ensure the final value is exactly the target
            currentSkyboxMaterial.SetFloat("_Exposure", 0);
        game_manager_scr.current_game_level = game_manager_scr.current_game_level + 1;
        RenderSettings.skybox = sky_mats_list[game_manager_scr.current_game_level];
        currentSkyboxMaterial = sky_mats_list[game_manager_scr.current_game_level];
        currentSkyboxMaterial.SetFloat("_Exposure", 0);

        var target_exppsure = 2 ;

        if (game_manager_scr.current_game_level == 0)
        {
            target_exppsure = 2;

        }
        else if(game_manager_scr.current_game_level == 1)
        {
            target_exppsure = 2;

        }

        else if (game_manager_scr.current_game_level == 2)
        {
            target_exppsure = 2;

        }


        else if (game_manager_scr.current_game_level == 3)
        {
            target_exppsure = 2;

        }


        else if (game_manager_scr.current_game_level == 4)
        {
            target_exppsure = 2;

        }

        else if (game_manager_scr.current_game_level == 5)
        {
            target_exppsure = 2;

        }
        else if (game_manager_scr.current_game_level == 6)
        {
            target_exppsure = 2;

        }




        StartCoroutine(ChangeExposureOverTime(target_exppsure));
       
       
    }


    public void go_next_level()
    {
        StartCoroutine(minus_ChangeExposureOverTime());

    }
    */
}
