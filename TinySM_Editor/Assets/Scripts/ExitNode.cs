using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI.Extensions;

public class ExitNode : MonoBehaviour
{
	public StateElement State;
	public StateElement NextState;

	public GameObject TransitionPrefab;
	public List<TransitionElement> TransitionNodes = new List<TransitionElement>();

	private TransitionElement m_tempTransition;
	bool m_isDragging;
	NicerOutline m_outline;

	private void Awake()
	{
		m_outline = gameObject.GetComponent<NicerOutline>();
		m_outline.enabled = false;
	}

	public void OnDragStart(BaseEventData data)
	{
		var pointerData = data as PointerEventData;
		if (m_tempTransition != null)
		{
			Destroy(m_tempTransition.gameObject);
		}
		m_tempTransition = Instantiate(TransitionPrefab).GetComponent<TransitionElement>();
		m_tempTransition.transform.SetParent(transform);
		m_tempTransition.transform.localPosition = Vector3.zero;
		m_tempTransition.transform.localScale = Vector3.one;
		m_tempTransition.gameObject.SetActive(true);
		m_tempTransition.SetData(State, null, null);
	}

	public void OnDrag(BaseEventData data)
	{
		var pointerData = data as PointerEventData;
		NextState = pointerData.pointerCurrentRaycast.gameObject.GetComponent<EntryNode>()?.State;
		if(NextState?.GUID == State.GUID)
		{
			NextState = null;
		}
		if(NextState)
		{ 
			m_tempTransition.SetColor(UiReferenceTracker.LevelInstance.Skin.GoodColor);
		}
		else
		{
			m_tempTransition.SetColor(UiReferenceTracker.LevelInstance.Skin.BadColor);
		}
		m_isDragging = true;
		m_tempTransition.SetLine(pointerData.position);
	}

	public void EndDrag(BaseEventData data)
	{
		m_isDragging = false;
		var pointerData = data as PointerEventData;
		var node = pointerData.pointerCurrentRaycast.gameObject.GetComponent<EntryNode>();
		if (node == null)
		{
			NextState = null;
			Destroy(m_tempTransition.gameObject);
			m_tempTransition = null;
			return;
		}
		NextState = node.State;
		if (NextState.GUID == State.GUID)
		{
			NextState = null;
		}
		TransitionNodes.Add(m_tempTransition);
		m_tempTransition.SetData(State, node, UiManager.LevelInstance.Handler.DefaultTransition);
		m_tempTransition = null;
	}
}
