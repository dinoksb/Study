using UnityEngine;

public class CheckSomethingWithFixedTime : MonoBehaviour
{
    private void FixedUpdate()
    {
        // 1초에 한번씩 체크.
        if(FixedTime.FixedFrameCount % 60 == 0)
        {
            // Do Something.
        }

        // 0.5초에 한번씩 체크.
        if (FixedTime.FixedFrameCount % 30 == 0)
        {
            // Do Something.
        }
    }
}
