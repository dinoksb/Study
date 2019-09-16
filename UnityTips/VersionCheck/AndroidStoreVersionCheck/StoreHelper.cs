using UnityEngine;
using System.Net;
using UniRx;
using System.Text.RegularExpressions;
using System;
using System.Collections;

/// <summary>
/// This script is for checking the version in the store of the selected market
/// </summary>
public class StoreHelper : MonoBehaviour
{
    [HideInInspector]
    public string current_store_version;
    [HideInInspector]
    public string store_url;
    private bool is_quit = false;

    
    public void GoToStore()
    {
        is_quit = true;
        Application.Quit();
    }

    void OnApplicationQuit()
    {
        if (!is_quit) return;

#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
		AndroidJavaClass uri = new AndroidJavaClass("android.net.Uri");
		AndroidJavaObject uridata = uri.CallStatic<AndroidJavaObject>("parse", "market://details?id=com.skt.massivear");
		var intent = new AndroidJavaObject("android.content.Intent");
		intent.Call<AndroidJavaObject>("setAction", intent.GetStatic<string>("ACTION_VIEW"));
		intent.Call<AndroidJavaObject>("setData", uridata);
		jo.Call("startActivity", intent);
#endif
    }

    // Google Play Store version check coroutine
    public IEnumerator GetGoogleStoreVersion(Action<bool> result_flag)
    {
        ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

        bool isDone = false;

        var marketUrl = "https://play.google.com/store/apps/details?id=com.skt.massivear&hl=en";
        var disposable = ObservableWWW.GetWWW(marketUrl)
            .Timeout(TimeSpan.FromMilliseconds(3000))
            .CatchIgnore((WWWErrorException ex) =>
            {
                FirebaseAnalyticsController.LogEvent("MarketVersionAquireFail", "errMsg", ex.RawErrorMessage);
                Debug.LogError(ex.RawErrorMessage);
                current_store_version = Application.version;
                result_flag(true);
                isDone = true;
            })
            .Subscribe(www =>
            {
                if (string.IsNullOrEmpty(www.error))
                {
                    string result_text = www.text;

                    var rx = new Regex(@"(?<=""htlgb"">)(\d{1,3}\.\d{1,3}\.{0,1}\d{0,3})(?=<\/span>)", RegexOptions.Compiled);
                    MatchCollection matches = rx.Matches(result_text);
                    current_store_version = matches.Count > 0 ? matches[0].Value : Application.version;
                    result_flag(true);
                    isDone = true;
                }
                else
                {
                    FirebaseAnalyticsController.LogEvent("MarketVersionAquireFail", "errMsg", www.error);

                    Debug.LogError("Version Check Error : " + www.error);
                    current_store_version = Application.version;
                    result_flag(true);
                    isDone = true;
                }
            }
            ,(error) =>
            {
                FirebaseAnalyticsController.LogEvent("MarketVersionAquireFail", "errMsg", error.Message);

                Debug.LogError(error);
                current_store_version = Application.version;
                result_flag(true);
                isDone = true;
            });

        while (!isDone)
        {
            yield return null;
        }
    }
}