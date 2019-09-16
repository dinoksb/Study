using System;
using UnityEngine;
using UniRx;
using UnityEngine.Android;

public class AndroidPermissionCallback : AndroidJavaProxy
{
    private event Action<string> OnPermissionGrantedAction;
    private event Action<string> OnPermissionDeniedAction;


    public AndroidPermissionCallback(Action<string> onGrantedCallback, Action<string> onDeniedCallback)
        : base("com.unity3d.plugin.UnityAndroidPermissions$IPermissionRequestResult")
    {
        if (onGrantedCallback != null)
        {
            OnPermissionGrantedAction += onGrantedCallback;
        }
        if (onDeniedCallback != null)
        {
            OnPermissionDeniedAction += onDeniedCallback;
        }
    }

    // Handle permission granted
    public virtual void OnPermissionGranted(string permissionName)
    {
        if (OnPermissionGrantedAction != null)
        {
            MainThreadDispatcher.Post(_ => OnPermissionGrantedAction(permissionName), null);
        }
    }

    // Handle permission denied
    public virtual void OnPermissionDenied(string permissionName)
    {
        if (OnPermissionDeniedAction != null)
        {
            MainThreadDispatcher.Post(_ => OnPermissionDeniedAction(permissionName), null);
        }
    }
}

public class AndroidPermissionController
{
    // permision names (필요한 Permission 추가해서 사용)
    public const string PERMISSION_READ_PHONE_STATE = "android.permission.READ_PHONE_STATE";
    public const string PERMISSION_WRITE_EXTERNAL_STORAGE = "android.permission.WRITE_EXTERNAL_STORAGE";
    public const string PERMISSION_CAMERA = "android.permission.CAMERA";
    public const string PERMISSION_FINE_LOCATION = "android.permission.ACCESS_FINE_LOCATION";
    public const string PERMISSION_COARSE_LOCATION = "android.permission.ACCESS_COARSE_LOCATION";
    public const string PERMISSION_MICROPHONE = "android.permission.RECORD_AUDIO";

    private static AndroidJavaObject activity;
    private static AndroidJavaObject permissionService;


    private static AndroidJavaObject GetActivity()
    {
        if (activity == null)
        {
            var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        }

        return activity;
    }

    private static AndroidJavaObject GetPermissionsService()
    {
        return permissionService ?? (permissionService = new AndroidJavaObject("com.unity3d.plugin.UnityAndroidPermissions"));
    }

    public static bool IsPermissionGranted(string permissionName)
    {
        return GetPermissionsService().Call<bool>("IsPermissionGranted", GetActivity(), permissionName);
    }

    public static bool IsAllPermissionGranted()
    {
        return IsPermissionGranted(PERMISSION_CAMERA) && IsPermissionGranted(Permission.FineLocation) && IsPermissionGranted(Permission.ExternalStorageWrite);
    }

    public static void RequestPermission(string[] permissionNames, AndroidPermissionCallback callback)
    {
        GetPermissionsService().Call("RequestPermissionAsync", GetActivity(), permissionNames, callback);
    }

    public static void RequestPermission(AndroidPermissionCallback callback)
    {
        var permissionNames = new string[] { PERMISSION_CAMERA, Permission.FineLocation, Permission.ExternalStorageWrite };
        GetPermissionsService().Call("RequestPermissionAsync", GetActivity(), permissionNames, callback);
    }
}
