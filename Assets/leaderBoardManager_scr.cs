using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Firestore;
using TMPro;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour
{
   
    public string UsernameKey = "username";      // PlayerPrefs key for username
    public string BestScoreKey = "best_score";   // PlayerPrefs key for best score
    FirebaseFirestore db;


    public TextMeshProUGUI first_name;
    public TextMeshProUGUI first_score;

    public TextMeshProUGUI second_name;
    public TextMeshProUGUI second_score;

    public TextMeshProUGUI third_name;
    public TextMeshProUGUI third_score;


    public TextMeshProUGUI user_name;
    public TextMeshProUGUI user_score;
    public TextMeshProUGUI user_ranking;


    void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
    }

    /// <summary>
    /// Submits current best score, then fetches:
    /// - topper_nums: how many users have a higher score
    /// - Top 3 usernames & their scores
    /// Calls DoMethodA on success, DoMethodB on fail
    /// </summary>
    // ... (all usings & class declarations remain)
    public async Task SubmitAndGetTopInfo()
    {
        try
        {
            string username = PlayerPrefs.GetString(UsernameKey, null);
            if (string.IsNullOrEmpty(username))
                throw new System.Exception("Username not set in PlayerPrefs.");
            username = username.ToLowerInvariant();

            int bestScore = PlayerPrefs.HasKey(BestScoreKey) ? PlayerPrefs.GetInt(BestScoreKey) : 0;
            if (!PlayerPrefs.HasKey(BestScoreKey))
            {
                PlayerPrefs.SetInt(BestScoreKey, 0);
                PlayerPrefs.Save();
            }

            // Update user's score
            DocumentReference userDoc = db.Collection("usernames").Document(username);
            Dictionary<string, object> updateData = new Dictionary<string, object>
        {
            { "best_score", bestScore }
        };
            await userDoc.SetAsync(updateData, SetOptions.MergeAll);

            // MANUAL COUNT: How many users scored higher?
            var snap = await db.Collection("usernames")
                               .WhereGreaterThan("best_score", bestScore)
                               .GetSnapshotAsync();
            int topper_nums = snap.Count;

            // Fetch Top 3 users by score
            var top3Query = db.Collection("usernames")
                              .OrderByDescending("best_score")
                              .Limit(3);
            QuerySnapshot top3Snap = await top3Query.GetSnapshotAsync();

            var topNames = new List<string>();
            var topScores = new List<int>();
            foreach (var doc in top3Snap.Documents)
            {
                string uname = doc.Id;
                int score = doc.ContainsField("best_score") ? doc.GetValue<int>("best_score") : 0;
                topNames.Add(uname);
                topScores.Add(score);
            }

            // Success
            DoMethodA(topper_nums, topNames, topScores);
            GetComponent<start_scene_manager>().leader_board_success = true;
        }
        catch (System.Exception ex)
        {
            DoMethodB(ex);
            GetComponent<start_scene_manager>().leader_board_success = false;

        }
    }

    // Success handler
    void DoMethodA(int topper_nums, List<string> topNames, List<int> topScores)
    {
        Debug.Log($"Topper count: {topper_nums}");
        for (int i = 0; i < topNames.Count; i++)
        {
            Debug.Log($"TOP {i + 1}: {topNames[i]} - {topScores[i]}");
        }

        first_name.text = topNames[0];
        first_score.text = topScores[0].ToString();

        second_name.text = topNames[1];
        second_score.text = topScores[1].ToString();



        third_name.text = topNames[2];
        third_score.text = topScores[2].ToString();



        user_name.text = PlayerPrefs.GetString("username");
        user_score.text = PlayerPrefs.GetInt("best_score").ToString();

        user_ranking.text = (topper_nums+1 ).ToString();

        // Your success logic here (UI update, etc)
    }

    // Failure handler
    void DoMethodB(System.Exception ex)
    {
        Debug.LogError("Leaderboard error: " + ex);
        // Your error logic here (UI message, retry, etc)
    }
}
