using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchDragPosition : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    [Header("[Margin]")]
    public int left;
    public int top;
    public int right;
    public int bottom;

    private int marginLeft;
    private int marginTop;
    private int marginRight;
    private int marginBottom;

    Vector3 offset;

    private void Start()
    {
        marginLeft = left;
        marginTop = Screen.height + top;
        marginRight = Screen.width + right;
        marginBottom = bottom;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        offset = transform.position - Input.mousePosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 pos = Input.mousePosition + offset;

        this.transform.position = pos;
        
        // 왼쪽 끝
        if(transform.position.x < 0 + marginLeft)
        {
            this.transform.position = new Vector3(marginLeft, pos.y, 0);

            // 위쪽 끝
            if (transform.position.y > marginTop)
            {
                this.transform.position = new Vector3(marginLeft, marginTop, 0);
            }
            // 아래쪽 끝
            else if (transform.position.y < marginBottom)
            {
                this.transform.position = new Vector3(marginLeft, marginBottom, 0);
            }
        }

        // 오른쪽 끝
        else if(transform.position.x > marginRight)
        {
            this.transform.position = new Vector3(marginRight, pos.y, 0);

            // 위쪽 끝
            if (transform.position.y > marginTop)
            {
                this.transform.position = new Vector3(marginRight, marginTop, 0);
            }
            // 아래쪽 끝
            else if (transform.position.y < marginBottom)
            {
                this.transform.position = new Vector3(marginRight, marginBottom, 0);
            }
        }

        // 위쪽 끝
        if (transform.position.y > marginTop)
        {
            this.transform.position = new Vector3(pos.x, marginTop, 0);

            // 왼쪽 끝
            if (transform.position.x < 0 + marginLeft)
            {
                this.transform.position = new Vector3(marginLeft, marginTop, 0);
            }
            // 오른쪽 끝
            else if (transform.position.x > marginRight)
            {
                this.transform.position = new Vector3(marginRight, marginTop, 0);
            }
        }

        // 아래쪽 끝
        else if(transform.position.y < marginBottom)
        {
            this.transform.position = new Vector3(pos.x, marginBottom, 0);

            // 왼쪽 끝
            if (transform.position.x < 0 + marginLeft)
            {
                this.transform.position = new Vector3(marginLeft, marginBottom, 0);
            }
            // 오른쪽 끝
            else if (transform.position.x > marginRight)
            {
                this.transform.position = new Vector3(marginRight, marginBottom, 0);
            }
        }
    }
}
