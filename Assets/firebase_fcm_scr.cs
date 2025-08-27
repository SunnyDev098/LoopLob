using UnityEngine;
using Firebase;
using Firebase.Extensions;
using System.Threading.Tasks;
public class firebase_fcm_scr : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                Debug.Log("Firebase ready! FCM will now work.");
                // Now safe to call GetTokenAsync, etc.
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
                // Show user a warning, block further Firebase usage, etc.
            }
        });

    }

}
