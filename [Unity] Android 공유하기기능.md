#[Unity] Android 공유하기 기능

## 내용

안드로이드의 공유하기 기능을 Plugin 없이 유니티에서 바로 접근하여 사용 가능.

## 예시

```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidShareUsing : MonoBehaviour {
    private const string subject = "안드로이드 공유하기 기능";
    private const string body = "https://play.google.com/store/apps/details?id=[앱ID]]&showAllReviews=true";

    public void Share() {
#if UNITY_ANDROID && !UNITY_EDITOR
        using (AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent")) 
        using (AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent")) {
            intentObject.Call<AndroidJavaObject>("setAction", intentObject.GetStatic<string>("ACTION_SEND"));
            intentObject.Call<AndroidJavaObject>("setType", "text/plain");
            intentObject.Call<AndroidJavaObject>("putExtra", intentObject.GetStatic<string>("EXTRA_SUBJECT"), subject);
            intentObject.Call<AndroidJavaObject>("putExtra", intentObject.GetStatic<string>("EXTRA_TEXT"), body);
            using (AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity")) 
            using (AndroidJavaObject jChooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, "Share Via"))
            currentActivity.Call("startActivity", jChooser);
        }
#endif
    }
}
```

[주의사항]

AndroidJavaObject / AndroidJavaClass 처리의 부하가 매우 높음.

따라서 생성한 AndroidJavaObject / AndroidJavaClass 를 꼭 Release 해줘야하는데 이를

가비지컬렉터에서 하게된다.

하여 가능한 빨리 제거되도록 Using 블록을 사용하여 관리해야한다.

* Using 블록은 삭제(Disposal : 해제)를 보장한다.
* 흔히 쓰이는 foreach 문도 컴파일러는 using 블록으로 처리함.
