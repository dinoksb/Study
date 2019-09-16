using UnityEngine;
public class VersionCheckExmaple : MonoBehavior
{
    private IEnumerator MarketVersionCheck()
    {
        Debug.Log("MarketVersionCheck");

        if (Application.isEditor)
        {
            //yield break;
        }

        // check version from market
        //var versionCheckResult = false;
        var compareCode = 0;
        yield return versionCheck.StartVersionCheck((result) =>
        {
            compareCode = result;
        });

        if (compareCode == -1)
        {
            // 선택적 업데이트
            FirebaseAnalyticsController.LogEvent("AppNotLatestVersion", "UpdateMandatory", "N");

            LoadingScreenController.Instance.Hide();

            PopupManager.Instance.AddPopup("앱이 최신 버전이 아닙니다.\n다운로드 하시겠습니까?", false, "취소", (value) =>
            {
                isDone = true;
                LoadingScreenController.Instance.Show();
            },
            "확인",
            (v) =>
            {
                isDone = true;
                versionCheck.GoStore();
            });

            yield return WaitForCompletion();
        }
        else if (compareCode == -2 || compareCode == -3)
        {
            // 강제 업데이트
            FirebaseAnalyticsController.LogEvent("AppNotLatestVersion", "UpdateMandatory", "Y");

            LoadingScreenController.Instance.Hide();

            PopupManager.Instance.AddPopup("앱이 최신 버전이 아닙니다.\n다운로드 페이지로 이동합니다", false, "확인", (value) =>
            {
                isDone = true;
                versionCheck.GoStore();
            });

            yield return WaitForCompletion();
        }
        /*
        else if (compareCode == -100)
        {
            // 버전 정보 획득 실패
            FirebaseAnalyticsController.LogEvent("MarketVersionCheckFail");

            LoadingScreenController.Instance.Hide();

            PopupManager.Instance.AddPopup("일시적인 네트워크 장애가 발생했습니다.\n앱을 다시 실행 해주세요.\n(Version check fail)", false, "확인", (value) =>
            {
                isDone = true;
                Application.Quit();
            });

            yield return WaitForCompletion();
        }
        */
    }

}
