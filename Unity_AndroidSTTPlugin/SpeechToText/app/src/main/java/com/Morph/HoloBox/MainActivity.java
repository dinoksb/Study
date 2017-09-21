package com.Morph.HoloBox;

import android.annotation.SuppressLint;
import android.app.Activity;
import android.content.ActivityNotFoundException;
import android.content.Context;
import android.content.Intent;
import android.media.AudioManager;
import android.media.MediaPlayer;
import android.media.ToneGenerator;
import android.nfc.Tag;
import android.os.Bundle;

// 유니티
import com.unity3d.player.UnityPlayerActivity;
import com.unity3d.player.UnityPlayer;

import android.os.CountDownTimer;
import android.os.Debug;
import android.os.Handler;
import android.os.Message;

// 음성인식
import android.speech.RecognitionListener;
import android.speech.RecognizerIntent;
import android.speech.SpeechRecognizer;

import android.util.Log;

import java.util.ArrayList;
import java.util.Collections;
import java.util.List;
import java.util.Timer;
import java.util.TimerTask;

import  android.util.DebugUtils;

public class MainActivity extends UnityPlayerActivity
{
    // 유니티에서 스크립트가 붙을 오브젝트 이름.
    private String UnityObjName = "pluginUnity";

    // 유니티에서 에러메세지 받을 함수 이름.
    private  String UnityMsg = "msgUnity";

    // 유니티에서 음성인식 결과를 받을 함수 이름.
    private  String UnitySTTResult = "sttUnity";
    private String UnitySTTPartialResult = "sttPartialUnity";

    // 핸들러 관련.
    private final int STTSTART = 1;
    private final int STTREADY = 2;
    private final int STTEND = 3;
    private final int STTTIMEOUT = 4;
    private msgHandler mHandler = null;

    // 음성인식 관련.
    private  SpeechRecognizer recognizer;
    private ArrayList<SpeechRecognizer> recognizerList;
    private Listener listener;
    private  static Context context;
    private String recogLang = "ko-KR";
    private boolean isError;
    private int mainTime = 0;
    private TimerTask task;
    private boolean isEndOfSpeech;
    private Timer currentTimer;

    // 안드로이드 볼륨(오디오 관련)
    private AudioManager audioManager;
    private int originAudioVolume;

    // 정적 변수
    private static final String TAG = MainActivity.class.getSimpleName();
    private static final boolean DEBUG = false;

    // 유니티에서 안드로이드 음성인식을 하려면 멀티 쓰레딩을 해야한다.
    // 그렇기 때문에 핸들러를 사용한다.

    class msgHandler extends Handler
    {
        @Override
        public void handleMessage(Message msg)
        {
            super.handleMessage(msg);
            switch (msg.what)
            {
                case STTREADY:
                    // 유니티에 음성인식이 시작했다는 메세지를 보냄.
                    UnityPlayer.UnitySendMessage(UnityObjName, UnityMsg, "START");
                    break;
                case  STTSTART:
                    // 실제 음성인식 작동 부분.
                    StartSpeechRecoService();
                    break;
                case STTEND:
                    // 유니티에 음성인식을 종료했다는 메세지를 보냄.
                    UnityPlayer.UnitySendMessage(UnityObjName, UnityMsg, "END");
                    //SetVolume(originAudioVolume);
                case STTTIMEOUT:
                    UnityPlayer.UnitySendMessage(UnityObjName, UnityMsg, "TIME_OUT");
                    break;
            }
        }
    }

    // 음성인식 리스너.
    private  class Listener implements RecognitionListener
    {
        @Override
        public void onReadyForSpeech(Bundle params) {
            // 음성인식이 작동하면 여기가 호출된다.
            mHandler.sendEmptyMessage(STTREADY);
        }

        @Override
        public void onBeginningOfSpeech() {
            isEndOfSpeech = false;
            Log.i("Unity", "음성인식 시작");
        }

        @Override
        public void onRmsChanged(float rmsdB) {

        }

        @Override
        public void onBufferReceived(byte[] buffer) {

        }

        @Override
        public void onEndOfSpeech() {

            // 음성인식이 끝나면 여기가 호출된다.
            mHandler.sendEmptyMessage(STTEND);
            EndSpeechReco();
            StopSpeechReco();
            Log.i("Unity", "onEndOfSpeech is Done !@#!@#!@#!@#!@#!@#!@#");
        }

        @Override
        public void onError(int error)
        {
            if(isEndOfSpeech) return;
            // 음성인식이 실패하면 나오는 에러.

            Log.i("Unity", "mHandler.obtainMessage().what: " + mHandler.obtainMessage().what);

            if(mHandler.obtainMessage().what == STTEND)
            return;

                String errMsg = "";
            switch (error)
            {
                case 0:
                    errMsg = "ERROR_NETWORK_TIMEOUT";   // 네트워크 오류 발생.
                    break;
                case 2:
                    errMsg = "ERROR_NETWORK";           // 네트워크 오류 발생.
                    break;
                case 3:
                    errMsg = "ERROR_AUDIO";             // 오디오 입력중 오류 발생.
                    break;
                case 4:
                    errMsg = "ERROR_SERVER";            // 서버 오류발생.
                    break;
                case 5:
                    errMsg = "ERROR_CLIENT";            // 입력이 없습니다.
                    break;
                case 6:
                    errMsg = "ERROR_SPEECH_TIMEOUT";   // 음성인식 서비스 과부하.
                    break;
                case 7:
                    errMsg = "ERROR_NO_MATCH";            // 클라이언트 오류발생.
                    break;
                case 8:
                    errMsg = "ERROR_RECOGNIZER_BUSY";            // 클라이언트 오류발생.

                    break;
            }
//
            try
            {
                Log.i("Unity", "onError try 구문!@#!@#!@!@#!@#!@#!@#!@#!@#");
                UnityPlayer.UnitySendMessage(UnityObjName, UnityMsg, errMsg);

                isError = true;


            } catch (Exception e) {
            }
//
//            // 핸들러 메세지 초기화.
//            EndSpeechReco();
//            StopSpeechReco();
//            isEndOfSpeech = true;
        }

        @Override
        public void onResults(Bundle results)
        {
            // 음성인식 결과.
            mHandler.removeMessages(0);
            ArrayList matches = results.getStringArrayList("results_recognition");
            if(matches != null)
            {
                mainTime = 0;
                currentTimer.cancel();
                currentTimer.purge();
                isEndOfSpeech = true;
                try
                {
                    //UnityPlayer.UnitySendMessage(UnityObjName, UnitySTTResult, (String)matches.get(0));

                    for(int i = 0; i < matches.size(); ++i)
                    {
                        UnityPlayer.UnitySendMessage(UnityObjName, UnitySTTResult, (String)matches.get(i));
                        Log.i("Unity", matches.get(i).toString());
                    }
                    StopSpeech();
                }
                catch (Exception e){
                }
            }
        }

        @Override
        public void onPartialResults(Bundle partialResults) {
            if(partialResults.getStringArrayList(SpeechRecognizer.RESULTS_RECOGNITION) != null){
                ArrayList resultArray = partialResults.getStringArrayList(SpeechRecognizer.RESULTS_RECOGNITION);
                String word = (String)resultArray.get(resultArray.size() - 1);

                UnityPlayer.UnitySendMessage(UnityObjName, UnitySTTPartialResult, word);
            }
//            String string = "Bundle{";
//            for (String key : partialResults.keySet())
//            {
//                partialResults.getBinder()
//                string += " " + key + " => " + partialResults.get(key) + ";";
//            }
//            UnityPlayer.UnitySendMessage(UnityObjName, UnitySTTPartialResult, string);
//            Log.i("Unity","onPartialResults: " + string);
        }

        @Override
        public void onEvent(int eventType, Bundle params) {

        }
    };

    // 초기화.
    @Override
    protected  void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        //audioManager = (AudioManager)getSystemService(Context.AUDIO_SERVICE);
        //audioManager.adjustStreamVolume(AudioManager.STREAM_NOTIFICATION, AudioManager.ADJUST_MUTE, 0);

        // 핸들러를 붙인다.
        mHandler = new msgHandler();

        // 음성인식 리스너를 등록한다.
        context = getApplicationContext();
        originAudioVolume = GetVolume();
        recognizerList = new ArrayList<>();
    }

    @Override
    public void onDestroy()
    {
        super.onDestroy();
        audioManager.adjustStreamVolume(AudioManager.STREAM_NOTIFICATION, AudioManager.ADJUST_UNMUTE, 0);
    }

    // 유니티에서 호출할 함수.
    public void StartSpeechReco(String _lang)
    {
        Log.i("Unity", "StartSpeechReco 시작!@#!@#!@#!@#!@#!@#!@#");
            try{
                Message msg1 = new Message();
                msg1.what = STTSTART;
                mHandler.sendMessage(msg1);

                final Timer timer = new Timer();
                currentTimer = timer;

                task = new TimerTask()
                {
                    @Override
                    public void run()
                    {
                        try{
                            mainTime++;
                            Log.i("Unity", "Sec: " + mainTime);

                            if(mainTime <= 5 && isError)
                            {
                                EndSpeechReco();
                                StopSpeechReco();
                                mHandler.sendEmptyMessage(STTSTART);
                                Log.i("Unity", "음성인식 재시작");
                                isError = false;
                                isEndOfSpeech = false;
                            }
                            else if(mainTime > 5)
                            {
                                StopSpeech();
                                mainTime = 0;
                                timer.cancel();
                                timer.purge();
                                Log.i("Unity", "5초가 지나서 음성인식을 종료합니다.");
                                isEndOfSpeech = true;
                                mHandler.sendEmptyMessage(STTTIMEOUT);
                            }
                        }
                        catch (Exception e) {
                            e.printStackTrace();
                        }
                    }
                };

                timer.schedule(task, 0, 1000);
        }
        catch (Exception e){
        }
    }

    // 유니티에서 호출할 함수.
    public void StopSpeech()
    {
        EndSpeechReco();
        StopSpeechReco();

        if(currentTimer != null)
            currentTimer = null;

        try{
            Message msg1 = new Message();
            msg1.what = STTEND;
            mHandler.sendMessage(msg1);
        }
        catch (Exception e){
        }
    }

    public void SetVolume(int _vol)
    {
        // 유니티에서 볼륨값을 넣어서 호출함.
        audioManager = (AudioManager)getSystemService(Context.AUDIO_SERVICE);
        int volume = _vol;
        int inputMax = 10;
        // 기기 자체의 최고 볼륨값을 가져옴.
        int musicMaxVolume = audioManager.getStreamMaxVolume(AudioManager.STREAM_MUSIC);
        float lastVolume = (float)volume / (float)inputMax;

        // 기기 최고의 볼륨값으로 보정.
        int lastSystemVolume = (int)(musicMaxVolume * lastVolume);
        Log.i("Unity", "Max - " + musicMaxVolume + " / current : " + String.valueOf(musicMaxVolume * lastVolume));

        // 볼륨을 설정해주면 끝.
        audioManager.setStreamVolume(AudioManager.STREAM_MUSIC, lastSystemVolume, 0);

        // 볼륨 바뀔때 소리내주는 부분(제조사 마다 조금씩 차이가 있음.)
        //ToneGenerator toneG = new ToneGenerator(AudioManager.STREAM_ALARM, lastSystemVolume);
        //toneG.startTone(ToneGenerator.TONE_CDMA_ALERT_CALL_GUARD, 100);
    }

    public int GetVolume()
    {
        // 기기의 현재 볼륨값을 얻어온다.
        // 유니티가 시작할때 얻어온 후 볼륨 제어 변수에 넣어주면 된다.
        AudioManager tempAdm = (AudioManager)getSystemService(Context.AUDIO_SERVICE);
        int maxVol = tempAdm.getStreamMaxVolume(AudioManager.STREAM_MUSIC);
        int currentVol = tempAdm.getStreamMaxVolume(AudioManager.STREAM_MUSIC);

        // 0 ~ 10 범위값으로 변환
        int result = (int)(((float)currentVol / (float)maxVol) * 10);
        UnityPlayer.UnitySendMessage("pluginUnity", "AndroidVolume", String.valueOf(result));
        return result;
    }

    // 실제 음성인식을 호출한다. "recogLang" 은 음성인식 할 언어로 하면된다.
    public void StartSpeechRecoService()
    {
        Log.i("Unity", "StartSpeechRecoService Play!@#!@#!@#!@#!@#!@#");
        if(GetVolume() != 0) {
            SetVolume(0);
        }



        SpeechRecognizer rc = SpeechRecognizer.createSpeechRecognizer(context);
        listener = new Listener();
        rc.setRecognitionListener(listener);

        Intent i = new Intent(RecognizerIntent.ACTION_RECOGNIZE_SPEECH); // 음성인식.
        i.putExtra(RecognizerIntent.EXTRA_CALLING_PACKAGE, getPackageName());
        i.putExtra(RecognizerIntent.EXTRA_LANGUAGE, recogLang);
        i.putExtra(RecognizerIntent.EXTRA_PARTIAL_RESULTS, true);

        rc.startListening(i);

        recognizerList.add(rc);

        //isEndOfSpeech = false;
            Log.i("Unity", "StartSpeechRecoService End!@#!@#!@#!@#!@#!@#");
       //}
    }

    // 메세지 초기화.
    public void EndSpeechReco()
    {
        this.mHandler.removeMessages(STTREADY);
        this.mHandler.removeMessages(STTSTART);
        this.mHandler.removeMessages(STTEND);
    }

    // 음성인식 종료.
    public void StopSpeechReco()
    {
        if(recognizerList.isEmpty() == false)
        {
            recognizerList.remove(0);
            Log.i("Unity", "SpeechReco 리스트 삭제");
        }
//        if(recognizer != null)
//        {
//            recognizer.destroy();
//            recognizer = null;
//        }
    }
}
