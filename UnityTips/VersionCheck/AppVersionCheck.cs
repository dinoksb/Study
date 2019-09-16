using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Script that controls the version check, and checks the versions - device version and the store version and sends to the store url
/// </summary>
public class AppVersionCheck : MonoBehaviour
{
    [HideInInspector] public StoreHelper store_helper;


    public IEnumerator StartVersionCheck(Action<int> onComplete)
    {
        store_helper = GetComponent<StoreHelper>();

        bool result = false;

        yield return store_helper.GetGoogleStoreVersion(flag => result = flag);

        // if fails to get the store version
        if (result == false)
        {
            onComplete?.Invoke(-100);
            yield break;
        }

        print("store version : " + store_helper.current_store_version);
        print("device version : " + Application.version);

        // compare versions
        int compareCode = VersionCompare(Application.version, store_helper.current_store_version);
        onComplete?.Invoke(compareCode);
    }

    public static int VersionCompare(string ver1, string ver2, bool evenUpdate = true)
    {
        string[] versions1 = ver1.Split('.');
        string[] versions2 = ver2.Split('.');

        int UNIT_MAJOR = 3;
        int UNIT_MINOR = 2;
        int UNIT_REVISION = 1;

        int ver1Major = ((versions1.Length >= 1) && !string.IsNullOrEmpty(versions1[0])) ? int.Parse(versions1[0]) : 0;
        int ver2Major = ((versions2.Length >= 1) && !string.IsNullOrEmpty(versions2[0])) ? int.Parse(versions2[0]) : 0;
        int ver1Minor = ((versions1.Length >= 2) && !string.IsNullOrEmpty(versions1[1])) ? int.Parse(versions1[1]) : 0;
        int ver2Minor = ((versions2.Length >= 2) && !string.IsNullOrEmpty(versions2[1])) ? int.Parse(versions2[1]) : 0;
        int ver1Rev = ((versions1.Length >= 3) && !string.IsNullOrEmpty(versions1[2])) ? int.Parse(versions1[2]) : 0;
        int ver2Rev = ((versions2.Length >= 3) && !string.IsNullOrEmpty(versions2[2])) ? int.Parse(versions2[2]) : 0;

        if (ver1.Equals(ver2))
        {
            return 0;
        }
        else if (ver1Major != ver2Major)
        {
            return (ver1Major > ver2Major) ? UNIT_MAJOR : -UNIT_MAJOR;
        }
        else if (ver1Minor != ver2Minor)
        {
            return (ver1Minor > ver2Minor) ? UNIT_MINOR : -UNIT_MINOR;
        }
        else if (ver1Rev != ver2Rev)
        {
            if (evenUpdate)
            {
                // Revision이 짝수면 강제 업데이트
                if (ver2Rev % 2 == 0)
                {
                    return -2;
                }
            }

            return (ver1Rev > ver2Rev) ? UNIT_REVISION : -UNIT_REVISION;
        }
        else
        {
            return 0;
        }
    }

    public void GoStore()
    {
        store_helper.GoToStore();
    }
}