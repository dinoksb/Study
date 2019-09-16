using UnityEngine;
public class AndroidPermissionExample : Monobehavior
{
     private IEnumerator PermissionCheck()
    {
        if (Application.isEditor)
        {
            yield break;
        }

        // check permission
        if (!AndroidPermissionController.IsPermissionGranted(AndroidPermissionController.PERMISSION_WRITE_EXTERNAL_STORAGE))
        {
            FirebaseAnalyticsController.LogEvent("WriteStoragePermissionNotGranted");

            AndroidPermissionCallback callback = new AndroidPermissionCallback((onGranted) =>
            {
                isPermissionPopupOn = false;
                isDone = true;
            },
            (onDeny) =>
            {
                isPermissionPopupOn = false;
                isDone = true;
                Application.Quit();
            });

            isPermissionPopupOn = true;
            AndroidPermissionController.RequestPermission(new string[] { AndroidPermissionController.PERMISSION_WRITE_EXTERNAL_STORAGE }, callback);

            yield return WaitForCompletion();
        }
    }
}