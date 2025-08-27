using UnityEngine;
using Firebase.Auth;
using Firebase.Firestore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class UsernameClaim : MonoBehaviour
{
    public GameObject pop_up;
    public GameObject auth_scene_go;
    public GameObject start_scene_go;

    // Cleans username: removes all whitespace, zero-width chars, then lowercases
    private string CleanUsername(string input)
    {
        if (string.IsNullOrEmpty(input)) return string.Empty;

        string cleaned = new string(
            input
                .Where(c =>
                    !char.IsWhiteSpace(c) &&   // spaces, tabs, etc.
                    c != '\u200B' &&           // zero width space
                    c != '\u200C' &&           // zero width non-joiner
                    c != '\u200D' &&           // zero width joiner
                    c != '\uFEFF'              // zero width no-break space (BOM)
                )
                .ToArray()
        );

        return cleaned.ToLowerInvariant();
    }

    public async Task<bool> CheckAndClaimUsername(string enteredName)
    {
        string username = CleanUsername(enteredName);

        // Debug output of raw codepoints (helps verify hidden char issues)
        Debug.Log("[UsernameClaim] Attempting to claim username: " + username);
        Debug.Log("[UsernameClaim] Codepoints: " +
                  string.Join(" ", username.Select(c => $"U+{(int)c:X4}")));

        var auth = FirebaseAuth.DefaultInstance;

        // Sign in anonymously if no current user
        if (auth.CurrentUser == null)
        {
            Debug.Log("[UsernameClaim] No current user. Attempting anonymous sign-in...");
            try
            {
                await auth.SignInAnonymouslyAsync();
                Debug.Log($"[UsernameClaim] Signed in! UID: {auth.CurrentUser?.UserId ?? "null"}");
            }
            catch (System.Exception ex)
            {
                Debug.LogError("[FirebaseAuth] SignInAnonymouslyAsync failed: " + ex);
                return false;
            }
        }

        var firestore = FirebaseFirestore.DefaultInstance;
        var docRef = firestore.Collection("usernames").Document(username);

        // Check if username is already taken
        DocumentSnapshot snapshot;
        try
        {
            snapshot = await docRef.GetSnapshotAsync();
        }
        catch (System.Exception ex)
        {
            Debug.LogError("[Firestore] GetSnapshotAsync failed: " + ex);
            return false;
        }

        if (snapshot.Exists)
        {
            Debug.Log("[UsernameClaim] Username taken: " + username);
            return false;
        }

        // Get FCM token
        string token = firebase_start_scr.CurrentFcmToken ?? "";
        if (string.IsNullOrEmpty(token))
        {
            Debug.LogWarning("[UsernameClaim] No cached FCM token — saving without token.");
        }
        else
        {
            Debug.Log("[UsernameClaim] Using cached FCM token: " + token);
        }

        // Prepare Firestore data with FCM token array
        var data = new Dictionary<string, object>()
        {
            { "uid", auth.CurrentUser.UserId },
            { "timestamp", FieldValue.ServerTimestamp },
            { "fcmTokens", string.IsNullOrEmpty(token) ? new List<string>() : new List<string> { token } }
        };

        try
        {
            await docRef.SetAsync(data);
            Debug.Log($"[UsernameClaim] Username claimed: {username} (UID: {auth.CurrentUser.UserId})");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("[Firestore] SetAsync failed: " + ex);
            return false;
        }

        // Save username locally
        PlayerPrefs.SetString("username", username);
        PlayerPrefs.Save();

        // Show success popup and change scenes
        if (pop_up) pop_up.gameObject.GetComponent<UsernamePanel>().errorText.text = "Username saved!";
        await Task.Delay(1000);

        if (auth_scene_go) auth_scene_go.SetActive(false);
        if (start_scene_go) start_scene_go.SetActive(true);

        return true;
    }
}
