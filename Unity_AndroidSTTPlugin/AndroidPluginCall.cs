using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AndroidPluginCall : MonoBehaviour
{
    public Text resultText;
    public Text debugText;

    private void Awake()
    {
        AndroidPluginManager.getInstance();
    }

    private void Start()
    {
        AndroidPluginManager.getInstance().SetCallbackSTTresult(new delegateSTTResult(STTResult));
        AndroidPluginManager.getInstance().SetCallbackMessage(new delegateMessage(AndroidMessage));
    }

    private void STTResult(string result)
    {
        Debug.Log("STT : " + result);
        resultText.text = result;
    }

    private void AndroidMessage(string msg)
    {
        Debug.Log("Log: " + msg);
        debugText.text = msg;
    }

    /// <음성인식>
    /// 여기서 호출하면 안드로이드에서 실제 음성인식 호출하고 등록된 리스너가 
    /// 처리를 해서 위에 콜백으로 결과가 들어오게 됩니다.
    /// </음성인식>
    public void Call_STTstart()
    {
        AndroidPluginManager.getInstance().Call_AndroidSTT("ko-KR");
    }
}
