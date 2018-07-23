# [Unity] Android 직접 접근법

## 내용

Unity 에서 Android Plugin 을 제작하지 않고 Android 의 클래스나 함수에 직접 접근이 가능하다.

## 예시

**Android** 에서 **Device Model 명** 을 얻어오는 방법.

```c#
AndroidJavaClass unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
AndroidJavaObject currentActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
AndroidJavaClass contextClass = new AndroidJavaClass("android.os.Build");
str = contextClass.GetStatic<string>("Model");
```

[Android Developer - (Model)][link]

[link]:https://developer.android.com/reference/android/os/Build.html#MODEL
