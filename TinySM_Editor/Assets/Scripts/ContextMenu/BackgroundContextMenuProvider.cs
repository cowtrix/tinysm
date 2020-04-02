using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TinySM;
using UnityEngine;
using UnityEngine.EventSystems;

public class BackgroundContextMenuProvider : ContextMenuProvider
{
	private void NewState(Vector2 worldPos, Type t)
	{
		var refs = UiReferenceTracker.LevelInstance;
		var obj = Instantiate(refs.StatePrefab.gameObject);
		obj.transform.SetParent(refs.GraphContainer);
		obj.transform.localScale = Vector3.one;
		obj.transform.position = worldPos;
		var state = obj.GetComponent<StateElement>();
		state.Bind((IState)Activator.CreateInstance(t));
	}

	public override CustomContextMenu GetMenu(object context)
	{
		if (UiManager.LevelInstance.CurrentFile?.StateMachineDefinition == null)
		{
			return null;
		}
		return new CustomContextMenu(UiManager.LevelInstance.GetTypes<IState>().Select(
			t => new CustomContextMenu.InvokeItem { Label = $"New {t.Name}", Action = (pos) => NewState(pos, t) }));
	}
}
