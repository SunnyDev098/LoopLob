using UnityEngine;
using Firebase.Firestore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class MessageSender_scr : MonoBehaviour
{
    // Save a message to Firestore
    public static async Task SaveMessage(string username, string messageText)
    {
        var firestore = FirebaseFirestore.DefaultInstance;
        var messagesCol = firestore.Collection("messages");
        messageText = username + " says: " + messageText;
        // Build the data dictionary
        var data = new Dictionary<string, object>()
        {
            { "username", username },
            { "message", messageText },
            { "timestamp", FieldValue.ServerTimestamp }
        };

        try
        {
            // Add the message (auto-generates a new doc ID)
            await messagesCol.AddAsync(data);

            Debug.Log("message_send");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("[Firestore] Failed to save message: " + ex.Message);
        }
    }
}
