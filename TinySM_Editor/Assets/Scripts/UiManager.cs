using SFB;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TinySM;
using UnityEngine;

public class UiManager : LevelSingleton<UiManager>
{
	public StateElement StatePrefab;
	public RectTransform GraphContainer;
	public IUIHandler Handler;
	public Skin Skin;
	public GameObject StateDivider;
	public List<GameObject> Fields;
	private Dictionary<Type, IFieldElement> m_fields;

	public TextEditorWindow Export;

	private void Awake()
	{
		m_fields = new Dictionary<Type, IFieldElement>();
		foreach (var fieldGO in Fields)
		{
			var field = fieldGO.GetComponent(typeof(IFieldElement)) as IFieldElement;
			m_fields[field.Type] = field;
		}
		Handler = new UIHandler<UserMessage, ResponseMessage>(new BotSMDefinition());
	}

	public void NewState(Vector2 worldPos, Type t)
	{
		var obj = Instantiate(StatePrefab.gameObject);
		obj.transform.SetParent(GraphContainer);
		obj.transform.localScale = Vector3.one;
		obj.transform.position = worldPos;
		var state = obj.GetComponent<StateElement>();
		state.Bind((IState)Activator.CreateInstance(t));
	}

	public void LoadDLL()
	{
		var files = StandaloneFileBrowser.OpenFilePanel("Load DLL", "", "dll", true);
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
