# [Unity] 기본최대 메쉬 정점 수

## 내용

Unity 에서 표현할 수 있는 기본 최대 메쉬의 정점 수는 65535 이다.

정점수를 더 많이 표현하려면 아래와같이 설정해줘야한다.

```c#
mesh.indexFotmat = Rendering.indexFotmat.UInt32;
```

하지만 모든 플랫폼에서 지원된다는 보장이 없다.