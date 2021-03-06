﻿using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TinySM;
using UnityEngine;
using UnityEngine.UI;

public class StateElement : MonoBehaviour
{
	public EntryNode EntryNode;
	public ExitNode ExitNode;
	public Guid GUID => State != null ? State.GUID : default;

	public IState State;

	public void Delete()
	{
		PromptWindow.LevelInstance.Prompt("Delete this State?", new[] {
			("Delete", () => {
				Destroy(gameObject);
				State.DefinitionInterface.RemoveState(State);
			}),
			("Cancel", default(Action))
		});
	}

	public void Bind(IState obj)
	{
		State = obj;
		UiManager.LevelInstance.CurrentFile.StateMachineDefinition.AddState(obj);
		BindObject(obj, transform);

		if(State.TransitionInterfaces == null)
		{
			// State cannot have exit transitions
			ExitNode.gameObject.SetActive(false);
		}
	}

	public bool ShouldDisplay(MemberInfo info)
	{
		if(info.GetCustomAttribute<JsonIgnoreAttribute>() != null)
		{
			return false;
		}
		if (info.Name == "Name" && info.DeclaringType == typeof(TrackedObject))
		{
			return true;
		}
		var hierarchy = info.DeclaringType.InheritanceHierarchy();
		bool first = true;
		foreach (var type in hierarchy)
		{
			if(type.IsGenericType && type.GetGenericTypeDefinition() == typeof(State<,>))
			{
				return false;
			}
			if(type == typeof(TrackedObject))
			{
				return false;
			}
			if (first && type == info.DeclaringType)
			{
				break;
			}
			first = false;
		}
		return true;
	}

	private void BindObject(object obj, Transform container)
	{
		if(obj == null)
		{
			Debug.LogError($"Object was null");
			return;
		}
		var type = obj.GetType();
		var members = type.GetMembers().Where(m => m is PropertyInfo || m is FieldInfo);
		//Debug.Log($"Gathered members from {type}:\n{string.Join("\n", members.Select(m => $"{m.Name}:{m.DeclaringType}"))}");
		foreach (var member in members)
		{
			var memberType = member.GetMemberType();
			if(!ShouldDisplay(member))
			{
				//Debug.Log($"Skipping member {member.Name}({memberType})");
				continue;
			}
			//Debug.Log($"Processing member {member.Name}({memberType})");
			var fieldPrefab = UiManager.LevelInstance.GetFieldForType(member.GetMemberType());
			if (fieldPrefab == null)
			{
				if(!memberType.IsPrimitive)
				{
					var group = Instantiate(UiReferenceTracker.LevelInstance.StateDivider);
					group.transform.SetParent(container);
					group.transform.localScale = Vector3.one;
					group.transform.SetAsLastSibling();
					var text = group.GetComponentInChildren<Text>();
					text.text = member.Name;
					BindObject(member.GetValue(obj), group.transform);
				}
				continue;
			}
			var fieldGO = Instantiate(fieldPrefab.GameObject);
			fieldGO.transform.SetParent(container);
			fieldGO.transform.localScale = Vector3.one;
			var field = fieldGO.GetComponent(typeof(IFieldElement)) as IFieldElement;
			field.Bind(member, obj);
		}
	}
}

