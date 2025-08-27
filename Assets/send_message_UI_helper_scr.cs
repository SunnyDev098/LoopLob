using UnityEngine;
using TMPro; // <<--- ADD THIS
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Firebase.Firestore;
public class send_message_ui_helper_scr : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text message_InputField; // TextMeshPro version!!
    public UnityEngine.UI.Button claimButton;
    public TMP_Text errorText; // TextMeshPro for errors
    public TMP_Text user_name; 

  
   

    void Start()
    {
        claimButton.onClick.AddListener(OnClaimClicked);
        errorText.text = "";
        user_name.text = PlayerPrefs.GetString("username", "unknown") + " says:";

    }

    async void OnClaimClicked()
    {
        string username = PlayerPrefs.GetString("username", "unknown");

        // Call this when submitting a chat message, e.g. on button click
        await MessageSender_scr.SaveMessage(username, message_InputField.text);
    }






}
