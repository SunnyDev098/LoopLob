using UnityEngine;
using System.Collections;
#if UNITY_IOS
using Unity.Notifications.iOS;
#endif
#if UNITY_ANDROID
using UnityEngine.Android;
#endif

public class NotificationPermissionManager : MonoBehaviour
{
    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.2f); // Let first frame render

#if UNITY_ANDROID
        if (GetAndroidSDKInt() >= 33 &&
            !Permission.HasUserAuthorizedPermission("android.permission.POST_NOTIFICATIONS"))
        {
            Permission.RequestUserPermission("android.permission.POST_NOTIFICATIONS");
        }
#elif UNITY_IOS
        var req = new AuthorizationRequest(
            AuthorizationOption.Alert | AuthorizationOption.Badge | AuthorizationOption.Sound, true);
        while (!req.IsFinished)
            yield return null;
        Debug.Log("iOS Notification Permission Result: " + req.Granted);
#endif
    }

#if UNITY_ANDROID
    private int GetAndroidSDKInt()
    {
        using (var versionClass = new AndroidJavaClass("android.os.Build$VERSION"))
        {
            return versionClass.GetStatic<int>("SDK_INT");
        }
    }
#endif
}
