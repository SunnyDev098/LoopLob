using UnityEngine;
using TMPro; // <<--- ADD THIS
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Firebase.Firestore;
public class UsernamePanel : MonoBehaviour
{
    public GameObject auth_scene_go;
    public GameObject start_scene_go;

    [Header("UI References")]
    public TMP_Text usernameInputField; // TextMeshPro version!!
    public UnityEngine.UI.Button claimButton;
    public UnityEngine.UI.Button try_later_Button;
    public TMP_Text errorText; // TextMeshPro for errors

    [Header("Your Username Claim Script")]
    public UsernameClaim usernameClaim; // Reference your UsernameClaim script

    // ---- Username validation logic ----
    // Only a-z, A-Z, 0-9, and "_", 3-16 chars
    public static bool IsValidUsername(string input)
    {
        if (string.IsNullOrEmpty(input))
            return false;
        return Regex.IsMatch(input, @"^[a-zA-Z0-9_]{3,16}$");
    }

    void Start()
    {
        claimButton.onClick.AddListener(OnClaimClicked);
        try_later_Button.onClick.AddListener(try_later);
        errorText.text = "";
        /*
        // Live filter: removes invalid chars as they type
        usernameInputField.onValueChanged.AddListener((val) =>
        {
            string filtered = Regex.Replace(val, @"[^a-zA-Z0-9_]", "");
            if (filtered != val)
            {
                int caret = usernameInputField.stringPosition - (val.Length - filtered.Length);
                usernameInputField.text = filtered;
                usernameInputField.stringPosition = Mathf.Clamp(caret, 0, filtered.Length);
            }
            errorText.text = "";
        });
        */
    }
    async void try_later()
    {
        auth_scene_go.SetActive(false);
        start_scene_go.SetActive(true);

    }
        async void OnClaimClicked()
    {

        string nameToCheck = usernameInputField.text.Trim();
        
        /*
        
        if (!IsValidUsername(nameToCheck))
        {

            Debug.Log(nameToCheck);
            errorText.text = "Use 3-16 letters, numbers, or _ (no spaces!)";
            return;
        }
        */
        claimButton.interactable = false;
        bool success = await usernameClaim.CheckAndClaimUsername(nameToCheck);

        if (success)
        {
            errorText.text = "Username claimed!";
            // Next: Hide panel, start game, etc.
            gameObject.SetActive(false);
        }
        else
        {
            errorText.text = "That name is taken. Try another.";
        }
        claimButton.interactable = true;
    }






}
