using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;
using TMPro; // For TextMeshPro support (remove if not using TMP)

public class GhostMessageFetcher : MonoBehaviour
{
    FirebaseFirestore db;

    [Header("Prefab & Settings")]
    public GameObject deathMessageBillboardPrefab;  // Assign your prefab in Inspector
    public Vector3 spawnBasePosition = Vector3.zero; // Base spawn position (height offset will be added)
  //  public Transform parentContainer;               // Optional: parent for billboards

    void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
        GetLast10Messages();
    }

    public void GetLast10Messages()
    {
        db.Collection("death_messages")
          .OrderByDescending("timestamp")  // <-- Works on Unity Firestore SDK
          .Limit(10)
          .GetSnapshotAsync()
          .ContinueWithOnMainThread(task =>
          {
              if (task.IsFaulted)
              {
                  Debug.LogError("Error getting death messages: " + task.Exception);
                  return;
              }

              QuerySnapshot snapshot = task.Result;

              if (snapshot.Count == 0)
              {
                  Debug.Log("No death messages found.");
                  return;
              }

              foreach (DocumentSnapshot doc in snapshot.Documents)
              {
                  string text = doc.ContainsField("text") ? doc.GetValue<string>("text") : "N/A";
                  string username = doc.ContainsField("writer_username") ? doc.GetValue<string>("writer_username") : "Unknown";
                  int height = doc.ContainsField("height") ? doc.GetValue<int>("height") : 0;
                  Debug.Log(text);
                  // Instantiate the billboard at position = base + height offset
                  GameObject billboard = Instantiate(
                      deathMessageBillboardPrefab,
                      spawnBasePosition + Vector3.up * height,
                      Quaternion.identity
                  );

                  // Set username text
                  Transform usernameTf = billboard.transform.GetChild(5);
                  if (usernameTf != null)
                  {
                      // TextMeshPro
                      TextMeshProUGUI tmp = usernameTf.GetComponent<TextMeshProUGUI>();
                      if (tmp != null) tmp.text = username;

                   
                  }

                  // Set message text
                  Transform messageTf = billboard.transform.GetChild(6);
                  if (messageTf != null)
                  {
                      // TextMeshPro
                      TextMeshProUGUI message = messageTf.GetComponent<TextMeshProUGUI>();
                      if (message != null) message.text = text;

                      // Legacy UI Text
                      UnityEngine.UI.Text uiText = messageTf.GetComponent<UnityEngine.UI.Text>();
                      if (uiText != null) uiText.text = text;
                  }

                  Debug.Log($"Spawned billboard for: {username} - '{text}' at height {height}");
              }
          });
    }
}
