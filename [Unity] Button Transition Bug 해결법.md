# [Unity] Button Transition Bug 해결법

## 내용

Button 의 Transition 을 Sprite Swap 으로 했을경우,

Button 을 클릭하고 나면 버튼의 Sprite 가 Highlight 에서 Normal 버튼으로 돌아가지 않는 Bug? 가 있다.

이를 해결 하기 위한 하나의 방법으로 Button 과 함께 **EventTrigger** Component 를 추가해주는 방법이 

있다.

```c#
public class ButtonAddEventTrigger : MonoBehaviour
{
    public Button button;

    private void Start()
    {
        EventTrigger eventTrigger = button.AddComponent<EventTrigger>();
        var eTriggerEntry = new EventTrigger.Entry();
        eTriggerEntry.eventID = EventTriggerType.PointerExit;
        eTriggerEntry.callback.AddListener((e) => button.OnDeselect(e));
        eventTrigger.triggers.Add(eTriggerEntry);
    }
}

```

위와 같은 방법으로 다른 EventTriggerType 도 추가 및 관리가 가능하다.