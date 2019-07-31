using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class UIDragHandler : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler {
    public void OnBeginDrag(PointerEventData eventData)
    {
        SetDraggedPosition(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        SetDraggedPosition(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        SetDraggedPosition(eventData);
    }

    private void SetDraggedPosition(PointerEventData eventData)
    {
        var rt = gameObject.GetComponent<RectTransform>();

        // transform the screen point to world point int rectangle
        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, eventData.position, eventData.pressEventCamera, out globalMousePos))
        {
            rt.position = globalMousePos;
        }
    }

    void Start ()
    {
       
	}

	void Update ()
    {
	
	}
}
