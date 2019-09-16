using UnityEngine;

public class NetworkChecker
{
    // 기기가 인터넷에 연결 할 수 있는지 확인.
    public static bool IsInternetConnected()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            return false;
        }

        return true;
    }

    // 기기가 통신 사업자 데이터 네트워크를 사용하는지 여부.
    public static bool IsUsingData()
    {
        if(Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
        {
            return true;
        }

        return false;
    }

    // 기기가 wi-fi 를 사용하는지 여부 (Standalone 일 경우 Lan 을 통해 인터넷 연결 가능한지 체크).
    public static bool IsUsingWifi()
    {
        if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
        {
            return true;
        }

        return false;
    }
}
