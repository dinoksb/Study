using UnityEngine;

public class DeviceSpecChecker : MonoBehaviour
{
    public static void DeviceSpecCheck(int minRam, int minCore)
    {
        Debug.Log("DeviceSpecCheck");

        if (Application.isEditor)
        {
            yield break;
        }

        // device spec check
        var ram = SystemInfo.systemMemorySize / 1000;
        var core = SystemInfo.processorCount;

        Debug.Log("### ram: " + ram);
        Debug.Log("### core: " + core);

        if ((ram < minRam || core < minCore))
        {
            Debug.Log("스펙 충족하지 않음");
        }
    }
}