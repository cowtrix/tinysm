using System.Collections;
using System.Collections.Generic;
using TinySM;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class TransitionElement : MonoBehaviour
{
	public Button Edit;
	public RectTransform Panel;
	public Vector2 LineOffset;
	public UILineRenderer LineStart, LineEnd;
	public Text Text;
	private StateElement m_state;
	private EntryNode m_destination;
	private ITransition m_transition;

	public void SetData(StateElement state, EntryNode destination, ITransition transition)
	{
		m_state = state;
		transform.SetParent(state.transform);
		transform.localPosition = Vector3.zero;
		m_transition = transition;
		m_destination = destination;
	}

	private void Update()
	{
		if (m_transition == null)
		{
			Panel.gameObject.SetActive(false);
		}
		else
		{
			Panel.gameObject.SetActive(true);
			if(m_transition.ConditionInterface != null)
			{
				Text.text = m_transition.ConditionInterface.ToString();
			}
		}
		if (m_destination != null)
		{
			SetLine(m_destination.transform.position);
			SetColor(UiManager.LevelInstance.Skin.NeutralColor);
		}
	}

	public void SetColor(Color c)
	{
		LineStart.color = c;
		LineEnd.color = c;
	}

	public void SetLine(Vector3 worldDest)
	{
		var origin = LineOffset + (Vector2)LineStart.transform.InverseTransformPoint(m_state.ExitNode.transform.position);
		var destination = (Vector2)LineStart.transform.InverseTransformPoint(worldDest);
		var halfpoint = (origin + destination) / 2;
		Panel.localPosition = halfpoint;
		LineStart.Points = new Vector2[]
		{
			origin,
			origin + LineOffset,
			new Vector2(halfpoint.x, origin.y),
			halfpoint
		};
		LineEnd.Points = new Vector2[]
		{
			halfpoint,
			new Vector2(halfpoint.x, destination.y),
			destination - LineOffset,
			destination
		};
		if(m_transition != null)
		{
			LineEnd.LineList = m_transition?.ConditionInterface == null;
		}
	}
}
