using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragScroll : MonoBehaviour
{
    public void OnDrag(BaseEventData data)
    {
        var pointerData = data as PointerEventData;
        if (pointerData.dragging)
        {
            transform.position += (Vector3)pointerData.delta;
        }
    }
}
