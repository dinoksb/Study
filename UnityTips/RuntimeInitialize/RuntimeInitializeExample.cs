using UnityEngine;

// 함수가 Static 이 아닐경우 동작하지 않음.

public class RuntimeInitializeTest
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void Test1()
    {
        Debug.Log("RuntimeInitializeOnLoadMethod AfterSceneLoad");
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Test2()
    {
        Debug.Log("RuntimeInitializeOnLoadMethod BeforeSceneLoad");
    }

    [RuntimeInitializeOnLoadMethod()]
    static void Test3()
    {
        Debug.Log("RuntimeInitializeOnLoadMethod None");
    }
}
