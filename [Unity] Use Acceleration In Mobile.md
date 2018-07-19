# [Unity] Use Acceleration In Mobile

## Input.Acceleration

Unity는 기본적으로 가속도를 얻을수 있는 API를 제공,

가속도 센서를 이용하지 않는 응용 프로그램에 대해서는 쓸모없는 설정이므로

Disabled 하는 편이 좋다.

### 가속도를 이용한 Object 이동 예제.

```c#
using UnityEngine;
using System.Collections;

public  class Player : MonoBehaviour {

    float speed = 5.0f ;

    void Update () {
        var dir = Vector3.zero;
        dir.x = Input.acceleration.x;
        dir.y = Input.acceleration.y;

        if (dir.sqrMagnitude> 1 ) {
            dir.Normalize ();
        }

        dir * = Time.deltaTime;

        transform.Translate (dir * speed);
    }
}
```

[Unity Documentation - Input.acceleration][link]

[link]: https://docs.unity3d.com/ScriptReference/Input-acceleration.html

---


이런 식으로도 사용이 가능.
```
public bool shakeToOpen = true;
public float shakeAcceleration = 3f;

if (shakeToOpen && Input.acceleration.sqrMagnitude > shakeAcceleration)
{
    isVisible = true;
}
```