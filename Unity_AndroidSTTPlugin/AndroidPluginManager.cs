using UnityEngine;

public delegate void delegateSTTResult(string result);
public delegate void delegateMessage(string msg);

public class AndroidPluginManager : MonoBehaviour
{
    private static AndroidPluginManager _instance;
    private static GameObject content;
    public static AndroidPluginManager getInstance()
    {
        if (_instance == null)
        {
            content = new GameObject();
            content.name = "pluginUnity"; // 오브젝트 이름 틀리면 안됩니다.
            _instance = content.AddComponent(typeof(AndroidPluginManager)) as AndroidPluginManager;
        }
        return _instance;
    }

    private AndroidJavaObject AJObject = null;
    private AndroidJavaClass AJClass = null;

    private delegateSTTResult sttResult;
    private delegateMessage message;

    private void OnDestroy()
    {
        _instance = null;
    }

    private void Awake()
    {
        Debug.Log("AJOject_0: " + AJObject);

#if UNITY_ANDROID && !UNITY_EDITOR
        AJClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AJObject = AJClass.GetStatic<AndroidJavaObject>("currentActivity");
#endif

        Debug.Log("AJOject_1: " + AJObject);
    }

    // 안드로이드에 있는 함수를 호출.
    public void Call_AndroidSTT(string _lang)
    {
        AJObject.Call("StartSpeechReco", _lang);
    }

    // 안드로이드 메세지.
    public void msgUnity(string _msg)
    {
        if(message != null)
        {
            message(_msg);
        }
    }

    // 음성인식 결과.
    public void sttUnity(string _result)
    {
        if(sttResult != null)
        {
            sttResult(_result);
        }
    }

    // 받은것을 콜백으로 실제 사용하는 스크립트로 넘겨준다.
    public void SetCallbackSTTresult(delegateSTTResult callback) { this.sttResult = callback; }
    public void SetCallbackMessage(delegateMessage callback) { this.message = callback; }

}
