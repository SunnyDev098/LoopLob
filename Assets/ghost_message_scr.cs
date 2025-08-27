using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;
using System;
using System.Collections.Generic;
using TMPro;

public class ghost_message_scr : MonoBehaviour
{

    FirebaseFirestore db;

    void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
    }

    public void SubmitDeathMessage(int death_height,string the_message)
    {
        string messageText = the_message;

        // Enforce character limit
        if (messageText.Length > 60)
        {
            messageText = messageText.Substring(0, 60);
        }

        if (string.IsNullOrEmpty(messageText))
        {
            Debug.LogWarning("Death message is empty!");
            return;
        }

        // Create the document data
        Dictionary<string, object> deathMessage = new Dictionary<string, object>
        {
            { "text", messageText },
            { "timestamp", FieldValue.ServerTimestamp },
            { "height", death_height },
            { "writer_username", PlayerPrefs.GetString("username") }
        };

        // Add to Firestore
        db.Collection("death_messages").AddAsync(deathMessage).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && !task.IsFaulted && !task.IsCanceled)
            {
                Debug.Log("Death message submitted successfully!");
                the_message = string.Empty; // Clear input
            }
            else
            {
                Debug.LogError("Error submitting death message: " + task.Exception);
            }
        });
    }
}
