using System.Text;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;
using TMPro;
using UnityEngine.Windows;
using System.Linq;

public class message_fetcher_scr : MonoBehaviour
{

   public TextMeshProUGUI message;
    FirebaseFirestore db;

    void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
        FetchLastFiveMessagesAsync();
    }

    // Async version
    public async Task FetchLastFiveMessagesAsync()
    {
        try
        {
              var query = db.Collection("messages")
             .OrderByDescending("timestamp")
             .Limit(5);

            QuerySnapshot snapshot = await query.GetSnapshotAsync();

            Dictionary<string, string> userMessageDict = new Dictionary<string, string>();
            // For duplicate usernames, you might want a List<>, but with 5 you're probably good.

            foreach (var doc in snapshot.Documents)
            {
                if (doc.TryGetValue("message", out string message))
                {
                    int idx = message.IndexOf("says");
                    if (idx >= 0)
                    {
                        // Username is before "says"
                        string username = message.Substring(0, idx).Trim();

                        // Message is after "says"
                        string afterSays = message.Substring(idx + "says".Length).TrimStart(':', ' ', '-', '　');

                        // Add to dict (handle duplicate username if needed)
                        userMessageDict[username] = afterSays;
                    }
                    else
                    {
                        // If "says" not found, could skip or use entire message as username or msg
                        userMessageDict[message] = "";
                    }
                }
            }


            Debug.Log("messages_collected");
            await Task.Delay(1000);


            game_manager_scr.global_userMessageDic = userMessageDict;
            game_manager_scr.ghost_messages_collected = true;
            // You can return lastFive if you want to use it elsewhere
            // return lastFive;

            message.text = userMessageDict.Keys.First()+ userMessageDict.Values.First();
        }   
        catch (System.Exception ex)
        {
            Debug.LogError("Error fetching last five messages: " + ex.Message);

            game_manager_scr.global_userMessageDic.Clear();
            game_manager_scr.ghost_messages_collected = false;
        }
    }
}
