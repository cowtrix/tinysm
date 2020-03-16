using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragScroll : MonoBehaviour
{
    public float ScrollSpeed = 1;
    public Vector2 Scale = new Vector2(0.5f, 1);
    public void OnDrag(BaseEventData data)
    {
        var pointerData = data as PointerEventData;
        if (pointerData.dragging)
        {
            transform.position += (Vector3)pointerData.delta;
        }
    }

    public void OnScroll(BaseEventData data)
    {
        var pointerData = data as PointerEventData;
        transform.localScale += Vector3.one * pointerData.scrollDelta.y * Time.deltaTime;
        transform.localScale = new Vector3(
            Mathf.Clamp(transform.localScale.x, Scale.x, Scale.y),
            Mathf.Clamp(transform.localScale.y, Scale.x, Scale.y),
            Mathf.Clamp(transform.localScale.z, Scale.x, Scale.y));
    }
}
