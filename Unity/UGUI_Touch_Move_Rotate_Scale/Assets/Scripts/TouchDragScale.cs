using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class TouchDragScale : MonoBehaviour, IPointerDownHandler, IDragHandler {

	public RectTransform TargetRectTransform;

    [SerializeField] private float minSizeRatio = 0.5f;
    [SerializeField] private float maxSizeRatio = 2f;

    private Vector2 minSize = Vector2.zero;
	private Vector2 maxSize = Vector2.zero;
	
	private Vector2 originalLocalPointerPosition;
	private Vector2 originalSizeDelta;

    public Action OnInitialize;
    public Action<Vector2> OnDragSizeDelta;

    [Header("[AspectRatio_Filter]")]
    [SerializeField] private bool isUseAspectRatioFilter;
    [SerializeField] private AspectRatioFitter.AspectMode AspectMode;
    [SerializeField] private int AspectRatio;
    [Tooltip("스크린사이즈의 몇퍼센트 크기로 설정할것인지.")]
    [SerializeField] private int sizePercentage = 10;
    

	void Awake () {
        if(isUseAspectRatioFilter)
        {
            AspectRatioFitter afFilter = TargetRectTransform.GetComponent<AspectRatioFitter>();
            if (afFilter == null)
                afFilter = TargetRectTransform.gameObject.AddComponent<AspectRatioFitter>();

            afFilter.aspectMode = AspectMode;
            afFilter.aspectRatio = AspectRatio;

            float size = 0;

            switch (AspectMode)
            {
                case AspectRatioFitter.AspectMode.WidthControlsHeight:
                    size = Screen.width * sizePercentage / 100;
                    break;
                case AspectRatioFitter.AspectMode.HeightControlsWidth:
                    size = Screen.height * sizePercentage / 100;
                    break;
                case AspectRatioFitter.AspectMode.FitInParent:
                    break;
                case AspectRatioFitter.AspectMode.EnvelopeParent:
                    break;
                default:
                    break;
            }

            TargetRectTransform.sizeDelta = new Vector2(size, size);
        }

        Vector2 calcMinSize = new Vector2(TargetRectTransform.sizeDelta.x * minSizeRatio, TargetRectTransform.sizeDelta.y * minSizeRatio);
        Vector2 calcMaxSize = new Vector2(TargetRectTransform.sizeDelta.x * maxSizeRatio, TargetRectTransform.sizeDelta.y * maxSizeRatio);


        minSize = new Vector2(calcMinSize.x, calcMinSize.y);
        maxSize = new Vector2(calcMaxSize.x, calcMaxSize.y);
	}

    public void OnPointerDown (PointerEventData data) {
		originalSizeDelta = TargetRectTransform.sizeDelta;
		RectTransformUtility.ScreenPointToLocalPointInRectangle (TargetRectTransform, data.position, data.pressEventCamera, out originalLocalPointerPosition);
	}
	
	public void OnDrag (PointerEventData data) {
		if (TargetRectTransform == null)
			return;
		
		Vector2 localPointerPosition;
		RectTransformUtility.ScreenPointToLocalPointInRectangle (TargetRectTransform, data.position, data.pressEventCamera, out localPointerPosition);
		Vector3 offsetToOriginal = localPointerPosition - originalLocalPointerPosition;
		
		Vector2 sizeDelta = originalSizeDelta + new Vector2 (offsetToOriginal.x, -offsetToOriginal.y);

        sizeDelta = new Vector2 (
			Mathf.Clamp (sizeDelta.x, minSize.x, maxSize.x),
			Mathf.Clamp (sizeDelta.y, minSize.y, maxSize.y)
		);

        TargetRectTransform.sizeDelta = sizeDelta;

        if (OnDragSizeDelta != null)
            OnDragSizeDelta(TargetRectTransform.sizeDelta);
    }
}
