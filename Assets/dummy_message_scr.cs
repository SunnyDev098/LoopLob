using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;
using System.Collections.Generic;

public class DummyDeathMessageCreator : MonoBehaviour
{
    FirebaseFirestore db;

    void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
        CreateDummyMessages();
    }

    [ContextMenu("CreateDummyMessages")] // Right-click script in Inspector to run in Editor
    public void CreateDummyMessages()
    {
        for (int i = 1; i <= 10; i++)
        {
            // Random height between 100 and 1000
            int height = Random.Range(100, 1001);

            string fakeUsername = "Player" + Random.Range(1, 1000);
            string fakeMessage = "Here lies " + fakeUsername + ", fallen at " + height + "m";

            Dictionary<string, object> deathMessage = new Dictionary<string, object>
            {
                { "text", fakeMessage },
                { "timestamp", FieldValue.ServerTimestamp },
                { "height", height },
                { "writer_username", fakeUsername }
            };

            db.Collection("death_messages").AddAsync(deathMessage).ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted && !task.IsFaulted && !task.IsCanceled)
                {
                    Debug.Log("Dummy death message added successfully!");
                }
                else
                {
                    Debug.LogError("Error adding dummy message: " + task.Exception);
                }
            });
        }
    }
}
