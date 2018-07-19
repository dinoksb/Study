using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class ScreenShot : MonoBehaviour
{
    //public GameObject SaveForScreenshot;
    private Coroutine coAndroidScreenshot; 

    private void Awake()
    {
        Debug.Log(Application.persistentDataPath);
    }

    public void onScreenShot()
    {
        Shot();
    }

    private void Shot()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if(coAndroidScreenshot == null)
            {
                coAndroidScreenshot = StartCoroutine(AndroidScreenshot());
            }
        }
        else
        {
            string date = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            string myFilename = "myScreenshot_" + date + ".png";
            string myFolderLocation = Application.persistentDataPath;
            string myScreenshotLocation = myFolderLocation + myFilename;

            if (!System.IO.Directory.Exists(myFolderLocation))
            {
                System.IO.Directory.CreateDirectory(myFolderLocation);
            }

            Application.CaptureScreenshot(myScreenshotLocation);
            Debug.Log(Application.persistentDataPath);
        }

    }

    private IEnumerator AndroidScreenshot()
    {
        // 한프레임이 끝날때까지 대기.
        yield return new WaitForEndOfFrame();

        // 저장할 경로를 설정
        string date = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        string myFilename = "myScreenshot_" + date + ".png";
#if UNITY_ENGINE
        string myFolderLocation = Application.persistentDataPath;
#else
        string myFolderLocation = "/storage/emulated/0/DCIM/Camera/";
#endif
        string myScreenshotLocation = myFolderLocation + myFilename;

        // 저장할 경로에 폴더 유,무 체크 후 없을시 폴더 생성
        if (!System.IO.Directory.Exists(myFolderLocation))
        {
            System.IO.Directory.CreateDirectory(myFolderLocation);
        }

        // 스크린샷 을 Byte로 저장, Texture2D 사용.
        byte[] imageByte;

        // 2D Texture객체를 만드는데, 스크린의 넓이, 높이 를 선택하고 텍스쳐 포맷은 스크린샷을 찍기위해서 RGB24로 맞춰준다.
        Texture2D tex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);

        // 현재 화면을 픽셀 단위로 읽는다.
        tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, true);

        // 읽어들인 것을 적용한다.
        tex.Apply();

        Rect rect = new Rect(0, 0, tex.width, tex.height);
        //Camera.main.cullingMask = 1 << 8;
        Camera.main.targetTexture = new RenderTexture(tex.width, tex.height, 1);
        //SaveForScreenshot.GetComponent<SpriteRenderer>().sprite = Sprite.Create(tex, rect, new Vector2(0.5f, 0.5f));

        //Debug.Log(SaveForScreenshot.GetComponent<SpriteRenderer>().sprite);

        // 읽어 들인 픽셀을 Byte[] 에 PNG 형식으로 인코딩 해서 넣는다.
        imageByte = tex.EncodeToPNG();

        // Byte[] 에 넣었으니 Texture2D 객체는 바로 파괴한다.(메모리관리)
        DestroyImmediate(tex);

        // 원하는 경로에 파일을 저장
        System.IO.File.WriteAllBytes(myScreenshotLocation, imageByte);

        //(ex 저장경로 : "mnt/sdcard/DCIM/Camera/" )

        // 저장한 이미지를 갤러리에 보이게 하기 위해 안드로이드 갤러리를 갱신해준다.
        AndroidAPICall(myScreenshotLocation);

        coAndroidScreenshot = null;

        yield return null;
    }
    private void AndroidAPICall(string path)
    {
        AndroidJavaClass classPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

        AndroidJavaObject objActivity = classPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        AndroidJavaClass classUri = new AndroidJavaClass("android.net.Uri");

        AndroidJavaObject objIntent = new AndroidJavaObject("android.content.Intent",
                                      new object[2] { "android.intent.action.MEDIA_SCANNER_SCAN_FILE",
                                      classUri.CallStatic<AndroidJavaObject>("parse", "file://" + path) });

        objActivity.Call("sendBroadcast", objIntent);
    }
}
