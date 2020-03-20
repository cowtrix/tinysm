using SFB;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TinySM;
using UnityEngine;
using UnityEngine.Events;

public class UiManager : LevelSingleton<UiManager>
{
	public enum EState
	{
		Init,
		Menu,
		EditSM,
	}

	[Serializable]
	public class UiStateEvent : UnityEvent<EState> { }
	[Serializable]
	public class StateUIDefinition
	{
		public EState State;
		public List<RectTransform> Enabled;
		public List<RectTransform> Disabled;
	}
	public class AssemblyData
	{
		public Assembly Assembly;
		public List<Type> Types = new List<Type>();
		public AssemblyData(Assembly assembly)
		{
			Assembly = assembly;
		}
	}
	public Action<Assembly> OnAssemblyLoaded;
	public List<AssemblyData> LoadedAssemblies = new List<AssemblyData>();
	public IUIHandler Handler;
	private Dictionary<Type, IFieldElement> m_fields;
	public List<StateUIDefinition> StateUIDefinitions;
	public UiStateEvent OnStateChanged;

	private UiReferenceTracker Refs => UiReferenceTracker.LevelInstance;

	private EState __state = EState.Init;
	public EState State 
	{ 
		get
		{
			return __state;
		}
		set
		{
			if(__state == value)
			{
				return;
			}
			__state = value;
			OnStateChanged.Invoke(__state);
		}
	}

	private void Awake()
	{
		m_fields = new Dictionary<Type, IFieldElement>();
		foreach (var fieldGO in Refs.Fields)
		{
			var field = fieldGO.GetComponent(typeof(IFieldElement)) as IFieldElement;
			m_fields[field.Type] = field;
		}
		OnStateChanged.AddListener(state =>
		{
			var config = StateUIDefinitions.SingleOrDefault(s => s.State == state);
			if(config == null)
			{
				return;
			}
			config.Enabled.ForEach(r => r.gameObject.SetActive(true));
			config.Disabled.ForEach(r => r.gameObject.SetActive(false));
		});
		State = EState.Menu;
	}

	public List<Type> GetStateTypes()
	{
		var result = new List<Type>();
		if(Handler != null)
		{
			result.AddRange(Handler.StateTypes);
		}
		return result;
	}

	public void NewState(Vector2 worldPos, Type t)
	{
		var obj = Instantiate(Refs.StatePrefab.gameObject);
		obj.transform.SetParent(Refs.GraphContainer);
		obj.transform.localScale = Vector3.one;
		obj.transform.position = worldPos;
		var state = obj.GetComponent<StateElement>();
		state.Bind((IState)Activator.CreateInstance(t));
	}

	public void LoadDLL()
	{
		var files = StandaloneFileBrowser.OpenFilePanel("Load DLL", "", "dll", true);
		foreach(var file in files)
		{
			var newAss = Assembly.LoadFile(file);
			var data = new AssemblyData(newAss);
			LoadedAssemblies.Add(data);
			Debug.Log($"Loaded {newAss.FullName}");
			var types = newAss.GetTypes();
			foreach(var type in types)
			{
				if(!typeof(ITrackedObject).IsAssignableFrom(type) || type.IsInterface || type.IsAbstract)
				{
					continue;
				}
				if(type.IsValueType)
				{
					PromptWindow.LevelInstance.Prompt($"Could not load type {type} as it was a struct");
				}
				Debug.Log("Discovered tracked type " + type);
				if(Handler != null)
				{
					Handler.TryAddType(type);
				}
				data.Types.Add(type);
			}
			OnAssemblyLoaded?.Invoke(newAss);
		}
	}

	public IFieldElement GetFieldForType(Type type)
	{
		if(m_fields.TryGetValue(type, out var field))
		{
			return field;
		}
		var valid = m_fields.Where(kvp => kvp.Key.IsAssignableFrom(type))
			.OrderByDescending(kvp => kvp.Key.InheritanceHierarchy().Count());
		return valid.FirstOrDefault().Value; 
	}
}
