using Newtonsoft.Json;
using SFB;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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

	public EditorFile CurrentFile;
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
		Toolbar.LevelInstance.Add("New", () => CurrentFile = new EditorFile());
		Toolbar.LevelInstance.Add("Open", LoadFile);
		Toolbar.LevelInstance.Add("Save", SaveFile, () => CurrentFile != null);
	}

	private void LoadFile()
	{
		var file = StandaloneFileBrowser.OpenFilePanel("Load Editor File", "", "tse", false).SingleOrDefault();
		if(!File.Exists(file))
		{
			return;
		}
		CurrentFile = JsonConvert.DeserializeObject<EditorFile>(File.ReadAllText(file), new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto, Formatting = Formatting.Indented });
	}

	private void SaveFile()
	{
		if(CurrentFile == null)
		{
			Debug.LogError("Tried to save a null handler");
			return;
		}
		var file = StandaloneFileBrowser.SaveFilePanel("Save Editor File", "", "untitled", "tse");
		if(string.IsNullOrEmpty(file))
		{
			return;
		}
		File.WriteAllText(file, JsonConvert.SerializeObject(CurrentFile, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto, Formatting = Formatting.Indented }));
		if(!File.Exists(file))
		{
			PromptWindow.LevelInstance.Prompt($"Failed to save file to {file}");
		}
	}

	public List<Type> GetTypes<T>() where T: ITrackedObject
	{
		var result = new List<Type>();
		if(CurrentFile != null)
		{
			result.AddRange(CurrentFile.GetTypes<T>());
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

	public void LoadDLLFromFile()
	{
		var files = StandaloneFileBrowser.OpenFilePanel("Load DLL", "", "dll", true);
		foreach(var file in files)
		{
			CurrentFile.LoadDLL(file);
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

	public void NewStateMachine()
	{
		/*ObjectPicker.LevelInstance.Pick(GetTypes<IStateMachineDefinition>(),
			t =>
			{
				var typeHierarchy = t.InheritanceHierarchy().ToList();
				var lastGeneric = typeHierarchy.Last(lastGen => lastGen.IsConstructedGenericType);
				var handlerType = typeof(EditorFile<,>).MakeGenericType(lastGeneric.GetGenericArguments());
				CurrentFile = (IEditorFile)Activator.CreateInstance(handlerType);
				CurrentFile.DefinitionInterface = (IStateMachineDefinition)Activator.CreateInstance(t);
				State = EState.EditSM;
			});*/
	}
}
