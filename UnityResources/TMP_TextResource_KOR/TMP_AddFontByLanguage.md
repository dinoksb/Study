# [Unity] Font 별 언어 추가하기.

## 내용

Unity 의 TextMeshPro 를 이용하여 폰트별로 영어 이외의 언어를 추가하는 방법.

## 예시

* 한글 폰트 제작 예시.
  한글의 경우 상용 한글의 경우 2350자, 모든 글자의 경우 11172자 이다. 
  하여 하나의 폰트 아틀라스에 모든 글자를 담을수 없고 여러개의 Font Atlas 로 나누어 만들어야 한다.

1. Window -> Font Asset Creator 창을 연다.
   ![TMP Create Font Example](Images/1.png)
2. 해당 폰트에 맞게 Font Settings 옵션을 설정한다.
   * Character Set 옵션을 Characters From File 로 설정한 후 해당 언어의 글자를 붙여넣는다.
  
3. Font Atlas 생성.
   
   * Font Atals 생성시 Missing Characters 가 없는지 확인하고 있을경우 Font Settings 의 옵션을 조절하여 Missing 이 없도록 생성한다.
   ![TMP Create Font Example](Images/2.png)
4. 2350자 또는 11172자를 모두 담은 아틀라스 들을 생성한 후 최상위 폰트 아틀라스를 선택한다.

![TMP Create Font Example](Images/3.png)

5. Inspector 창에서 Fallback Font Assets 창의 Fallback List 에
   제작한 한글 FontAsset 들을 모두 넣는다.
   
![TMP Create Font Example](Images/4.png)

6. TextMeshPro Text Font 에 모든 한글 폰트가 담겨있는 최상위 폰트 어셋을 설정해준다.

![TMP Create Font Example](Images/5.png)

Tip: Font Asset Creator 에서 Padding 은 Resolution 이 커질 때마다 2의 배수로 곱해준다.

