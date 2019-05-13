# [Unity] GameObject Find 함수 퍼포먼스 측정.

## 내용
1개의 게임오브젝트를 생성하고 1프레임당 1000번의 호출로 테스트 해본 측정 결과. 아래의 순서로 속도로 빠르기가 측정됨. 

* 오브젝트의 타입으로 게임오브젝트를 찾는것은 아예 쓰지않는게 좋을정도로 퍼포먼스가 안좋음.


GameObject.Find(); > GameObject.FindGameObjectWithTag(); >
GameObject.FindObjectOfType<T>();

[Finding Objects in Unity3D – Misconceptions and Recommendations(unity3d.college)][link]

[link]:https://unity3d.college/2018/09/04/finding-objects-unity3d-misconceptions-recommendations/