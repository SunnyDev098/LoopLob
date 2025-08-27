using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Collections;
using System.Text;

public class SendNotificationToWatero2 : MonoBehaviour
{
    // ✅ URL from your Gen 2 Cloud Function deployment
    private const string FunctionUrl = "https://sendusernotificationgege-3xveygq2aa-uc.a.run.app";

    // Auto-trigger for testing
    private void Start()
    {
        TriggerNotification();
    }

    // Can be linked to UI button
    public void TriggerNotification()
    {
        StartCoroutine(SendNotification());
    }

    private IEnumerator SendNotification()
    {
        // ✅ Make payload exactly match expected keys in function
        var payload = new
        {
            username = "ahmad",  // Must exactly match Firestore doc ID in `usernames` collection
            title = "salam joje",
            body = "chetori"
        };

        // ✅ Serialize with Newtonsoft.Json to avoid invalid formatting
        string jsonData = JsonConvert.SerializeObject(payload);
        Debug.Log("[Unity] Sending payload: " + jsonData);

        // ✅ Create POST request to your function
        using (UnityWebRequest request = new UnityWebRequest(FunctionUrl, UnityWebRequest.kHttpVerbPOST))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            // ✅ Send the request and wait for completion
            yield return request.SendWebRequest();

            // ✅ Handle Unity-side result
            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("[Unity] Notification sent! Server response: " + request.downloadHandler.text);
            }
            else
            {
                Debug.LogError("[Unity] Error sending notification: " + request.error);
                Debug.LogError("[Unity] Server response: " + request.downloadHandler.text);
            }
        }
    }
}
