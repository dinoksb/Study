using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TouchDragRotation : MonoBehaviour
{
    public RectTransform TargetRectTransform;

    Vector2 centerPoint;

    public float maximumRotateAngle = 360;

    float calcAngle = 0f;
    float prevCalcAngle = 0f;

    bool isTouched = false;

    public float GetClampedValue()
    {
        // returns a value in range [-1,1] similar to GetAxis("Horizontal")
        return calcAngle / maximumRotateAngle;
    }

    public float GetAngle()
    {
        // returns the wheel angle itself without clamp operation
        return calcAngle;
    }

    void Start()
    {
        InitEventsSystem();
    }

    void Update()
    {
        // Rotate the image
        if(isTouched)
            TargetRectTransform.localEulerAngles = Vector3.back * calcAngle;
    }

    void InitEventsSystem()
    {
        EventTrigger events = gameObject.GetComponent<EventTrigger>();

        if (events == null)
            events = gameObject.AddComponent<EventTrigger>();

        if (events.triggers == null)
            events.triggers = new System.Collections.Generic.List<EventTrigger.Entry>();

        EventTrigger.Entry entry = new EventTrigger.Entry();
        EventTrigger.TriggerEvent callback = new EventTrigger.TriggerEvent();
        UnityAction<BaseEventData> functionCall = new UnityAction<BaseEventData>(PressEvent);
        callback.AddListener(functionCall);
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback = callback;

        events.triggers.Add(entry);

        entry = new EventTrigger.Entry();
        callback = new EventTrigger.TriggerEvent();
        functionCall = new UnityAction<BaseEventData>(DragEvent);
        callback.AddListener(functionCall);
        entry.eventID = EventTriggerType.Drag;
        entry.callback = callback;

        events.triggers.Add(entry);
    }

    public void PressEvent(BaseEventData eventData)
    {
        Vector2 pointerPos = ((PointerEventData)eventData).position;

        isTouched = true;
        centerPoint = RectTransformUtility.WorldToScreenPoint(((PointerEventData)eventData).pressEventCamera, TargetRectTransform.position);
        prevCalcAngle = Vector2.Angle(Vector2.up, pointerPos - centerPoint);
    }

    public void DragEvent(BaseEventData eventData)
    {
        // Executed when mouse/finger is dragged over the target
        Vector2 pointerPos = ((PointerEventData)eventData).position;

        float wheelNewAngle = Vector2.Angle(Vector2.up, pointerPos - centerPoint);
        // Do nothing if the pointer is too close to the center of the target
        if (Vector2.Distance(pointerPos, centerPoint) > 20f)
        {
            if (pointerPos.x > centerPoint.x)
                calcAngle += wheelNewAngle - prevCalcAngle;
            else
                calcAngle -= wheelNewAngle - prevCalcAngle;
        }
        // Make sure target angle never exceeds maximumRotateAngle
        //calcAngle = Mathf.Clamp(calcAngle, -maximumRotateAngle, maximumRotateAngle);

        prevCalcAngle = wheelNewAngle;
    }

    public void ReleaseEvent(BaseEventData eventData)
    {
        // Executed when mouse/finger stops touching the target
        // Performs one last DragEvent, just in case
        //DragEvent(eventData);

        isTouched = false;
    }
}
