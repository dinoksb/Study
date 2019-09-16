using UnityEngine;

public class ARCoreInstallCheckExample : MonoBehaviour
{
    private IEnumerator ARCoreValidation()
    {
        // check arcore
        CheckARCore();

        yield return WaitForCompletion();
    }

    private void CheckARCore()
    {
        Debug.Log("CheckARCoreCompatibility");

        // check arcore
        ARCoreInstallChecker.CheckARCoreCompatibility(this, () =>
        {
            // already installed
            Debug.Log("ARCoreInstalled");
            isDone = true;
        },
        () =>
        {
            // need to install
            Debug.Log("NeedToInstallARCore");

            PopupManager.Instance.AddPopup("Google Play Services for AR 앱을 설치해\nJump 앱에서 생동감 있는 사진을\n촬영해보세요", false, "확인", (value) =>
            {
                ARCoreInstallChecker.RequestARCoreInstall(() =>
                {
                    isDone = true;
                },
                () =>
                {
                    PopupManager.Instance.AddPopup("Google Play Services for AR 앱을 다운로드하지 못했어요.\n다시 시도해주세요", false, "취소", (v) =>
                    {
                        FirebaseAnalyticsController.LogEvent("DeclinedToInstallARCoreAndAppQuit");
                        isDone = true;
                        Application.Quit();
                    },
                    "재시도",
                    (vv) =>
                    {
                        FirebaseAnalyticsController.LogEvent("RetryToInstallARCore");
                        CheckARCore();

                    });
                });
            });
        },
        (error) =>
        {
            Debug.Log("CheckARCoreCompatibilityFailed");

            PopupManager.Instance.AddPopup("Google Play Services for AR 앱을 다운로드하지 못했어요.\n다시 시도해주세요", false, "취소", (v) =>
            {
                Debug.Log("DeclinedToInstallARCoreAndAppQuit");
                isDone = true;
                Application.Quit();
            },
            "재시도",
            (vv) =>
            {
                Debug.Log("RetryToInstallARCore");
                CheckARCore();
            });
        });
    }
}