using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI.Extensions;

public class ExitNode : MonoBehaviour
{
	public Vector2 LineOffset;
	public UILineRenderer LineRenderer;
	public StateElement State;
	public StateElement NextState;

	bool m_isDragging;
	NicerOutline m_outline;

	private void Awake()
	{
		m_outline = gameObject.GetComponent<NicerOutline>();
		m_outline.enabled = false;
	}

	public void OnDrag(BaseEventData data)
	{
		var pointerData = data as PointerEventData;
		NextState = pointerData.pointerCurrentRaycast.gameObject.GetComponent<EntryNode>()?.State;
		if(NextState?.GUID == State.GUID)
		{
			NextState = null;
		}
		m_isDragging = true;
		SetLine(pointerData.position);
		LineRenderer.SetAllDirty();
	}

	private void Update()
	{
		if(NextState)
			SetLine((Vector2)NextState.EntryNode.transform.position - LineOffset);
		else if(!m_isDragging && LineRenderer.Points.Any())
		{
			LineRenderer.Points = new Vector2[0]; 
		}
		m_outline.enabled = m_isDragging;

		if(!m_isDragging)
		{
			SetColor(UiManager.LevelInstance.Skin.NeutralColor);
		}
		else
		{
			if(NextState == null)
			{
				SetColor(UiManager.LevelInstance.Skin.BadColor);
			}
			else
			{
				SetColor(UiManager.LevelInstance.Skin.GoodColor);
			}
		}
		
	}

	void SetColor(Color c)
	{
		m_outline.effectColor = c;
		LineRenderer.color = c;
	}

	void SetLine(Vector3 worldDest)
	{
		var origin = LineOffset + (Vector2)transform.InverseTransformPoint(LineRenderer.transform.position);
		var destination = (Vector2)transform.InverseTransformPoint(worldDest);
		var halfpoint = (origin + destination) / 2;
		LineRenderer.Points = new Vector2[]
		{
			origin,
			new Vector2(halfpoint.x, origin.y),
			new Vector2(halfpoint.x, destination.y),
			destination
		};
	}

	public void EndDrag(BaseEventData data)
	{
		m_isDragging = false;
		var pointerData = data as PointerEventData;
		var node = pointerData.pointerCurrentRaycast.gameObject.GetComponent<EntryNode>();
		if (node == null)
		{
			NextState = null;
			LineRenderer.Points = new Vector2[0];
			LineRenderer.SetAllDirty();
			return;
		}
		NextState = node.State;
		if (NextState.GUID == State.GUID)
		{
			NextState = null;
		}
	}
}
